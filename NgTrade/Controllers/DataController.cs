using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Breeze.ContextProvider;
using Breeze.WebApi2;
using iTextSharp.text;
using Newtonsoft.Json.Linq;
using NgTrade.Models.Data;
using NgTrade.Models.Info;
using NgTrade.Models.Repo.Impl;
using NgTrade.Models.Repo.Interface;

namespace NgTrade.Controllers
{
        [BreezeController]
    public class DataController : ApiController
    {
        private readonly INgTradeRepository _ngTradeRepository;

        public DataController(INgTradeRepository ngTradeRepository)
        {
            _ngTradeRepository = ngTradeRepository;
        }

        [HttpGet]
        public string Metadata()
        {
            return _ngTradeRepository.Metadata;
        }

        [HttpPost]
        public SaveResult SaveChanges(JObject saveBundle)
        {
            return _ngTradeRepository.SaveChanges(saveBundle);
        }

        [HttpGet]
        public IQueryable<AccountProfile> AccountProfiles()
        {
            return _ngTradeRepository.AccountProfiles;
        }

        [HttpGet]
        public IQueryable<Dailypricelist> Dailypricelists()
        {
            return _ngTradeRepository.Dailypricelists;
        }

        [HttpGet]
        public IQueryable<Companyprofile> Companyprofiles()
        {
            return _ngTradeRepository.Companyprofiles;
        }

        [HttpGet]
        public IQueryable<Holding> Holdings()
        {
            return _ngTradeRepository.Holdings;
        }

        [HttpGet]
        public IQueryable<MailingList> MailingLists()
        {
            return _ngTradeRepository.MailingLists;
        }

        [HttpGet]
        public IQueryable<News> News()
        {
            return _ngTradeRepository.News;
        }

        [HttpGet]
        public IQueryable<Order> Orders()
        {
            return _ngTradeRepository.Orders;
        }

        [HttpGet]
        public IQueryable<Quote> Quotes()
        {
            return _ngTradeRepository.Quotes;
        }

        [HttpGet]
        public IQueryable<StockChart> QuoteUtcDate()
        {
            var quotes = _ngTradeRepository.Quotes.ToList().Where(e => e.Date > DateTime.Now.AddMonths(-6));
            var stockChartsList = quotes.Select(quote => new StockChart {A = GetTime(quote.Date), B = quote.Open, C = quote.High, D = quote.Low, E = quote.Close, Symbol = quote.Symbol}).ToList();
            return stockChartsList.AsQueryable();
        }

        private static Int64 GetTime(DateTime date)
        {
            var st = new DateTime(1970, 1, 1);
            var t = (date.ToUniversalTime() - st);
            var retval = (Int64)(t.TotalMilliseconds + 0.5);
            return retval;
        }

        [HttpGet]
        public IQueryable<UserProfile> UserProfiles()
        {
            return _ngTradeRepository.UserProfiles;
        }

        [HttpGet]
        public object Lookups()
        {
            var accountProfiles = _ngTradeRepository.AccountProfiles;
            var dailypricelists = _ngTradeRepository.Dailypricelists;
            var companyprofiles = _ngTradeRepository.Companyprofiles;
            var holdings = _ngTradeRepository.Holdings;
            var mailingLists = _ngTradeRepository.MailingLists;
            var news = _ngTradeRepository.News;
            var orders = _ngTradeRepository.Orders;
            var quotes = _ngTradeRepository.Quotes;
            var userProfiles = _ngTradeRepository.UserProfiles;

            return
                new
                {
                    accountProfiles,
                    dailypricelists,
                    companyprofiles,
                    holdings,
                    mailingLists,
                    news,
                    orders,
                    quotes,
                    userProfiles
                };
        }
    }
}
