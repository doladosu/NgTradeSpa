using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using NgTrade.Models.Info;
using NgTrade.Models.Repo.Interface;
using NgTrade.Models.ViewModel;
using WebMatrix.WebData;

namespace NgTrade.Controllers
{
    public class UserController : ApiController
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ISmtpRepository _smtpRepository;

        public UserController(IAccountRepository accountRepository, ISmtpRepository smtpRepository)
        {
            _accountRepository = accountRepository;
            _smtpRepository = smtpRepository;
        }

        public HttpResponseMessage GetProfile()
        {
            string cookieToken, formToken;
            var httpCookie = HttpContext.Current.Response.Cookies[AntiForgeryConfig.CookieName];
            if (httpCookie != null && !string.IsNullOrWhiteSpace(httpCookie.Value))
            {
                AntiForgery.GetTokens(httpCookie.Value, out cookieToken, out formToken);
            }
            else
            {
                AntiForgery.GetTokens(null, out cookieToken, out formToken);
            }
            httpCookie = HttpContext.Current.Response.Cookies[AntiForgeryConfig.CookieName];
            if (httpCookie != null)
                httpCookie.Value = cookieToken;

            var accountProfile = _accountRepository.GetAccountProfile(WebSecurity.CurrentUserId);
            var userDashboardInfo = new UserDashboardInfo {UserProfile = accountProfile, DisplayName = accountProfile.FirstName + "." + accountProfile.LastName, Balance = accountProfile.Balance.GetValueOrDefault() };
            var response = Request.CreateResponse(HttpStatusCode.OK, accountProfile);
            return response;
        }

        public HttpResponseMessage Contact(ContactViewModel contactVm)
        {
            var response = Request.CreateResponse(HttpStatusCode.BadRequest, contactVm);
            if (!ModelState.IsValid)
            {
                return response;
            }

            var contact = new ContactViewModel
            {
                Email = contactVm.Email,
                Name = contactVm.Name,
                Message = contactVm.Message
            };

            _smtpRepository.SendContactEmail(contact);

            response = Request.CreateResponse(HttpStatusCode.OK, contactVm);
            return response;
        }

    }
}