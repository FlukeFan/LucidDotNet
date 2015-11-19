using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Demo.Web.Utility
{
    public class LucidRazorViewEngine : RazorViewEngine
    {
        private IDictionary<Type, LucidViewCollection> _controllers = new Dictionary<Type, LucidViewCollection>();

        public string BaseNamespace { get; protected set; }

        public LucidRazorViewEngine(string baseNamespace)
        {
            BaseNamespace = baseNamespace;
        }

        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            var controller = controllerContext.Controller;

            if (controller == null)
                return base.FindView(controllerContext, viewName, masterName, useCache);

            var controllerType = controller.GetType();

            if (!_controllers.ContainsKey(controllerType))
                lock(_controllers)
                    if (!_controllers.ContainsKey(controllerType))
                        _controllers[controllerType] = new LucidViewCollection(controllerType, BaseNamespace);

            var viewCollection = _controllers[controllerType];
            var details = viewCollection.Details(this, controllerContext, viewName, masterName);

            if (string.IsNullOrEmpty(details.ViewPath))
                return base.FindView(controllerContext, viewName, masterName, useCache);

            var view = CreateView(controllerContext, details.ViewPath, details.MasterPath);
            return new ViewEngineResult(view, this);
        }

        public class LucidViewCollection
        {
            public string Path { get; protected set; }

            public LucidViewCollection(Type controllerType, string baseNamespace)
            {
                var ns = controllerType.Namespace;

                if (ns.StartsWith(baseNamespace))
                    ns = ns.Substring(baseNamespace.Length + 1);

                Path = string.Format("~/{0}", ns.Replace(".", "/"));
            }

            public LucidViewPathResult Details(LucidRazorViewEngine engine, ControllerContext controllerContext, string viewName, string masterName)
            {
                var viewPath = string.Format("{0}/{1}.cshtml", Path, viewName);

                if (!engine.FileExists(controllerContext, viewPath))
                    viewPath = null;

                return new LucidViewPathResult
                {
                    ViewPath = viewPath,
                };
            }
        }

        public class LucidViewPathResult
        {
            public string ViewPath;
            public string MasterPath = "";
        }
    }
}