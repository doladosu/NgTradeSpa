﻿using System;
using System.Web.Mvc;
using System.Web.Security;
using NgTrade.Filters;
using NgTrade.Models.Data;
using NgTrade.Models.Repo.Interface;

namespace NgTrade.Controllers
{
    [InitializeSimpleMembership]
    public class BaseController : Controller
    {
        public readonly IAccountRepository AccountRepository;
        public readonly IQuoteRepository QuoteRepository;
        public readonly ISmtpRepository SmtpRepository;
        public readonly INewsRepository NewsRepository;

        public BaseController(IAccountRepository accountRepository, IQuoteRepository quoteRepository, ISmtpRepository smtpRepository, INewsRepository newsRepository)
        {
            AccountRepository = accountRepository;
            QuoteRepository = quoteRepository;
            SmtpRepository = smtpRepository;
            NewsRepository = newsRepository;
        }

        private AccountProfile _loggedInSubscriber;

        public AccountProfile LoggedInSubscriber
        {
            get
            {
                if (_loggedInSubscriber == null && User.Identity.IsAuthenticated)
                {
                    var membershipUser = Membership.GetUser();
                    if (membershipUser != null)
                    {
                        if (membershipUser.ProviderUserKey != null)
                        {
                            var subscriberId = membershipUser.ProviderUserKey.ToString();
                            _loggedInSubscriber = AccountRepository.GetAccountProfile(new Guid(subscriberId));
                        }
                    }
                }
                return _loggedInSubscriber;
            }
            set { _loggedInSubscriber = value; }
        }
    }
}