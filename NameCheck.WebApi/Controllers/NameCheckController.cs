using SerialLabs;
using SerialLabs.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace NameCheck.WebApi.Controllers
{
    public class NameCheckController : Controller
    {
        protected IDataService<CheckResultModel, DescendingSortedGuid> DataService;

        public NameCheckController(IDataService<CheckResultModel, DescendingSortedGuid> dataService)
        {
            Guard.ArgumentNotNull(dataService, "dataService");
            DataService = dataService;
        }

        // GET: NameCheck
        public ActionResult Index()
        {
            return View(new NameCheckModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(NameCheckModel model)
        {
            if (ModelState.IsValid)
            {
                // Check and add result to history
                var result = await NameCheckManager.CheckNameAsync(model.Name);
                // Add to storage
                await DataService.SaveAsync(result);
                // Add to session history
                var history = ReadOrCreateHistory();
                history.Add(result);
                model.Name = null;
                model.History = history;
            }
            return View(model);
        }

        private IList<CheckResultModel> ReadOrCreateHistory()
        {
            var history = Session["checkHistory"] as IList<CheckResultModel>;
            if (history == null)
            {
                history = new List<CheckResultModel>();
                Session["checkHistory"] = history;
            }

            return history;
        }
    }
}