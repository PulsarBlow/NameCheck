using SuperMassive;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NameCheck.WebApi
{
    public class NameCheckBatchViewModel
    {
        protected string _separator = Constants.DefaultBatchSeparator;

        [Required]
        public string Batch { get; set; }

        public string Separator { get { return _separator; } }
        public List<NameCheckBatchModel> History { get; set; }

        public NameCheckBatchViewModel() { }
        public NameCheckBatchViewModel(string separator)
        {
            Guard.ArgumentNotNullOrWhiteSpace(separator, "separator");
            this._separator = separator;
        }
    }
}
