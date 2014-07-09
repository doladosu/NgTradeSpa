using System.Linq;
using System.Web.Http;
using Breeze.ContextProvider;
using Breeze.WebApi2;
using Newtonsoft.Json.Linq;
using NgTrade.Models.Data;
using NgTrade.Models.Repo.Impl;
using NgTrade.Models.Repo.Interface;

namespace NgTrade.Controllers
{
        [BreezeController]
    public class DataController : ApiController
    {
        private readonly INgTradeRepository _ngTradeRepository;

        public DataController()
        {
            _ngTradeRepository = new NgTradeRepository();
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
