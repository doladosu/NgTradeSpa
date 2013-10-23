﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using NgTrade.Models.Data;
using NgTrade.Models.Info;
using NgTrade.Models.Repo.Interface;

namespace NgTrade.Models.Repo.Impl
{
    public class QuoteRepository : IQuoteRepository
    {
        private static readonly object CacheLockObjectCurrentSales = new object();
        private const string ALL_QUOTES_CACHE_KEY = "AllQuotesCache";
        private const string ALL_COMPANIES_CACHE_KEY = "AllCompaniesCache";

        public List<Quote> GetTopFiveMarketLosersToday()
        {
            var allQuotes = GetAllQuotes();
            var dateTimeQuote = allQuotes.OrderByDescending(q => q.Date).FirstOrDefault();
            var quotes = allQuotes.Where(q => q.Close < q.Open && dateTimeQuote != null && q.Date == dateTimeQuote.Date).OrderBy(q => q.Change1);
            return quotes.Take(5).ToList();
        }

        public List<Quote> GetTopFiveMarketGainersToday()
        {
            var allQuotes = GetAllQuotes();
            var dateTimeQuote = allQuotes.OrderByDescending(q => q.Date).FirstOrDefault();
            var quotes = allQuotes.Where(q => q.Close > q.Open && dateTimeQuote != null && q.Date == dateTimeQuote.Date).OrderByDescending(q => q.Change1);
            return quotes.Take(5).ToList();
        }

        public Quote GetQuote(string symbol)
        {
            var allQuotes = GetAllQuotes();
            var quotes = allQuotes.Where(q => q.Symbol.ToLower().Trim() == symbol.ToLower().Trim());
            var dateTime = (quotes.Select(q => q.Date)).Max();
            return allQuotes.FirstOrDefault(q => q.Symbol == symbol && q.Date == dateTime);
        }

        public List<Quote> GetQuoteList(string symbol)
        {
            var allQuotes = GetAllQuotes();
            var quotes = allQuotes.Where(q => q.Symbol.ToLower().Trim() == symbol.ToLower().Trim());
            return quotes.ToList();
        }

        public DateTime GetCurrentStockDay()
        {
            var allQuotes = GetAllQuotes();
            var dateTimeQuote = allQuotes.OrderByDescending(q => q.Date).FirstOrDefault();
            if (dateTimeQuote != null)
            {
                return dateTimeQuote.Date;
            }
            return DateTime.Now;
        }

        public List<Quote> GetDayList()
        {
            var allQuotes = GetAllQuotes();
            var dateTimeQuote = allQuotes.OrderByDescending(q => q.Date).FirstOrDefault();
            var quotes = allQuotes.Where(q => q.Date == dateTimeQuote.Date).OrderBy(q => q.Symbol);
            return quotes.ToList();
        }

        public Companyprofile GetCompany(string symbol)
        {
            var allCompanies = GetAllCompanies();
            var quotes = allCompanies.Where(q => q.Symbol.ToLower().Trim() == symbol.ToLower().Trim());
            return quotes.FirstOrDefault();
        }

        public List<Companyprofile> GetCompanies()
        {
            var allCompanies = GetAllCompanies();
            return allCompanies.ToList();
        }

        public List<QuoteSector> GetDaysListWithSector()
        {
            var allQuotes = GetAllQuotes();
            var allCompanies = GetAllCompanies();
            var dateTimeQuote = allQuotes.OrderByDescending(q => q.Date).FirstOrDefault();

            var items = (from e in allQuotes.Where(q => q.Date == dateTimeQuote.Date).ToList()
                         join a in allCompanies
                             on e.Symbol equals a.Symbol into result
                         from a in result.DefaultIfEmpty()
                         select new {e, a}).Select(quote => new QuoteSector()
                                                                {
                                                                    Category =
                                                                        (quote.a != null)
                                                                            ? quote.a.Category
                                                                            : "",
                                                                    Change1 = quote.e.Change1,
                                                                    Close = quote.e.Close,
                                                                    Date = quote.e.Date,
                                                                    High = quote.e.High,
                                                                    Low = quote.e.Low,
                                                                    Open = quote.e.Open,
                                                                    QuoteId = quote.e.QuoteId,
                                                                    Symbol = quote.e.Symbol,
                                                                    Trades = quote.e.Trades,
                                                                    Volume = quote.e.Volume
                                                                }).ToList();
            return items;

        }

        public List<Quote> GetAllQuotes()
        {
            try
            {
                var cache = MemoryCache.Default;
                var result = cache[ALL_QUOTES_CACHE_KEY] as List<Quote>;

                if (result == null)
                {
                    lock (CacheLockObjectCurrentSales)
                    {
                        result = cache[ALL_QUOTES_CACHE_KEY] as List<Quote>;

                        var policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(15) };

                        if (result == null)
                        {
                            using (var db = new UsersContext())
                            {
                                result = db.Quotes.ToList();
                            }
                            cache.Add(ALL_QUOTES_CACHE_KEY, result, policy);
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

        public List<Companyprofile> GetAllCompanies()
        {
            try
            {
                var cache = MemoryCache.Default;
                var result = cache[ALL_COMPANIES_CACHE_KEY] as List<Companyprofile>;

                if (result == null)
                {
                    lock (CacheLockObjectCurrentSales)
                    {
                        result = cache[ALL_COMPANIES_CACHE_KEY] as List<Companyprofile>;

                        var policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(15) };

                        if (result == null)
                        {
                            using (var db = new UsersContext())
                            {
                                result = db.Companyprofiles.ToList();
                            }
                            cache.Add(ALL_QUOTES_CACHE_KEY, result, policy);
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