﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Caching;
using System.Web.Mvc;
using DoddleReport;
using DoddleReport.Web;
using NgTrade.Models.Data;
using NgTrade.Models.Repo.Interface;
using NgTrade.Models.ViewModel;

namespace NgTrade.Controllers
{
    public class HomeController : Controller
    {
        private readonly IQuoteRepository _quoteRepository;

        public HomeController(IQuoteRepository quoteRepository)
        {
            _quoteRepository = quoteRepository;
        }

        public ActionResult Index()
        {
            List<Quote> dayLosersList;
            var dayLosersListCacheModel = HttpContext.Cache.Get("dayLosersListCache") as IEnumerable<Quote>;
            if (dayLosersListCacheModel != null)
            {
                dayLosersList = dayLosersListCacheModel.ToList();
            }
            else
            {
                dayLosersList = _quoteRepository.GetTopFiveMarketLosersToday();
                var expireMins = Int32.Parse(ConfigurationManager.AppSettings["CacheExpireMins"]);
                HttpContext.Cache.Add("dayLosersListCache", dayLosersList, null,
                                      DateTime.Now.AddMinutes(expireMins), Cache.NoSlidingExpiration,
                                      CacheItemPriority.Normal, null);
            }

            List<Quote> dayGainersList;
            var dayGainersListCacheModel = HttpContext.Cache.Get("dayGainersListCache") as IEnumerable<Quote>;
            if (dayGainersListCacheModel != null)
            {
                dayGainersList = dayGainersListCacheModel.ToList();
            }
            else
            {
                dayGainersList = _quoteRepository.GetTopFiveMarketGainersToday();
                var expireMins = Int32.Parse(ConfigurationManager.AppSettings["CacheExpireMins"]);
                HttpContext.Cache.Add("dayGainersListCache", dayGainersList, null,
                                      DateTime.Now.AddMinutes(expireMins), Cache.NoSlidingExpiration,
                                      CacheItemPriority.Normal, null);
            }

            DateTime stockDay;
            var stockDayCacheModel = HttpContext.Cache.Get("stockDayCache") as DateTime?;
            if (stockDayCacheModel != null)
            {
                stockDay = stockDayCacheModel.GetValueOrDefault();
            }
            else
            {
                stockDay = _quoteRepository.GetCurrentStockDay();
                var expireMins = Int32.Parse(ConfigurationManager.AppSettings["CacheExpireMins"]);
                HttpContext.Cache.Add("stockDayCache", stockDay, null,
                                      DateTime.Now.AddMinutes(expireMins), Cache.NoSlidingExpiration,
                                      CacheItemPriority.Normal, null);
            }

            var dayLosers = dayLosersList;
            var dayGainers = dayGainersList;
            var quoteDay = stockDay;
            var sQuoteDay = String.Format("{0:ddd, MMM d, yyyy}", quoteDay);
            var homeViewModel = new HomeViewModel
            {
                DayGainers = dayGainers,
                DayLosers = dayLosers,
                SQuoteDay = sQuoteDay
            };
            return View(homeViewModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Faq()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Terms()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Privacy()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [OutputCache(CacheProfile = "StaticPageCache")]
        public ReportResult DailyPriceList()
        {
            List<Quote> dayList;
            var dayListCacheModel = HttpContext.Cache.Get("dayListCache") as IEnumerable<Quote>;
            if (dayListCacheModel != null)
            {
                dayList = dayListCacheModel.ToList();
            }
            else
            {
                dayList = _quoteRepository.GetDayList();
                var expireMins = Int32.Parse(ConfigurationManager.AppSettings["CacheExpireMins"]);
                HttpContext.Cache.Add("dayListCache", dayList, null,
                                      DateTime.Now.AddMinutes(expireMins), Cache.NoSlidingExpiration,
                                      CacheItemPriority.Normal, null);
            }

            DateTime stockDay;
            var stockDayCacheModel = HttpContext.Cache.Get("stockDayCache") as DateTime?;
            if (stockDayCacheModel != null)
            {
                stockDay = stockDayCacheModel.GetValueOrDefault();
            }
            else
            {
                stockDay = _quoteRepository.GetCurrentStockDay();
                var expireMins = Int32.Parse(ConfigurationManager.AppSettings["CacheExpireMins"]);
                HttpContext.Cache.Add("stockDayCache", stockDay, null,
                                      DateTime.Now.AddMinutes(expireMins), Cache.NoSlidingExpiration,
                                      CacheItemPriority.Normal, null);
            }
            // Get the data for the report (any IEnumerable will work)
            var query = dayList;
            var dayQuote = stockDay;

            // Create the report and turn our query into a ReportSource
            var report = new Report(query.ToReportSource());


            // Customize the Text Fields
            report.TextFields.Title =
                "Nigerian Stock Exchange Trading Online NSE Daily Price List - NgTradeOnline NSE Daily Price List";
            report.TextFields.SubTitle = "This is the daily price list for " +
                                         string.Format("{0:ddd, MMM d, yyyy}", dayQuote);
            report.TextFields.Footer = "Copyright 2011 &copy; NgTradeOnline";

            // Render hints allow you to pass additional hints to the reports as they are being rendered
            report.RenderHints.BooleanCheckboxes = true;

            // Customize the data fields
            report.DataFields["ChangeTracker"].Hidden = true;
            report.DataFields["QuoteId"].Hidden = true;
            report.DataFields["Date"].Hidden = true;
            report.DataFields["Close"].DataFormatString = "{0:N}";
            report.DataFields["Open"].DataFormatString = "{0:N}";
            report.DataFields["Open"].HeaderText = "OPEN";
            report.DataFields["Low"].DataFormatString = "{0:N}";
            report.DataFields["High"].DataFormatString = "{0:N}";
            report.DataFields["CHANGE1"].DataFormatString = "{0:N}";
            report.DataFields["CHANGE1"].HeaderText = "CHANGE";

            // Return the ReportResult
            // the type of report that is rendered will be determined by the extension in the URL (.pdf, .xls, .html, etc)
            return new ReportResult(report);
        }

        public static string ExportCsv<T>(List<T> list)
        {
            return Helpers.FileExtension.GetCsv(list);
        }

        public ActionResult ClearCache()
        {
            Response.RemoveOutputCacheItem("/");

            HttpContext.Cache.Remove("dayListCache");
            HttpContext.Cache.Remove("dayGainersCache");
            HttpContext.Cache.Remove("dayLosersCache");
            HttpContext.Cache.Remove("dayLosersListCache");
            HttpContext.Cache.Remove("dayGainersListCache");
            HttpContext.Cache.Remove("stockDayCache");

            return RedirectToAction("Index", "Home");
        }
    }
}
