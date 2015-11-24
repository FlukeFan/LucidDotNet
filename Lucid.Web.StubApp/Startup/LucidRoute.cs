using System;
using System.Web;
using System.Web.Routing;

namespace Lucid.Web.StubApp.Startup
{
    public class LucidRoute : RouteBase
    {
        private LucidFeatureFolders _lucidFeatureFolders;

        public LucidRoute(LucidFeatureFolders lucidFeatureFolders)
        {
            _lucidFeatureFolders = lucidFeatureFolders;
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            var request = httpContext.Request;
            var path = (request.AppRelativeCurrentExecutionFilePath + request.PathInfo).Substring(2); // remove ~/
            var pathParts = path.Split('/');
            var actionData = _lucidFeatureFolders.FindActionData(pathParts, 0);

            if (actionData == null)
                return null;

            return actionData.RouteData(this);
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            throw new NotImplementedException();
        }
    }
}