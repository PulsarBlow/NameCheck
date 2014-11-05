using SuperMassive;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace NameCheck.WebApi
{
    public class NameCheckController : BaseMvcController
    {
        protected IDataService<NameCheckModel, DescendingSortedGuid> NameCheckDataService { get; private set; }
        protected IDataService<NameCheckBatchModel, DescendingSortedGuid> NameCheckBatchDataService { get; private set; }
        protected NameCheckProvider Provider { get; private set; }

        public NameCheckController(IDataService<NameCheckModel, DescendingSortedGuid> nameCheckDataService, IDataService<NameCheckBatchModel, DescendingSortedGuid> nameCheckBatchDataService, NameCheckProvider provider)
        {
            Guard.ArgumentNotNull(nameCheckDataService, "nameCheckDataService");
            NameCheckDataService = nameCheckDataService;
            Guard.ArgumentNotNull(nameCheckBatchDataService, "nameCheckBatchDataService");
            NameCheckBatchDataService = nameCheckBatchDataService;
            Guard.ArgumentNotNull(provider, "provider");
            Provider = provider;
        }

        // GET: NameCheck
        public ActionResult Index()
        {
            var viewModel = new NameCheckViewModel();
            viewModel.History = ReadOrCreateSessionItem<List<NameCheckModel>>(Constants.SessionKeys.NameCheckHistory);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(NameCheckViewModel viewModel)
        {
            if (viewModel == null) { return RedirectToAction("index"); }

            viewModel.History = ReadOrCreateSessionItem<List<NameCheckModel>>(Constants.SessionKeys.NameCheckHistory);

            if (ModelState.IsValid)
            {
                NameCheckModel model = await Provider.NameCheckAsync(viewModel.Name, EndpointType.Website, Request.UserHostAddress);
                await NameCheckDataService.SaveAsync(model);
                viewModel.History.Add(model);
                SaveOrCreateSessionItem(Constants.SessionKeys.NameCheckHistory, viewModel.History);
                viewModel.Name = null;
                ModelState.Clear();
            }

            return View(viewModel);
        }

        [Authorize]
        public ActionResult Batch()
        {
            var viewModel = new NameCheckBatchViewModel(Constants.DefaultBatchSeparator);
            viewModel.History = ReadOrCreateSessionItem<List<NameCheckBatchModel>>(Constants.SessionKeys.NameCheckBatchHistory);
            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Batch(NameCheckBatchViewModel viewModel)
        {
            if (viewModel == null) { return RedirectToAction("batch"); }

            viewModel.History = ReadOrCreateSessionItem<List<NameCheckBatchModel>>(Constants.SessionKeys.NameCheckBatchHistory);

            if (ModelState.IsValid)
            {
                NameCheckBatchModel model = await Provider.NameCheckBatchAsync(
                    viewModel.Batch,
                    viewModel.Separator,
                    EndpointType.Website,
                    Request.UserHostAddress);

                await NameCheckBatchDataService.SaveAsync(model);
                viewModel.History.Add(model);
                SaveOrCreateSessionItem(Constants.SessionKeys.NameCheckBatchHistory, viewModel.History);
                viewModel.Batch = null;
                ModelState.Clear();

            }
            return View(viewModel);
        }

        private T ReadOrCreateSessionItem<T>(string keyName)
            where T : class, new()
        {
            var history = Session[keyName] as T;
            if (history == null)
            {
                history = new T();
                Session[keyName] = history;
            }

            return history;
        }
        private void SaveOrCreateSessionItem<T>(string keyName, T value)
            where T : class, new()
        {
            Session[keyName] = value;
        }

    }
}