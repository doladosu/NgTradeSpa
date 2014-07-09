using System.Linq;
using Breeze.ContextProvider;
using Breeze.ContextProvider.EF6;
using Newtonsoft.Json.Linq;
using NgTrade.Models.Data;
using NgTrade.Models.Repo.Interface;

namespace NgTrade.Models.Repo.Impl
{
    public class NgTradeRepository : INgTradeRepository
    {
        private readonly EFContextProvider<UsersContext>
           _contextProvider = new EFContextProvider<UsersContext>();

        private UsersContext Context
        {
            get { return _contextProvider.Context; }
        }

        public string Metadata
        {
            get { return _contextProvider.Metadata(); }
        }

        public SaveResult SaveChanges(JObject saveBundle)
        {
            return _contextProvider.SaveChanges(saveBundle);
        }

        public IQueryable<AccountProfile> AccountProfiles
        {
            get { return Context.AccountProfiles; }
        }

        public IQueryable<Dailypricelist> Dailypricelists
        {
            get { return Context.Dailypricelists; }
        }

        public IQueryable<Companyprofile> Companyprofiles
        {
            get { return Context.Companyprofiles; }
        }

        public IQueryable<Holding> Holdings
        {
            get { return Context.Holdings; }
        }

        public IQueryable<MailingList> MailingLists
        {
            get { return Context.MailingLists; }
        }

        public IQueryable<News> News
        {
            get { return Context.NewsList; }
        }

        public IQueryable<Order> Orders
        {
            get { return Context.Orders; }
        }

        public IQueryable<Quote> Quotes
        {
            get { return Context.Quotes; }
        }

        public IQueryable<UserProfile> UserProfiles
        {
            get { return Context.UserProfiles; }
        }
    }
}