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

        public HttpResponseMessage Logout()
        {
            WebSecurity.Logout();
            var userProfile = new UserProfile();

            var response = Request.CreateResponse(HttpStatusCode.OK, userProfile);
            return response;
        }
    }
}
