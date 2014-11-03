using CookComputing.XmlRpc;
using Microsoft.WindowsAzure;
using SuperMassive;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace NameCheck.WebApi
{
    public static class GandiApiManager
    {
        public static bool CheckDomain(string domainWithExtension)
        {
            IDomainProxy proxy = XmlRpcProxyGen.Create<IDomainProxy>();
            XmlRpcStruct result = proxy.Available(CloudConfigurationManager.GetSetting(Constants.ConfigurationKeys.GandiApiKey), new string[] { domainWithExtension });
            if (result == null || result.Count == 0 || result[domainWithExtension] == null)
            {
                return false;
            }
            string status = result[domainWithExtension] as String;
            if (String.Compare(status, "pending", true) == 0)
            {
                return GandiApiManager.CheckDomain(domainWithExtension);
            }
            return IsDomainStatusAvailable(status);
        }

        public static Dictionary<string, bool> CheckDomains(string name, string[] extensions)
        {
            Guard.ArgumentNotNullOrWhiteSpace(name, "name");
            Guard.ArgumentNotNull(extensions, "extensions");

            Dictionary<string, bool> result = new Dictionary<string, bool>();
            extensions.Each(x => { result.AddOrUpdate(x, false); });

            IDomainProxy proxy = XmlRpcProxyGen.Create<IDomainProxy>();
            XmlRpcStruct rpcResult = proxy.Available(CloudConfigurationManager.GetSetting(Constants.ConfigurationKeys.GandiApiKey), GetDomains(name, extensions));

            if (ShouldRevalidate(rpcResult))
            {
                return CheckDomains(name, extensions);
            }

            foreach (var key in rpcResult.Keys)
            {
                var skey = key.ToString();
                var indexOf = skey.IndexOf('.') + 1;
                var extension = skey.Substring(indexOf, skey.Length - indexOf);
                if (result.ContainsKey(extension))
                {
                    result[extension] = IsDomainStatusAvailable(rpcResult[key].ToString());
                }
            }

            return result;
        }

        public static bool ShouldRevalidate(XmlRpcStruct rpcResult)
        {
            if (rpcResult == null) { return false; }
            foreach (var key in rpcResult.Keys)
            {
                string domainStatus = rpcResult[key] as String;
                if (String.IsNullOrWhiteSpace(domainStatus) ||
                    String.Compare(domainStatus, "pending", true) == 0)
                {
                    return true;
                }
            }
            return false;
        }


        public static string[] GetDomains(string name, string[] extensions)
        {
            Guard.ArgumentNotNullOrWhiteSpace(name, "name");
            Guard.ArgumentNotNull(extensions, "extensions");

            List<string> domains = new List<string>();
            extensions.Each(x =>
            {
                domains.Add(FormatDomain(name, x));
            });
            return domains.ToArray();
        }

        public static bool IsDomainStatusAvailable(string status)
        {
            status = status.ToLowerInvariant();

            switch (status)
            {
                case "available":
                case "available_reserved":
                case "available_preorder":
                    return true;
                case "unavailable":
                case "unavailable_premium":
                case "unavailable_restricted":
                case "error_invalid":
                case "error_refused":
                case "error_timeout":
                case "error_unknown":
                case "reserved_corporate":
                case "pending":
                default:
                    return false;
            }
        }
        private static string FormatDomain(string name, string extension)
        {
            Guard.ArgumentNotNullOrWhiteSpace(name, "name");
            Guard.ArgumentNotNullOrWhiteSpace(extension, "extension");
            return String.Format(CultureInfo.InvariantCulture, "{0}.{1}",
                name.ToLowerInvariant(),
                extension.ToLowerInvariant().Replace(".", ""));
        }
    }


    [XmlRpcUrl("https://rpc.gandi.net/xmlrpc/")]
    public interface IDomainProxy : IXmlRpcProxy
    {
        [XmlRpcMethod("domain.available")]
        XmlRpcStruct Available(string apiKey, string[] domainNames);
    }

    public class DomainResult
    {
        public string Domain { get; set; }
        public bool IsAvailable { get; set; }
    }
}