using System;
using System.Linq;
using System.Web.Mvc;

namespace Lucid.Web.Routing
{
    public class RouteParameterFilter : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            // No Authorization done here.
            //
            // This is implemented as an IAuthorizationFilter in order to intercept the pipeline
            // after routing has created a controllerContext and identified a MethodInfo (ActionDescriptor),
            // but before parameter binding has occurred.

            var routeData = filterContext.RouteData;
            string[] urlParameters = (string[])routeData.DataTokens[LucidRoute.RouteDataParametersKey];

            if (urlParameters == null)
                return;

            var actionDescriptor = filterContext.ActionDescriptor;
            var actionParameters = actionDescriptor.GetParameters();

            for (int i = 0; i < urlParameters.Length; i++)
            {
                if (i >= actionParameters.Length)
                    throw new Exception(
                        string.Format("Too many URL parameters {0} supplied for method {1} on {2}",
                        "/" + string.Join("/", urlParameters),
                        string.Format("{0}({1})", actionDescriptor.ActionName, string.Join(", ", actionParameters.Select(ap => ap.ParameterName))),
                        actionDescriptor.ControllerDescriptor.ControllerType));

                var actionParameter = actionParameters[i];
                var urlParameter = urlParameters[i];
                var parameterName = actionParameter.ParameterName;

                if (routeData.Values.ContainsKey(parameterName))
                    throw new Exception("RouteData already contains parameter: " + parameterName);

                routeData.Values.Add(parameterName, urlParameter);
            }
        }
    }
}
