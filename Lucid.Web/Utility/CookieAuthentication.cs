using System;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Security;

namespace Lucid.Web.Utility
{
    public class CookieAuthentication : IAuthenticationFilter
    {
        public interface IUser
        {
            string Name { get; }
            string CookieValue();
        }

        private const string _cookieName        = "LucidAuth";
        private const string _encryptionPurpose = "LucidCookieAuthentication";

        public static Action<HttpResponseBase, IUser> Authenticate = (HttpResponseBase response, IUser user) =>
        {
            if (!FormsAuthentication.CookiesSupported)
                throw new Exception("FormsAuthentication.CookiesSupported returned false; Cookies are not supported");

            var cookieValue = user.CookieValue();
            var encryptedCookieValue = MachineKey.Protect(Encoding.UTF8.GetBytes(cookieValue), _encryptionPurpose);
            var cookie = new HttpCookie(_cookieName, Convert.ToBase64String(encryptedCookieValue));
            response.Cookies.Add(cookie);
        };

        public static Action<HttpResponseBase> LogOut = (HttpResponseBase response) =>
        {
            var authCookie = response.Cookies[_cookieName];

            if (authCookie != null)
                authCookie.Expires = DateTime.UtcNow.AddDays(-1);
        };

        private string                              _loginAction;
        private Func<AuthenticationContext, bool>   _skipAuthentication;
        private Func<string, IPrincipal>            _createUser;

        public CookieAuthentication(string loginAction, Func<AuthenticationContext, bool> skipAuthentication, Func<string, IPrincipal> createUser)
        {
            _loginAction = (loginAction ?? "").ToLower();
            _skipAuthentication = skipAuthentication;
            _createUser = createUser;
        }

        public void OnAuthentication(AuthenticationContext filterContext)
        {
            if (SkipAuthentication(filterContext))
                return;

            if (!AuthenticateSuccess(filterContext))
                HandleUnauthenticated(filterContext);
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            // OnAuthentication already called by this point, so do nothing here
        }

        private bool SkipAuthentication(AuthenticationContext context)
        {
            var request = context.HttpContext.Request;

            if (request.AppRelativeCurrentExecutionFilePath.ToLower() == _loginAction || request.CurrentExecutionFilePath.ToLower() == _loginAction)
                return true;

            return _skipAuthentication(context);
        }

        private bool AuthenticateSuccess(AuthenticationContext context)
        {
            var request = context.HttpContext.Request;
            var authCookie = request.Cookies[_cookieName];

            if (authCookie == null)
                return false;

            var encryptedCookieValue = Convert.FromBase64String(authCookie.Value);
            var cookieValue = MachineKey.Unprotect(encryptedCookieValue, _encryptionPurpose);
            var user = _createUser(Encoding.UTF8.GetString(cookieValue));

            if (user == null)
                return false;

            context.Principal = user;
            return user.Identity.IsAuthenticated;
        }

        private void HandleUnauthenticated(AuthenticationContext context)
        {
            var url = string.Format("{0}?{1}={2}", _loginAction, "returnUrl", HttpUtility.UrlEncode(context.HttpContext.Request.Url.OriginalString));
            var redirect = new RedirectResult(url);
            context.Result = redirect;
        }
    }
}