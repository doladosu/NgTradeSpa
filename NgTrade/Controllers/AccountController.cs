using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using NgTrade.Helpers.Paging;
using NgTrade.Models.Data;
using NgTrade.Models.Info;
using NgTrade.Models.Repo.Interface;
using NgTrade.Models.ViewModel;
using WebMatrix.WebData;
using NgTrade.Models;

namespace NgTrade.Controllers
{
    //[Authorize]
    public class AccountController : BaseController
    {
        private const int PageSize = 5;

        public AccountController(IAccountRepository accountRepository, IQuoteRepository quoteRepository, IHoldingRepository holdingRepository, IOrderRepository orderRepository, ISmtpRepository smtpRepository) : base(accountRepository, quoteRepository, smtpRepository, null, holdingRepository, orderRepository)
        {
        }

        public ActionResult Index()
        {
            //if (LoggedInSubscriber == null && User.Identity.IsAuthenticated)
            //{
            //    var membershipUser = Membership.GetUser();
            //    if (membershipUser != null)
            //    {
            //        if (membershipUser.ProviderUserKey != null)
            //        {
            //            var subscriberId = membershipUser.ProviderUserKey.ToString();
            //            LoggedInSubscriber = AccountRepository.GetAccountProfile(Int32.Parse(subscriberId));
            //        }
            //    }
            //}
            //if (LoggedInSubscriber == null)
            //{
            //    return RedirectToAction("LogOff");
            //}
            //var accountProfile = AccountRepository.GetAccountProfile(LoggedInSubscriber.UserId);
            //var holdings = HoldingRepository.GetHoldings(accountProfile.UserId);
            //var holdingSum = 0.00;
            //if (holdings.Any())
            //{
            //    foreach (var holding in holdings)
            //    {
            //        var quote = QuoteRepository.GetQuote(holding.Symbol);
            //        holdingSum = holdingSum + Convert.ToDouble(holding.Quantity*quote.Close);
            //    }
            //}
            //var accountViewModel = new AccountViewModel
            //{
            //    AccountNumber = LoggedInSubscriber.AccountNumber.GetValueOrDefault(),
            //    Address1 = accountProfile.Address1,
            //    Address2 = accountProfile.Address2,
            //    City = accountProfile.City,
            //    State = accountProfile.State,
            //    Country = accountProfile.Country,
            //    FirstName = accountProfile.FirstName,
            //    LastName = accountProfile.LastName,
            //    Email = accountProfile.Email,
            //    Status = accountProfile.Verified.GetValueOrDefault(),
            //    Phone = accountProfile.Phone1,
            //    Balance = accountProfile.Balance.GetValueOrDefault() + Convert.ToDecimal(holdingSum),
            //    AccountName = accountProfile.UserName
            //};
            return View();
        }

        public ActionResult Portfolio(int? page, int? ot)
        {           
            var pageNumber = (page ?? 1);
            var holdings = HoldingRepository.GetHoldings(LoggedInSubscriber.UserId);
            var orders = OrderRepository.GetOrders(LoggedInSubscriber.UserId);

            List<PortfolioModel> portfolioModels;
            if (ot.GetValueOrDefault() == 1)
            {
                portfolioModels = (from order in orders
                                   where order.Status.ToLower() == "complete" && order.Type.ToLower() == "sell"
                                   let quoteInfo = QuoteRepository.GetQuote(order.Symbol)
                                   select new PortfolioModel
                                   {
                                       Purchaseprice = order.Price,
                                       Purchasedate = order.OpenDate,
                                       Quantity = order.Quantity,
                                       QuoteSymbol = order.Symbol,
                                       CurrentPrice = order.Price,
                                       GainLoss = 0,
                                       Holdingid = order.HoldingId,
                                       MarketValue = 0
                                   }).ToList();
            }
            else
            {
                portfolioModels = (from holding in holdings
                                   where holding.Quantity > 0
                                   let quoteInfo = QuoteRepository.GetQuote(holding.Symbol)
                                   select new PortfolioModel
                                       {
                                           Purchaseprice = holding.Price,
                                           Purchasedate = holding.DatePurchased,
                                           Quantity = holding.Quantity,
                                           QuoteSymbol = holding.Symbol,
                                           CurrentPrice = quoteInfo.Close,
                                           GainLoss = holding.Quantity*(quoteInfo.Close - holding.Price),
                                           Holdingid = holding.Id,
                                           MarketValue = holding.Quantity*quoteInfo.Close
                                       }).ToList();
            }
            var pagingInfo = new PagingInfo
            {
                CurrentPage = pageNumber,
                ItemsPerPage = PageSize,
                TotalItems = holdings.Count
            };

            var portfolioViewModel = new PortfolioViewModel { PagingInfo = pagingInfo, PortfolioVm = portfolioModels.Skip(PageSize * (pageNumber - 1)).Take(PageSize).ToList(), Ot = ot.GetValueOrDefault()};
            return View(portfolioViewModel);
        }

        public ActionResult Trade(string symbol)
        {
            var actions = new List<string> { "Buy", "Sell" };
            var price = 0.00;
            if (!string.IsNullOrWhiteSpace(symbol))
            {
                try
                {
                    Quote quote = QuoteRepository.GetQuote(symbol);
                    if (quote != null)
                    {
                        price = Convert.ToDouble(quote.Close);
                    }
                }
                catch (Exception)
                {
                    
                }
            }
            var actionsList = actions.Select(x => new SelectListItem
            {
                Text = x,
                Value = x
            }).ToList();
            var tradeViewModel = new TradeViewModel {ActionsList = actionsList, Price = price, Symbol = symbol };
            return View(tradeViewModel);
        }

        public ActionResult Refer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Refer(ReferViewModel referViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(referViewModel);
            }

            var referral = new ReferViewModel
            {
                Email = referViewModel.Email,
                Name = referViewModel.Name,
                ReferralName = LoggedInSubscriber.FirstName
            };
            var exists = AccountRepository.GetMailingList(referViewModel.Email);
            if (exists == null)
            {
                var mailingList = new MailingList { DateAdded = DateTime.Now, Email = referViewModel.Email, Subscribed = true };
                AccountRepository.AddToMailingList(mailingList);
            }
            SmtpRepository.SendReferralEmail(referral);

            return RedirectToAction("Refer", new { message = "Your message is sent", messageClass = "success" });
        }

        [OutputCache(CacheProfile = "StaticPageCache")]
        public ActionResult GetSymbolInfo(string symbol)
        {
            var stockData = QuoteRepository.GetQuote(symbol);
            return Json(stockData, JsonRequestBehavior.AllowGet);
        }

        public RedirectToRouteResult CreateStockOrder(TradeViewModel tradeViewModel)
        {
            var stock = QuoteRepository.GetQuote(tradeViewModel.Symbol);
            if (stock == null)
            {
                TempData["ErrorMessage"] = "There was an error placing your order, please try again!";
                return RedirectToAction("Trade");
            }
            var orderModel =new OrderModel
                {
                    Action = tradeViewModel.Action,
                    Price = Convert.ToDouble(stock.Close),
                    Shares = tradeViewModel.Shares,
                    Symbol = tradeViewModel.Symbol,
                    UserProfile = LoggedInSubscriber
                };
            var order = new Order
                {
                    AccountId = LoggedInSubscriber.UserId,
                    CompletionDate = DateTime.Now,
                    OpenDate = DateTime.Now,
                    Price = Convert.ToDecimal(tradeViewModel.Price),
                    Quantity = tradeViewModel.Shares,
                    Symbol = tradeViewModel.Symbol,
                    Type = tradeViewModel.Action,
                    Status = "Open"
                };
            var orderCreated = OrderRepository.CreateOrder(order);
            Holding holding = null;
            if (orderCreated != null)
            {
                holding = HoldingRepository.CreateHolding(orderModel);
                if (holding != null)
                {
                    orderCreated.HoldingId = holding.Id;
                    orderCreated.Status = "Complete";
                    OrderRepository.UpdateOrder(orderCreated);
                }
            }
            if (holding != null)
            {
                TempData["SuccessMessage"] = "Your order has been successfully placed. Please check back in 24 hours to view completion";
                return RedirectToAction("Trade");
            }
            TempData["ErrorMessage"] = "There was an error placing your order, please try again!";
            return RedirectToAction("Trade");
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, model.RememberMe))
            {
                var loginCookie = new HttpCookie("lg", "1");
                Response.SetCookie(loginCookie);
                return RedirectToLocal(returnUrl);
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }

        //
        // POST: /Account/LogOff

        public ActionResult LogOff()
        {
            if (Request.Cookies["lg"] != null)
            {
                var loginCookie = new HttpCookie("lg") { Expires = DateTime.Now.AddDays(-1) };
                Response.Cookies.Add(loginCookie);
            }
            WebSecurity.Logout();

            return RedirectToAction("Index", "Account");
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    model.SignupDate = DateTime.Now;
                    model.Verified = false;
                    model.BankVerified = false;
                    model.BirthDate = DateTime.Now;
                    model.Balance = 1000000;
                    WebSecurity.CreateUserAndAccount(model.UserName, model.Password, new { model.FirstName, model.LastName, model.Email, model.Address1, model.Address2, model.City, model.State, model.Country, model.Phone1, model.BirthDate, model.Occupation, model.SignupDate, model.BankVerified, model.Verified, model.Balance  });
                    WebSecurity.Login(model.UserName, model.Password);
                    var loginCookie = new HttpCookie("lg", "1");
                    Response.SetCookie(loginCookie);
                    var exists = AccountRepository.GetMailingList(model.Email);
                    if (exists == null)
                    {
                        var mailingList = new MailingList { DateAdded = DateTime.Now, Email = model.Email, Subscribed = true };
                        AccountRepository.AddToMailingList(mailingList);
                    }
                    return RedirectToAction("Index", "Account");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/Disassociate

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disassociate(string provider, string providerUserId)
        {
            string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
            ManageMessageId? message = null;

            // Only disassociate the account if the currently logged in user is the owner
            if (ownerAccount == User.Identity.Name)
            {
                // Use a transaction to prevent the user from deleting their last login credential
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                    if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
                    {
                        OAuthWebSecurity.DeleteAccount(provider, providerUserId);
                        scope.Complete();
                        message = ManageMessageId.RemoveLoginSuccess;
                    }
                }
            }

            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage

        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : "";
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }
            else
            {
                // User does not have a local password so remove any validation errors caused by a missing
                // OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", String.Format("Unable to create local account. An account with the name \"{0}\" may already exist.", User.Identity.Name));
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return RedirectToAction("ExternalLoginFailure");
            }

            if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
            {
                var loginCookie = new HttpCookie("lg", "1");
                Response.SetCookie(loginCookie);
                return RedirectToLocal(returnUrl);
            }

            if (User.Identity.IsAuthenticated)
            {
                // If the current user is logged in add the new account
                OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
                var loginCookie = new HttpCookie("lg", "1");
                Response.SetCookie(loginCookie);
                return RedirectToLocal(returnUrl);
            }
            // User is new, ask for their desired membership name
            string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
            ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
            ViewBag.ReturnUrl = returnUrl;
            return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
        }

        //
        // POST: /Account/ExternalLoginConfirmation

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            string provider;
            string providerUserId;

            if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Insert a new user into the database
                using (UsersContext db = new UsersContext())
                {
                    UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
                    // Check if user already exists
                    if (user == null)
                    {
                        // Insert name into the profile table
                        db.UserProfiles.Add(new UserProfile { UserName = model.UserName });
                        db.SaveChanges();

                        OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
                        OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);
                        var loginCookie = new HttpCookie("lg", "1");
                        Response.SetCookie(loginCookie);
                        return RedirectToLocal(returnUrl);
                    }
                    ModelState.AddModelError("UserName", "User name already exists. Please enter a different user name.");
                }
            }

            ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // GET: /Account/ExternalLoginFailure

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        }

        [ChildActionOnly]
        public ActionResult RemoveExternalLogins()
        {
            ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
            List<ExternalLogin> externalLogins = (from account in accounts
                                                  let clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider)
                                                  select new ExternalLogin
                                                             {
                                                                 Provider = account.Provider, ProviderDisplayName = clientData.DisplayName, ProviderUserId = account.ProviderUserId,
                                                             }).ToList();

            ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            return PartialView("_RemoveExternalLoginsPartial", externalLogins);
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(string rt)
        {
            var resetPasswordViewModel = new ResetPasswordViewModel{Token = rt};
            return View(resetPasswordViewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                WebSecurity.ResetPassword(model.Token, model.Password);
                return RedirectToAction("Login");
            }
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordViewModel forgotPasswordViewModel)
        {
            //check user existance
            if (string.IsNullOrWhiteSpace(forgotPasswordViewModel.UserName))
            {
                TempData["ErrorMessage"] = "Enter your user name";
            }
            else
            {
                var user = Membership.GetUser(forgotPasswordViewModel.UserName);
                if (user == null)
                {
                    TempData["ErrorMessage"] = "User does not exist.";
                }
                else
                {
                    //generate password token
                    var token = WebSecurity.GeneratePasswordResetToken(forgotPasswordViewModel.UserName);
                    //create url with above token
                    var resetLink = "<a href='" +
                                    Url.Action("ResetPassword", "Account", new {rt = token}, "http") +
                                    "'>Reset Password</a>";
                    //get user emailid
                    var email = "";
                    var users = AccountRepository.GetAllAccountProfiles();
                    foreach (var userProfile in users)
                    {
                        if (userProfile.UserName.ToLower() == forgotPasswordViewModel.UserName.ToLower())
                        {
                            email = userProfile.Email;
                            break;
                        }
                    }
                    //send mail
                    string body = "<b>Please click the link to reset your NgTradeOnline Password </b><br/>" + resetLink; //edit it
                    try
                    {
                        SmtpRepository.SendForgotPasswordEmail(email, body);
                        TempData["SuccessMessage"] = "Check your email for your reset password link.";
                    }
                    catch (Exception ex)
                    {
                        TempData["ErrorMessage"] = "Error occured while sending email." + ex.Message;
                    }
                }
            }
            return View();
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Account");
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
