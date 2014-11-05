using Microsoft.WindowsAzure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NameCheck.WebApi
{
    public class ApiKeyAuthorizationAttribute
    {
        public const string DefaultKeyName = "auth";

        private string _keyValue;

        /// <summary>
        /// Creates a new instance of the <see cref="SimpleKeyMvcAuthorizationAttribute"/>
        /// </summary>
        /// <param name="appSettingName"></param>
        public ApiKeyAuthorizationAttribute(string appSettingName)
        {
            if (String.IsNullOrWhiteSpace(appSettingName))
            {
                throw new ArgumentNullException("appSettingName", "appSettingName is null or empty");
            }
            _keyValue = CloudConfigurationManager.GetSetting(appSettingName);
        }

        /// <summary>
        /// Authorize
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (String.IsNullOrWhiteSpace(_keyValue))
                return;
            string keyValue = GetKeyValueFromContext(DefaultKeyName, filterContext.HttpContext.Request);
            if (keyValue != _keyValue)
            {
                filterContext.Result = new HttpNotFoundResult();
            }            
        }

        public static string GetKeyValueFromContext(string keyName, HttpRequestBase httpRequest)
        {
            if(String.IsNullOrWhiteSpace(keyName) ||httpRequest == null) {return null;}
            string[] values = httpRequest.QueryString.GetValues(keyName);
            if(values == null || values.Length == 0)
            {
                return null;
            }
            return values[0].ToString();
        }
    }
    }
}