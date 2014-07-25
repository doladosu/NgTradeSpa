using System.Web.Mvc;

namespace NgTrade.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Account()
        {
            return View();
        }
    }
}