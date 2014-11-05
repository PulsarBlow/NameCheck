using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace NameCheck.WebApi
{
    public class HttpUnauthorizedChallengeResult : IHttpActionResult
    {
        public AuthenticationHeaderValue Challenge { get; private set; }
        public IHttpActionResult InnerResult { get; private set; }

        public HttpUnauthorizedChallengeResult(AuthenticationHeaderValue challenge, IHttpActionResult innerResult)
        {
            Challenge = challenge;
            InnerResult = innerResult;
        }

        public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage response = await InnerResult.ExecuteAsync(cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                if (!response.Headers.WwwAuthenticate.Any(x => x.Scheme.Equals(Challenge.Scheme, StringComparison.InvariantCultureIgnoreCase)))
                {
                    response.Headers.WwwAuthenticate.Add(Challenge);
                }
            }

            return response;
        }
    }
}