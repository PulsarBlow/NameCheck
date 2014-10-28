using SerialLabs;
using SerialLabs.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace NameCheck.WebApi
{
    public class NameCheckController : BaseMvcController
    {
        protected IDataService<NameCheckModel, DescendingSortedGuid> DataService;

        public NameCheckController(IDataService<NameCheckModel, DescendingSortedGuid> dataService)
        {
            Guard.ArgumentNotNull(dataService, "dataService");
            DataService = dataService;
        }

        // GET: NameCheck
        public ActionResult Index()
        {
            return View(new NameCheckViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(NameCheckViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Check and add result to history
                NameCheckModel model = await NameCheckManager.CheckNameAsync(viewModel.Name, EndpointType.Website);
                // Add to storage
                await DataService.SaveAsync(model);
                // Add to session history
                var history = ReadOrCreateHistory();
                history.Add(model);
                viewModel.Name = String.Empty;
                viewModel.History = history;
            }
            return View(viewModel);
        }

        private IList<NameCheckModel> ReadOrCreateHistory()
        {
            var history = Session["checkHistory"] as IList<NameCheckModel>;
            if (history == null)
            {
                history = new List<NameCheckModel>();
                Session["checkHistory"] = history;
            }

            return history;
        }
    }
}