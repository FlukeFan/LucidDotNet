using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Lucid.Web.Routing
{
    public class FeatureActions
    {
        public FeatureActions                       Parent      { get; protected set; }
        public IDictionary<string, FeatureActions>  Paths       { get; protected set; }
        public ActionData                           ActionData  { get; protected set; }

        public FeatureActions(FeatureActions parent)
        {
            Parent = parent;
            Paths = new Dictionary<string, FeatureActions>();
        }

        public ActionData FindActionData(string[] pathParts, int partIndex)
        {
            if (partIndex >= pathParts.Length)
                return ActionData;

            var part = pathParts[partIndex].ToLower();

            if (Paths.ContainsKey(part))
                return Paths[part].FindActionData(pathParts, partIndex + 1);

            return ActionData;
        }

        public void Add(Type controllerType, string controllerName, string areaName, string[] controllerFolders, int depth)
        {
            if (controllerFolders.Length > 0)
                AddSubfolder(controllerType, controllerName, areaName, controllerFolders, depth);
            else
                AddControllerActions(controllerType, controllerName, areaName, depth);
        }

        public void AddAction(Type controllerType, MethodInfo action, string controllerName, string areaName, int depth)
        {
            ActionData = new ActionData(controllerType, action, controllerName, areaName, depth);
        }

        private void AddSubfolder(Type controllerType, string controllerName, string areaName, string[] controllerFolders, int depth)
        {
            var folderName = controllerFolders.First().ToLower();

            if (!Paths.ContainsKey(folderName))
                Paths.Add(folderName, new FeatureActions(this));

            var folder = Paths[folderName];
            var subFolders = controllerFolders.Skip(1).ToArray();

            folder.Add(controllerType, controllerName, areaName, subFolders, depth + 1);
        }

        private void AddControllerActions(Type controllerType, string controllerName, string areaName, int depth)
        {
            var actions = controllerType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            foreach (var action in actions)
                AddControllerAction(controllerType, action, controllerName, areaName, depth);
        }

        private void AddControllerAction(Type controllerType, MethodInfo action, string controllerName, string areaName, int depth)
        {
            var actionName = action.Name.ToLower();

            if (!Paths.ContainsKey(actionName))
                Paths.Add(actionName, new FeatureActions(this));

            var actionPath = Paths[actionName];
            actionPath.AddAction(controllerType, action, controllerName, areaName, depth + 1);

            if (actionName == "index")
            {
                AddAction(controllerType, action, controllerName, areaName, depth);

                if (controllerName == "Home" && Parent != null)
                    Parent.AddAction(controllerType, action, controllerName, areaName, depth - 1);
            }
        }
    }
}