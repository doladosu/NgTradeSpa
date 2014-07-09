﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using NgTrade.Models.Info;

namespace NgTrade.DataHub
{
    public class StockTicker
    {
        // Singleton instance
        private static readonly Lazy<StockTicker> LazyInstance = new Lazy<StockTicker>(
            () => new StockTicker(GlobalHost.ConnectionManager.GetHubContext<StockTickerHub>().Clients));

        private readonly object _marketStateLock = new object();
        private readonly object _updateStockPricesLock = new object();

        private readonly ConcurrentDictionary<string, Stock> _stocks = new ConcurrentDictionary<string, Stock>();

        // Stock can go up or down by a percentage of this factor on each change
        private const double RangePercent = 0.002;

        private readonly TimeSpan _updateInterval = TimeSpan.FromMilliseconds(250);
        private readonly Random _updateOrNotRandom = new Random();

        private Timer _timer;
        private volatile bool _updatingStockPrices;
        private volatile MarketState _marketState;

        private StockTicker(IHubConnectionContext<dynamic> clients)
        {
            Clients = clients;
            LoadDefaultStocks();
        }

        public static StockTicker Instance
        {
            get { return LazyInstance.Value; }
        }

        private IHubConnectionContext<dynamic> Clients { get; set; }

        public MarketState MarketState
        {
            get { return _marketState; }
            private set { _marketState = value; }
        }

        public IEnumerable<Stock> GetAllStocks()
        {
            return _stocks.Values;
        }

        public void OpenMarket()
        {
            lock (_marketStateLock)
            {
                if (MarketState == MarketState.Open) return;
                _timer = new Timer(UpdateStockPrices, null, _updateInterval, _updateInterval);

                MarketState = MarketState.Open;

                BroadcastMarketStateChange(MarketState.Open);
            }
        }

        public void CloseMarket()
        {
            lock (_marketStateLock)
            {
                if (MarketState != MarketState.Open) return;
                if (_timer != null)
                {
                    _timer.Dispose();
                }

                MarketState = MarketState.Closed;

                BroadcastMarketStateChange(MarketState.Closed);
            }
        }

        public void Reset()
        {
            lock (_marketStateLock)
            {
                if (MarketState != MarketState.Closed)
                {
                    throw new InvalidOperationException("Market must be closed before it can be reset.");
                }

                LoadDefaultStocks();
                BroadcastMarketReset();
            }
        }

        private void LoadDefaultStocks()
        {
            _stocks.Clear();

            var stocks = new List<Stock>
            {
                new Stock {Symbol = "MSFT", Price = 30.31m},
                new Stock {Symbol = "APPL", Price = 578.18m},
                new Stock {Symbol = "GOOG", Price = 570.30m}
            };

            stocks.ForEach(stock => _stocks.TryAdd(stock.Symbol, stock));
        }

        private void UpdateStockPrices(object state)
        {
            // This function must be re-entrant as it's running as a timer interval handler
            lock (_updateStockPricesLock)
            {
                if (_updatingStockPrices) return;
                _updatingStockPrices = true;

                foreach (var stock in _stocks.Values.Where(TryUpdateStockPrice))
                {
                    BroadcastStockPrice(stock);
                }

                _updatingStockPrices = false;
            }
        }

        private bool TryUpdateStockPrice(Stock stock)
        {
            // Randomly choose whether to udpate this stock or not
            var r = _updateOrNotRandom.NextDouble();
            if (r > 0.1)
            {
                return false;
            }

            // Update the stock price by a random factor of the range percent
            var random = new Random((int) Math.Floor(stock.Price));
            var percentChange = random.NextDouble()*RangePercent;
            var pos = random.NextDouble() > 0.51;
            var change = Math.Round(stock.Price*(decimal) percentChange, 2);
            change = pos ? change : -change;

            stock.Price += change;
            return true;
        }

        private void BroadcastMarketStateChange(MarketState marketState)
        {
            switch (marketState)
            {
                case MarketState.Open:
                    Clients.All.marketOpened();
                    break;
                case MarketState.Closed:
                    Clients.All.marketClosed();
                    break;
            }
        }

        private void BroadcastMarketReset()
        {
            Clients.All.marketReset();
        }

        private void BroadcastStockPrice(Stock stock)
        {
            Clients.All.updateStockPrice(stock);
        }
    }

    public enum MarketState
    {
        Closed,
        Open
    }
}