using System;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace Lucid.Web.Routing
{
    public class LucidRoute : RouteBase
    {
        public const string RouteDataParametersKey = "_lucidRouteDataParameters";

        private FeatureFolders _lucidFeatureFolders;

        public LucidRoute(FeatureFolders lucidFeatureFolders)
        {
            _lucidFeatureFolders = lucidFeatureFolders;
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            var request = httpContext.Request;
            var path = (request.AppRelativeCurrentExecutionFilePath + request.PathInfo).Substring(2); // remove ~/
            var pathParts = path.Split('/').Where(s => !string.IsNullOrEmpty(s)).ToArray();
            var actionData = _lucidFeatureFolders.FindActionData(pathParts, 0);

            if (actionData == null)
                return null;

            var routeData = actionData.CreateRouteData(this);
            var urlParameters = pathParts.Skip(actionData.Depth).ToArray();
            routeData.DataTokens.Add(RouteDataParametersKey, urlParameters);
            return routeData;
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            // TODO implement this so we can use existing routing styles
            throw new NotImplementedException();
        }
    }
}