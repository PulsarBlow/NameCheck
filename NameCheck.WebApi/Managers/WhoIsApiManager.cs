using SerialLabs;
using System;
using System.Globalization;
using System.Threading.Tasks;
using Whois;
using Whois.Domain;

namespace NameCheck.WebApi
{
    public static class WhoIsApiManager
    {
        public static async Task<WhoIsCheckResult> CheckName(string name)
        {
            Guard.ArgumentNotNullOrWhiteSpace(name, "name");

            WhoIsCheckResult result = new WhoIsCheckResult();
            result.DomainCom = await CheckAvailability(FormatDomainName(name, "com"));
            result.DomainNet = await CheckAvailability(FormatDomainName(name, "net"));
            result.DomainOrg = await CheckAvailability(FormatDomainName(name, "org"));
            return result;
        }


        private static string FormatName(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                return name;
            }

            return NameHelper.RemoveExtension(name);
        }

        private static string FormatExtension(string extension)
        {
            Guard.ArgumentNotNullOrWhiteSpace(extension, "extension");
            return extension.Replace(".", String.Empty);
        }

        private static string FormatDomainName(string name, string extension)
        {
            return String.Format(CultureInfo.InvariantCulture, "{0}.{1}", FormatName(name), FormatExtension(extension));
        }

        private static async Task<bool> CheckAvailability(string domain)
        {
            return await Task.Factory.StartNew(() =>
            {
                var whois = new WhoisLookup().Lookup(domain);
                return IsAvailable(whois);
            });
        }

        private static bool IsAvailable(WhoisRecord record)
        {
            if (record == null || record.Text == null || record.Text.Count == 0)
            {
                return false;
            }

            var expirationIndex = -1;
            for (int i = 0; i < record.Text.Count; i++)
            {
                var item = (string)record.Text[i];
                if (String.IsNullOrWhiteSpace(item))
                {
                    continue;
                }
                if (item.StartsWith("Registrar Registration Expiration Date"))
                {
                    expirationIndex = i;
                    break;
                }
            }

            if (expirationIndex == -1)
            {
                return true;
            }

            string expirationString = record.Text[expirationIndex] as String;
            var dateString = expirationString.Substring(expirationString.IndexOf(':') + 1).Trim();
            DateTime expirationDate;
            if (DateTime.TryParse(dateString, out expirationDate))
            {
                return expirationDate < DateTime.Now;
            }

            return false;
        }
    }
}