using System;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;

namespace Lucid.Web.StubApp.Startup
{
    public class LucidActionData
    {
        public Type         ControllerType  { get; protected set; }
        public string       ControllerName  { get; protected set; }
        public string       Namespace       { get; protected set; }
        public MethodInfo   Method          { get; protected set; }
        public string       ActionName      { get; protected set; }
        public string       AreaName        { get; protected set; }

        public LucidActionData(Type controllerType, MethodInfo action, string controllerName, string areaName)
        {
            ControllerType = controllerType;
            ControllerName = controllerName;
            Namespace = controllerType.Namespace;
            Method = action;
            ActionName = action.Name;
            AreaName = areaName;
        }

        public RouteData RouteData(LucidRoute lucidRoute)
        {
            var routeData = new RouteData(lucidRoute, new MvcRouteHandler());

            var dataTokens = routeData.DataTokens;
            dataTokens.Add("Namespaces", new string[] { Namespace });

            if (AreaName != null)
                dataTokens.Add("area", AreaName);

            var values = routeData.Values;
            values.Add("controller", ControllerName);
            values.Add("action", ActionName);

            return routeData;
        }
    }
}