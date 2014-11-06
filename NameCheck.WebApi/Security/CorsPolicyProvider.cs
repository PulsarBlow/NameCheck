using Microsoft.WindowsAzure;
using SuperMassive;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http.Cors;

namespace NameCheck.WebApi
{
    public class CorsPolicyProvider : ICorsPolicyProvider
    {
        private CorsPolicy _policy;

        public CorsPolicyProvider(string origins = Constants.Cors.AllowAll, string methods = Constants.Cors.AllowAll, string headers = Constants.Cors.AllowAll)
        {
            _policy = new CorsPolicy();
            AllowOrigins(CloudConfigurationManager.GetSetting(Constants.ConfigurationKeys.CorsAllowOrigins));
            AllowMethods(CloudConfigurationManager.GetSetting(Constants.ConfigurationKeys.CorsAllowMethods));
            AllowHeaders(CloudConfigurationManager.GetSetting(Constants.ConfigurationKeys.CorsAllowHeaders));
        }

        public Task<CorsPolicy> GetCorsPolicyAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_policy);
        }

        private void AllowOrigins(string origins)
        {
            if (String.IsNullOrWhiteSpace(origins) || origins == Constants.Cors.AllowAll)
            {
                _policy.AllowAnyOrigin = true;
            }
            else
            {
                _policy.AllowAnyOrigin = false;
                ParseAndFill(_policy.Origins, origins);
            }
        }
        private void AllowMethods(string methods)
        {
            if (String.IsNullOrWhiteSpace(methods) || methods == Constants.Cors.AllowAll)
            {
                _policy.AllowAnyMethod = true;
            }
            else
            {
                _policy.AllowAnyMethod = false;
                ParseAndFill(_policy.Methods, methods);
            }
        }
        private void AllowHeaders(string headers)
        {
            if (String.IsNullOrWhiteSpace(headers) || headers == Constants.Cors.AllowAll)
            {
                _policy.AllowAnyHeader = true;
            }
            else
            {
                _policy.AllowAnyHeader = false;
                ParseAndFill(_policy.Headers, headers);
            }
        }

        private static void ParseAndFill(IList<string> list, string commaSeparatedList)
        {
            if (list == null || String.IsNullOrWhiteSpace(commaSeparatedList)) { return; }
            ConfigHelper.ParseCommaSeparatedList(commaSeparatedList).Each(x =>
            {
                list.AddIfNotNull(x);
            });
        }
    }
}