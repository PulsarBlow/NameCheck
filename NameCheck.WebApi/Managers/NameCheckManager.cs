using SerialLabs;
using SerialLabs.Data;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace NameCheck.WebApi
{
    public static class NameCheckManager
    {
        public static async Task<CheckResultModel> CheckNameAsync(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name", "name is null or empty");
            }


            string formattedName = NameHelper.Format(name);
            var twitterResult = await TwitterApiManager.IsNameAvailable(formattedName);

            var result = new CheckResultModel();
            result.Id = DescendingSortedGuid.NewSortedGuid();
            result.Name = name;
            result.Twitter = twitterResult.IsAvailable;
            result.DomainExtensions = GandiApiManager.CheckDomains(formattedName, new string[] { "com", "net", "org" });
            return result;
        }


    }
}