using SuperMassive;
using System.Collections.Generic;

namespace NameCheck.WebApi
{
    public class NameCheckBatchModel : BaseModel<DescendingSortedGuid>
    {
        public string Value { get; set; }
        public long BatchDurationMs { get; set; }

        public IList<NameCheckModel> NameChecks { get; set; }
    }

}