using System.Web.Mvc;

namespace NameCheck.WebApi.Controllers
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