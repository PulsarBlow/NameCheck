using Microsoft.WindowsAzure;
using SuperMassive;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace NameCheck.WebApi
{
    public interface IAuthorizedUserStore
    {
        bool IsAuthorized(string email);
    }

    public class AuthorizedUserStore : IAuthorizedUserStore
    {
        public Dictionary<string, List<ApplicationUser>> Store { get; protected set; }

        public AuthorizedUserStore()
        {
            Store = CreateStoreFromConfig();
        }

        public bool IsAuthorized(string email)
        {
            Guard.ArgumentNotNullOrWhiteSpace(email, "email");
            var user = CreateUser(email);
            if (user == null) { return false; }
            if (!Store.ContainsKey(user.Domain))
            {
                return false;
            }
            // Check if any wildcard user
            if (Store[user.Domain].Any(x => x.UserName == "*"))
            {
                return true; // all user of this domain are implicitly authorized
            }

            return Store[user.Domain].Any(x => x.UserName.Equals(user.UserName, StringComparison.CurrentCultureIgnoreCase));
        }

        private static Dictionary<string, List<ApplicationUser>> CreateStoreFromConfig()
        {
            Dictionary<string, List<ApplicationUser>> store = new Dictionary<string, List<ApplicationUser>>();
            string config = CloudConfigurationManager.GetSetting(Constants.ConfigurationKeys.AuthorizedEmails);
            if (String.IsNullOrWhiteSpace(config)) { return store; }
            string[] authorizedEmails = config.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            if (authorizedEmails == null) { return store; }
            foreach (var email in authorizedEmails)
            {
                if (String.IsNullOrWhiteSpace(email)) { continue; }
                var user = CreateUser(email);
                if (user == null) { continue; }
                if (store.ContainsKey(user.Domain))
                {
                    store[user.Domain].AddIfNotNull(user);
                }
                else
                {
                    store.Add(user.Domain, new List<ApplicationUser> { user });
                }
            }
            return store;
        }

        public static ApplicationUser CreateUser(string email)
        {
            if (String.IsNullOrWhiteSpace(email))
            {
                return null;
            }
            string formattedEmail = FormatEmail(email);

            string[] parts = formattedEmail.Split(new string[] { "@" }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2 || String.IsNullOrWhiteSpace(parts[0]) || String.IsNullOrWhiteSpace(parts[1]))
            {
                return null;
            }

            return new ApplicationUser
            {
                Domain = parts[1],
                UserName = parts[0],
                Email = formattedEmail
            };
        }

        private static string FormatEmail(string email)
        {
            if (String.IsNullOrWhiteSpace(email))
            {
                return email;
            }
            return email.ToLower(CultureInfo.CurrentUICulture).Trim();
        }
    }
}