using SuperMassive;
using System.Collections.Generic;

namespace NameCheck.WebApi
{
    public class NameCheckModel : BaseModel<DescendingSortedGuid>
    {
        public string Name { get; set; }

        public string Query { get; set; }

        public string Key { get; set; }
        public long QueryDurationMs { get; set; }

        public IDictionary<string, bool> SocialNetworks { get; set; }

        public IDictionary<string, bool> Domains { get; set; }
    }
}