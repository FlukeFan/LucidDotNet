using System.Security.Principal;

namespace Lucid.Web.App.Shared
{
    public class MenuModel
    {
        public string   Layout;
        public bool     IsLoggedIn;
        public string   IdentityText;

        public MenuModel(bool isPjax, IIdentity identity)
        {
            Layout = isPjax
                ? Views.MasterPjax
                : Views.Master;

            if (identity.IsAuthenticated)
            {
                // logged in
                IsLoggedIn = true;
                IdentityText = identity.Name;
            }
            else
            {
                // logged out
                IsLoggedIn = false;
            }
        }
    }
}