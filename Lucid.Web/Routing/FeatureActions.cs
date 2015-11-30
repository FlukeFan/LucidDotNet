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

        public void Add(Type controllerType, string controllerName, string areaName, string[] controllerFolders)
        {
            if (controllerFolders.Length > 0)
                AddSubfolder(controllerType, controllerName, areaName, controllerFolders);
            else
                AddControllerActions(controllerType, controllerName, areaName);
        }

        public void AddAction(Type controllerType, MethodInfo action, string controllerName, string areaName)
        {
            ActionData = new ActionData(controllerType, action, controllerName, areaName);
        }

        private void AddSubfolder(Type controllerType, string controllerName, string areaName, string[] controllerFolders)
        {
            var folderName = controllerFolders.First().ToLower();

            if (!Paths.ContainsKey(folderName))
                Paths.Add(folderName, new FeatureActions(this));

            var folder = Paths[folderName];
            var subFolders = controllerFolders.Skip(1).ToArray();

            folder.Add(controllerType, controllerName, areaName, subFolders);
        }

        private void AddControllerActions(Type controllerType, string controllerName, string areaName)
        {
            var actions = controllerType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            foreach (var action in actions)
                AddControllerAction(controllerType, action, controllerName, areaName);
        }

        private void AddControllerAction(Type controllerType, MethodInfo action, string controllerName, string areaName)
        {
            var actionName = action.Name.ToLower();

            if (!Paths.ContainsKey(actionName))
                Paths.Add(actionName, new FeatureActions(this));

            var actionPath = Paths[actionName];
            actionPath.AddAction(controllerType, action, controllerName, areaName);

            if (actionName == "index")
            {
                AddAction(controllerType, action, controllerName, areaName);

                if (controllerName == "Home" && Parent != null)
                    Parent.AddAction(controllerType, action, controllerName, areaName);
            }
        }
    }
}