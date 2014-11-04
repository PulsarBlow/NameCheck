using SuperMassive;
using System.Reflection;
using System.Web.Mvc;

namespace NameCheck.WebApi
{
    public abstract class BaseMvcController : Controller
    {
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            ViewBag.Version = AssemblyHelper.GetInformationalVersion(Assembly.GetAssembly(this.GetType()));
        }
    }
}