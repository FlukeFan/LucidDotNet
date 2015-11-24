using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Lucid.Web.StubApp.Startup
{
    public class LucidFeatureActions
    {
        public LucidFeatureActions(LucidFeatureActions parent)
        {
            Parent = parent;
            Paths = new Dictionary<string, LucidFeatureActions>();
        }

        public LucidFeatureActions                      Parent      { get; protected set; }
        public IDictionary<string, LucidFeatureActions> Paths       { get; protected set; }
        public LucidActionData                          ActionData  { get; protected set; }

        public LucidActionData FindActionData(string[] pathParts, int partIndex)
        {
            if (partIndex >= pathParts.Length)
                return ActionData;

            var part = pathParts[partIndex].ToLower();

            if (Paths.ContainsKey(part))
                return Paths[part].FindActionData(pathParts, partIndex + 1);

            return ActionData;
        }

        public void Add(Type controllerType, string controllerName, string[] controllerFolders)
        {
            if (controllerFolders.Length > 0)
                AddSubfolder(controllerType, controllerName, controllerFolders);
            else
                AddControllerActions(controllerType, controllerName, controllerFolders);
        }

        public void AddAction(Type controllerType, MethodInfo action, string controllerName)
        {
            ActionData = new LucidActionData(controllerType, action, controllerName);
        }

        private void AddSubfolder(Type controllerType, string controllerName, string[] controllerFolders)
        {
            var folderName = controllerFolders.First().ToLower();

            if (!Paths.ContainsKey(folderName))
                Paths.Add(folderName, new LucidFeatureActions(this));

            var folder = Paths[folderName];
            var subFolders = controllerFolders.Skip(1).ToArray();

            folder.Add(controllerType, controllerName, subFolders);
        }

        private void AddControllerActions(Type controllerType, string controllerName, string[] controllerFolders)
        {
            var actions = controllerType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            foreach (var action in actions)
                AddControllerAction(controllerType, action, controllerName);
        }

        private void AddControllerAction(Type controllerType, MethodInfo action, string controllerName)
        {
            var actionName = action.Name.ToLower();

            if (!Paths.ContainsKey(actionName))
                Paths.Add(actionName, new LucidFeatureActions(this));

            var actionPath = Paths[actionName];
            actionPath.AddAction(controllerType, action, controllerName);

            if (actionName == "index")
            {
                AddAction(controllerType, action, controllerName);

                if (controllerName == "Home" && Parent != null)
                    Parent.AddAction(controllerType, action, controllerName);
            }
        }
    }
}