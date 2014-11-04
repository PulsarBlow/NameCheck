using System.Web.Mvc;

namespace NameCheck.WebApi
{
    public class HomeController : BaseMvcController
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
    }
}