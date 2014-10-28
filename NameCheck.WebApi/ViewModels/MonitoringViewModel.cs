using System;
using System.Collections.Generic;

namespace NameCheck.WebApi
{
    public class MonitoringViewModel
    {
        public IList<IRateLimit> RateLimits { get; set; }
        public IList<NameCheckModel> CheckResults { get; set; }
        public MonitoringConfiguration Configuration { get; set; }
        public Exception Error { get; set; }

    }

    public class MonitoringConfiguration : Dictionary<string, string>
    {
        public MonitoringConfiguration()
            : base()
        { }
        public MonitoringConfiguration(int capacity)
            : base(capacity)
        { }
        public MonitoringConfiguration(IDictionary<string, string> dictionary)
            : base(dictionary)
        { }

        public MonitoringConfiguration(IEqualityComparer<string> comparer)
            : base(comparer)
        { }

        public MonitoringConfiguration(IDictionary<string, string> dictionary, IEqualityComparer<string> comparer)
            : base(dictionary, comparer)
        { }

        public MonitoringConfiguration(int capacity, IEqualityComparer<string> comparer)
            : base(capacity, comparer)
        { }
    }
}