using System.Linq;
using Breeze.ContextProvider;
using Newtonsoft.Json.Linq;
using NgTrade.Models.Data;

namespace NgTrade.Models.Repo.Interface
{
    public interface INgTradeRepository
    {
        string Metadata { get; }
        SaveResult SaveChanges(JObject saveBundle);
        IQueryable<AccountProfile> AccountProfiles { get; }
        IQueryable<Dailypricelist> Dailypricelists { get; }
        IQueryable<Companyprofile> Companyprofiles { get; }
        IQueryable<Holding> Holdings { get; }
        IQueryable<MailingList> MailingLists { get; }
        IQueryable<News> News { get; }
        IQueryable<Order> Orders { get; }
        IQueryable<Quote> Quotes { get; }
        IQueryable<UserProfile> UserProfiles { get; }
    }
}