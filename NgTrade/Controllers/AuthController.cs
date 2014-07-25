using System.Net;
using System.Net.Http;
using System.Web.Http;
using NgTrade.Models;
using WebMatrix.WebData;

namespace NgTrade.Controllers
{
    public class AuthController : ApiController
    {
        public HttpResponseMessage Login(LoginModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.UserName) && !string.IsNullOrWhiteSpace(model.Password) && WebSecurity.Login(model.UserName, model.Password))
            {
                var response = Request.CreateResponse(HttpStatusCode.OK, true);
                return response;
            }
            var badresponse = Request.CreateResponse(HttpStatusCode.Unauthorized, false);
            return badresponse;
        }

        public HttpResponseMessage Logout()
        {
            WebSecurity.Logout();

            var response = Request.CreateResponse(HttpStatusCode.OK, false);
            return response;
        }
    }
}
