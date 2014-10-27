using System;
using System.Collections.Generic;

namespace NameCheck.WebApi
{
    public class MonitoringModel
    {
        public IList<IRateLimit> RateLimits { get; set; }
        public IList<CheckResultModel> CheckResults { get; set; }
        public MonitoringConfiguration Configuration { get; set; }
        public Exception LastKnownException { get; set; }
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