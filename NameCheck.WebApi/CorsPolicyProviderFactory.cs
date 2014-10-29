using System.Net.Http;
using System.Web.Http.Cors;

namespace NameCheck.WebApi
{
    public class CorsPolicyProviderFactory : ICorsPolicyProviderFactory
    {
        ICorsPolicyProvider _provider;

        public CorsPolicyProviderFactory(string origins = Constants.Cors.AllowAll, string methods = Constants.Cors.AllowAll, string headers = Constants.Cors.AllowAll)
        {
            _provider = new CorsPolicyProvider(origins, methods, headers);
        }

        public ICorsPolicyProvider GetCorsPolicyProvider(HttpRequestMessage request)
        {
            return _provider;
        }
    }
}