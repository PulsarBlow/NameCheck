using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using SuperMassive;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NameCheck.WebApi
{
    public class AccountController : BaseMvcController
    {
        protected IAuthorizedUserStore AuthorizedUserStore { get; private set; }

        public AccountController(IAuthorizedUserStore authorizedUserStore)
        {
            Guard.ArgumentNotNull(authorizedUserStore, "authorizedUserStore");
            AuthorizedUserStore = authorizedUserStore;
        }

        //
        // GET: /Account/
        [AllowAnonymous]
        public ActionResult Signin(string error, string returnUrl)
        {
            return View(new SigninViewModel { Error = error, ReturnUrl = returnUrl });
        }

        [AllowAnonymous]
        public async Task<ActionResult> OAuth2Callback(string provider, string returnUrl, string error = null)
        {
            if (error != null)
            {
                return Redirect(Url.Content("~/account/signin") + (!String.IsNullOrWhiteSpace(error) ? "?error=" + Uri.EscapeDataString(error) : ""));
            }

            var result = await Authentication.AuthenticateAsync(DefaultAuthenticationTypes.ExternalCookie);

            if (!result.Identity.IsAuthenticated || !IsAuthorized(result.Identity))
            {
                return Redirect(Url.Content("~/account/signin") + (!String.IsNullOrWhiteSpace(error) ? "?error=" + Uri.EscapeDataString(error) : ""));
            }

            Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            var claims = result.Identity.Claims.ToList();
            claims.Add(new Claim(ClaimTypes.AuthenticationMethod, provider));

            var ci = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
            Authentication.SignIn(ci);

            return RedirectToLocal(returnUrl);

        }


        [AllowAnonymous]
        public ActionResult ExternalSignin(string returnUrl, string provider)
        {
            Authentication.Challenge(new AuthenticationProperties
            {
                RedirectUri = Url.Action("OAuth2Callback", new { returnUrl, provider })
            }, provider);
            return new HttpUnauthorizedResult();
        }

        /// <summary>
        /// Logout
        /// </summary>
        /// <returns>Http 200 Result</returns>
        public ActionResult Signout()
        {
            Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("index", "home");
        }

        private bool IsAuthorized(ClaimsIdentity identity)
        {
            if (identity == null)
            {
                return false;
            }

            Claim email = identity.FindFirst(ClaimTypes.Email);
            if (email == null || String.IsNullOrWhiteSpace(email.Value))
            {
                return false;
            }

            return AuthorizedUserStore.IsAuthorized(email.Value);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        protected IAuthenticationManager Authentication
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
    }
}