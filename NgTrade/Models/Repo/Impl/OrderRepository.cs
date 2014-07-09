using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Caching;
using NgTrade.Models.Data;
using NgTrade.Models.Repo.Interface;

namespace NgTrade.Models.Repo.Impl
{
    public class OrderRepository : IOrderRepository
    {
        private const string AllOrdersCache = "AllOrdersCache";
        private static readonly object CacheLockObjectCurrentOrders = new object();

        public Order CreateOrder(Order order)
        {
            try
            {
                using (var db = new UsersContext())
                {
                    db.Orders.Add(order);
                    db.SaveChanges();
                }
                return order;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void UpdateOrder(Order order)
        {
            using (var db = new UsersContext())
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public List<Order> GetOrders(int userId)
        {
            var allOrders = GetAllOrders();
            return allOrders.Where(q => q.AccountId == userId).OrderByDescending(q => q.Id).ToList();
        }

        public List<Order> GetAllOrders()
        {
            try
            {
                var cache = MemoryCache.Default;
                var result = cache[AllOrdersCache] as List<Order>;

                if (result == null)
                {
                    lock (CacheLockObjectCurrentOrders)
                    {
                        result = cache[AllOrdersCache] as List<Order>;

                        var policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(5) };

                        if (result == null)
                        {
                            using (var db = new UsersContext())
                            {
                                result = db.Orders.ToList();
                            }
                            cache.Add(AllOrdersCache, result, policy);
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
    }
}