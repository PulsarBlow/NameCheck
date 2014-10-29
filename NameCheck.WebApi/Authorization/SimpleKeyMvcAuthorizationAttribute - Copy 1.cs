using Microsoft.WindowsAzure;
using System;
using System.Web.Mvc;

namespace NameCheck.WebApi
{
    public class SimpleKeyMvcAuthorizationAttribute : AuthorizeAttribute
    {
        private string _key;

        /// <summary>
        /// Creates a new instance of the <see cref="SimpleKeyMvcAuthorizationAttribute"/>
        /// </summary>
        /// <param name="appSettingName"></param>
        public SimpleKeyMvcAuthorizationAttribute(string appSettingName)
        {
            if (String.IsNullOrWhiteSpace(appSettingName))
            {
                throw new ArgumentNullException("appSettingName", "appSettingName is null or empty");
            }
            _key = CloudConfigurationManager.GetSetting(appSettingName);
        }

        /// <summary>
        /// Authorize
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (String.IsNullOrWhiteSpace(_key))
                return;

            string[] values = filterContext.HttpContext.Request.QueryString.GetValues("key");
            if (values == null || values.Length == 0 || values[0].ToString() != _key)
                filterContext.Result = new HttpNotFoundResult();
        }
    }
}