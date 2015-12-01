using System;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;

namespace Lucid.Web.Routing
{
    public class ActionData
    {
        public Type         ControllerType  { get; protected set; }
        public string       ControllerName  { get; protected set; }
        public string       Namespace       { get; protected set; }
        public string       ActionName      { get; protected set; }
        public string       AreaName        { get; protected set; }
        public int          Depth           { get; protected set; }

        public ActionData(Type controllerType, MethodInfo action, string controllerName, string areaName, int depth)
        {
            ControllerType = controllerType;
            ControllerName = controllerName;
            Namespace = controllerType.Namespace;
            ActionName = action.Name;
            AreaName = areaName;
            Depth = depth;
        }

        public RouteData CreateRouteData(LucidRoute lucidRoute)
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