using Microsoft.WindowsAzure;
using SuperMassive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace NameCheck.WebApi
{
    public class TokenBasedAuthenticationAttribute : Attribute, IAuthenticationFilter
    {
        public const string DefaultAuthenticationParamKey = "token";
        public const string AuthenticationScheme = "Token";

        protected string AuthenticationParamKey { get; private set; }
        public bool AllowMultiple { get { return false; } }

        public TokenBasedAuthenticationAttribute()
            : this(DefaultAuthenticationParamKey)
        { }

        public TokenBasedAuthenticationAttribute(string authenticationParamKey)
        {
            Guard.ArgumentNotNullOrWhiteSpace(authenticationParamKey, "authenticationParamKey");
            AuthenticationParamKey = authenticationParamKey;
        }

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            HttpRequestMessage request = context.Request;
            string token = null;
            if (!ReadAuthenticationToken(request, out token))
            {
                return;
            }

            if (String.IsNullOrWhiteSpace(token))
            {
                context.ErrorResult = new HttpAuthenticationFailureResult("Invalid token", request);
                return;
            }

            IPrincipal principal = AuthenticateAsync(token);

            if (principal == null)
            {
                context.ErrorResult = new HttpAuthenticationFailureResult("Invalid token", request);
            }
            else
            {
                context.Principal = principal;
            }
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            Challenge(context);
            return Task.FromResult(0);
        }

        public bool ReadAuthenticationToken(HttpRequestMessage request, out string token)
        {
            AuthenticationHeaderValue authenticationHeader = request.Headers.Authorization;
            KeyValuePair<string, string> authenticationParam = request.GetQueryNameValuePairs()
                .FirstOrDefault(x => x.Key.Equals(AuthenticationParamKey, StringComparison.InvariantCultureIgnoreCase));
            token = null;

            if (authenticationHeader != null && authenticationHeader.Scheme.Equals(AuthenticationScheme, StringComparison.InvariantCultureIgnoreCase))
            {
                token = authenticationHeader.Parameter;
                return true;

            }

            if (!String.IsNullOrWhiteSpace(authenticationParam.Key))
            {
                token = authenticationParam.Value;
                return true;
            }

            return false;
        }

        private void Challenge(HttpAuthenticationChallengeContext context)
        {
            context.ChallengeWith(AuthenticationScheme, null);
        }

        protected IPrincipal AuthenticateAsync(string token)
        {
            string configValue = CloudConfigurationManager.GetSetting(Constants.ConfigurationKeys.AuthorizedApiToken);
            if (String.IsNullOrWhiteSpace(configValue)) { return null; }
            if (configValue != token)
            {
                return null;
            }

            IPrincipal principal = new ClaimsPrincipal(new ClaimsIdentity(new GenericIdentity("ApiUser", AuthenticationScheme), new List<Claim>{
                new Claim(ClaimTypes.AuthenticationMethod, AuthenticationScheme),
                new Claim(ClaimTypes.Role, Constants.Claims.Roles.AuthenticatedUser),
                new Claim(ClaimTypes.NameIdentifier, "Anonymous")
            }));

            return principal;

        }
    }
}