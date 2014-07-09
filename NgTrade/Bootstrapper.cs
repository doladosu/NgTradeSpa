using System.Web.Mvc;
using Microsoft.Practices.Unity;
using NgTrade.Models.Repo.Impl;
using NgTrade.Models.Repo.Interface;
using Unity.Mvc3;

namespace NgTrade
{
    public static class Bootstrapper
    {
        public static void Initialise()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            container.RegisterType<IQuoteRepository, QuoteRepository>();
            container.RegisterType<IAccountRepository, AccountRepository>();
            container.RegisterType<INewsRepository, NewsRepository>();
            container.RegisterType<IHoldingRepository, HoldingRepository>();
            container.RegisterType<IOrderRepository, OrderRepository>();
            container.RegisterType<ISmtpRepository, SmtpRepository>();
            container.RegisterType<INgTradeRepository, NgTradeRepository>();            

            return container;
        }
    }
}