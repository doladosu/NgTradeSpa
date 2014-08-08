using System;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Security;
using NgTrade.Models;
using NgTrade.Models.Data;
using NgTrade.Models.Repo.Interface;
using WebMatrix.WebData;

namespace NgTrade.Controllers
{
    public class AuthController : ApiController
    {
        private readonly IAccountRepository _accountRepository;

        public AuthController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public HttpResponseMessage Login(LoginModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.UserName) && !string.IsNullOrWhiteSpace(model.Password) && WebSecurity.Login(model.UserName, model.Password))
            {
                FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(model.UserName), null); 
                string cookieToken, formToken;
                AntiForgery.GetTokens(null, out cookieToken, out formToken);
                var httpCookie = HttpContext.Current.Response.Cookies[AntiForgeryConfig.CookieName];
                if (httpCookie != null)
                    httpCookie.Value = cookieToken;

                var accountProfile = _accountRepository.GetAccountProfile(WebSecurity.CurrentUserId);
                var response = Request.CreateResponse(HttpStatusCode.OK, accountProfile);
                return response;
            }
            var userProfile = new UserProfile();
            var badresponse = Request.CreateResponse(HttpStatusCode.OK, userProfile);
            return badresponse;
        }

        public HttpResponseMessage Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, false);
                    HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(model.UserName), null);
                    string cookieToken, formToken;
                    AntiForgery.GetTokens(null, out cookieToken, out formToken);
                    var httpCookie = HttpContext.Current.Response.Cookies[AntiForgeryConfig.CookieName];
                    if (httpCookie != null)
                        httpCookie.Value = cookieToken;


                    model.SignupDate = DateTime.Now;
                    model.Verified = false;
                    model.BankVerified = false;
                    model.BirthDate = DateTime.Now;
                    model.Balance = 1000000;
                    WebSecurity.CreateUserAndAccount(model.UserName, model.Password, new { model.FirstName, model.LastName, model.Email, model.Address1, model.Address2, model.City, model.State, model.Country, model.Phone1, model.BirthDate, model.Occupation, model.SignupDate, model.BankVerified, model.Verified, model.Balance });
                    WebSecurity.Login(model.UserName, model.Password);
                    var exists = _accountRepository.GetMailingList(model.Email);
                    if (exists == null)
                    {
                        var mailingList = new MailingList { DateAdded = DateTime.Now, Email = model.Email, Subscribed = true };
                        _accountRepository.AddToMailingList(mailingList);
                    }
                    var accountProfile = _accountRepository.GetAccountProfile(WebSecurity.CurrentUserId);

                    var response = Request.CreateResponse(HttpStatusCode.OK, accountProfile);
                    return response;
                }
                catch (MembershipCreateUserException)
                {
                }
            }
            var badresponse = Request.CreateResponse(HttpStatusCode.OK, false);
            return badresponse;
        }
        public HttpResponseMessage Logout()
        {
            WebSecurity.Logout();
            var userProfile = new UserProfile();

            var response = Request.CreateResponse(HttpStatusCode.OK, userProfile);
            return response;
        }
    }
}
