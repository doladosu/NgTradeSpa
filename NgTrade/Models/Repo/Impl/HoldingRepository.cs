using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Caching;
using NgTrade.Models.Data;
using NgTrade.Models.Info;
using NgTrade.Models.Repo.Interface;

namespace NgTrade.Models.Repo.Impl
{
    public class HoldingRepository : IHoldingRepository
    {
        private const string AllHoldingsCacheKey = "AllHoldingsCache";
        private static readonly object CacheLockObjectCurrentSales = new object();


        public List<Holding> GetHoldings(int accountId)
        {
            var allHoldings = GetAllHoldings();
            return allHoldings.Where(q => q.AccountId == accountId).OrderByDescending(q => q.DatePurchased).ToList();
        }

        public List<Holding> GetAllHoldings()
        {
            try
            {
                var cache = MemoryCache.Default;
                var result = cache[AllHoldingsCacheKey] as List<Holding>;

                if (result == null)
                {
                    lock (CacheLockObjectCurrentSales)
                    {
                        result = cache[AllHoldingsCacheKey] as List<Holding>;

                        var policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(5) };

                        if (result == null)
                        {
                            using (var db = new UsersContext())
                            {
                                result = db.Holdings.ToList();
                            }
                            cache.Add(AllHoldingsCacheKey, result, policy);
                        }
                    }
                }
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Holding CreateHolding(OrderModel orderModel)
        {
            var acctProfile =  orderModel.UserProfile;
            var orderTotal = orderModel.Price * orderModel.Shares;
            bool shareExists = false;
            var allHoldings = GetAllHoldings();
            var hld = allHoldings.FirstOrDefault(h => h.Symbol == orderModel.Symbol && h.AccountId == acctProfile.UserId);
            if (hld != null)
            {
                shareExists = true;
            }

            if (orderModel.Action.ToLower() == "buy")
            {
                var bal = Convert.ToDouble(acctProfile.Balance.GetValueOrDefault());
                if (bal < orderTotal)
                {
                    return null;
                }
            }
            else
            {
                if (hld == null)
                {
                    return null;
                }
                if (hld.Quantity < orderModel.Shares)
                {
                    return null;
                }
            }
            try
            {
                if (shareExists)
                {
                    if (orderModel.Action.ToLower() == "buy")
                    {
                        hld.Quantity = hld.Quantity + orderModel.Shares;
                        var hlds =
                            allHoldings.Where(
                                h => h.Symbol == orderModel.Symbol && h.AccountId == acctProfile.UserId).ToList();
                        var calcPrice = hlds.Aggregate(orderModel.Price, (current, holding) => current + Convert.ToDouble(holding.Price));
                        hld.Price = Convert.ToDecimal(calcPrice/(hlds.Count + 1));
                    }
                    else
                    {
                        hld.Quantity = hld.Quantity - orderModel.Shares;
                    }
                }
                else
                {
                    hld = new Holding
                    {
                        Quantity = orderModel.Shares,
                        AccountId = orderModel.UserProfile.UserId,
                        DatePurchased = DateTime.Now,
                        Price = Convert.ToDecimal(orderModel.Price),
                        Symbol = orderModel.Symbol
                    };
                }
                if (orderModel.Action.ToLower() == "buy")
                {
                    acctProfile.Balance = acctProfile.Balance.GetValueOrDefault() - Convert.ToDecimal(orderTotal);
                }
                else
                {
                    acctProfile.Balance = acctProfile.Balance.GetValueOrDefault() + Convert.ToDecimal(orderTotal);
                }
                using (var db = new UsersContext())
                {
                    db.Entry(acctProfile).State = EntityState.Modified;
                    if (shareExists)
                    {
                        db.Entry(hld).State = EntityState.Modified;
                    }
                    else
                    {
                        db.Holdings.Add(hld);
                    }
                    db.SaveChanges();
                }
                return hld;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}