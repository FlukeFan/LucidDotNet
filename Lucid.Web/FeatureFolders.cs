using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Mvc.Async;
using System.Web.Routing;
using Lucid.Web.Routing;

namespace Lucid.Web
{
    public class FeatureFolders
    {
        public LucidRoute       Route               { get; protected set; }
        public string           RootFolderNamespace { get; protected set; }
        public string           RootFolder          { get; protected set; }
        public FeatureActions   RootActions         { get; protected set; }

        public FeatureFolders(Assembly controllersAssembly, string rootFolderNamespace)
        {
            RootFolderNamespace = rootFolderNamespace;
            RootFolder = rootFolderNamespace.Split('.').Last();
            RootActions = new FeatureActions(null);

            BuildActions(controllersAssembly);
            Route = new LucidRoute(this);
        }

        public static FeatureFolders Register(Assembly controllerAssembly, string rootFolderNamespace, RouteCollection routes, ViewEngineCollection viewEngines)
        {
            var features = new FeatureFolders(controllerAssembly, rootFolderNamespace);

            routes.Add("LucidFeatureFolders", features.Route);

            var engine = new LucidRazorViewEngine(features.RootFolder);
            viewEngines.Clear();
            viewEngines.Add(engine);

            return features;
        }

        public ActionData FindActionData(string[] pathParts, int partIndex)
        {
            return RootActions.FindActionData(pathParts, partIndex);
        }

        protected virtual void BuildActions(Assembly controllersAssembly)
        {
            var controllerTypes = controllersAssembly.GetTypes()
                .Where(ct => IsController(ct) && IsInFeatureFolder(ct));

            foreach (var controllerType in controllerTypes)
                AddControllerType(controllerType);
        }

        protected virtual bool IsController(Type type)
        {
            return typeof(IController).IsAssignableFrom(type) || typeof(IAsyncController).IsAssignableFrom(type);
        }

        protected virtual bool IsInFeatureFolder(Type type)
        {
            return type.Namespace.StartsWith(RootFolderNamespace);
        }

        protected virtual void AddControllerType(Type controllerType)
        {
            const string controllerSuffix = "Controller";

            if (!controllerType.Name.EndsWith(controllerSuffix))
                throw new Exception(string.Format("Expected controller {0} to have name convention <name>Controller", controllerType.FullName));

            var controllerName = controllerType.Name.Substring(0, controllerType.Name.Length - controllerSuffix.Length);

            var controllerFolder = controllerType.Namespace.Substring(RootFolderNamespace.Length + 1);
            var controllerFolders = controllerFolder.Split('.');

            if (controllerFolders.Last() != controllerName)
                throw new Exception(string.Format("Expected controller {0} to have convention of namespace ending in {1}", controllerType.FullName, controllerName));

            string areaName = null;

            if (controllerFolders.Length > 1)
                areaName = string.Join("/", controllerFolders.Take(controllerFolders.Length - 1));

            RootActions.Add(controllerType, controllerName, areaName, controllerFolders);
        }
    }
}
