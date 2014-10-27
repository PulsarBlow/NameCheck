using System;
using System.Globalization;
using System.Threading.Tasks;
using Whois;
using Whois.Domain;

namespace NameCheck.WebApi
{
    public static class WhoIsApiManager
    {
        public static async Task<ApiResponse> IsNameAvailable(string name)
        {
            return await Task.Factory.StartNew(() =>
            {
                var whois = new WhoisLookup().Lookup(FormatName(name));
                return new ApiResponse
                {
                    IsAvailable = CheckAvailability(whois)
                };
            });
        }


        private static string FormatName(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                return name;
            }

            string result = NameHelpers.RemoveExtension(name);
            // Others formatters ...

            // Add .com (only supported at this time)
            result = String.Format(CultureInfo.InvariantCulture, "{0}.com", result);
            return result;
        }

        private static bool CheckAvailability(WhoisRecord record)
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