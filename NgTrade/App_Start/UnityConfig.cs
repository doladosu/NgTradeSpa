using Microsoft.Practices.Unity;
using System.Web.Http;
using NgTrade.Models.Repo.Impl;
using NgTrade.Models.Repo.Interface;
using Unity.WebApi;

namespace NgTrade
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            container.RegisterType<IAccountRepository, AccountRepository>();
            container.RegisterType<INgTradeRepository, NgTradeRepository>();
            container.RegisterType<ISmtpRepository, SmtpRepository>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}