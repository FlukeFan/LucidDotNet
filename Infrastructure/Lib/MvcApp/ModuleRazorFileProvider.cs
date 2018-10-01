using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace Lucid.Infrastructure.Lib.MvcApp
{
    public class ModuleRazorFileProvider : IFileProvider
    {
        private IFileProvider _physicalFileProvider;

        public ModuleRazorFileProvider(string rootSourcePath)
        {
            _physicalFileProvider = new PhysicalFileProvider(rootSourcePath);
        }

        IDirectoryContents IFileProvider.GetDirectoryContents(string subpath)
        {
            var convertedPath = ConvertPath(subpath);
            return _physicalFileProvider.GetDirectoryContents(convertedPath);
        }

        IFileInfo IFileProvider.GetFileInfo(string subpath)
        {
            var convertedPath = ConvertPath(subpath);
            return _physicalFileProvider.GetFileInfo(convertedPath);
        }

        IChangeToken IFileProvider.Watch(string filter)
        {
            var convertedPath = ConvertPath(filter);
            return _physicalFileProvider.Watch(convertedPath);
        }

        private string ConvertPath(string potentialCompiledPath)
        {
            var compiledViewStart = "/Lucid.";

            if (!potentialCompiledPath.StartsWith(compiledViewStart))
                return potentialCompiledPath;

            var namespacePath = potentialCompiledPath.Substring(compiledViewStart.Length);
            var separator = namespacePath.IndexOf('/');
            var viewPath = namespacePath.Substring(separator, namespacePath.Length - separator);
            namespacePath = namespacePath.Substring(0, namespacePath.IndexOf('/'));
            namespacePath = namespacePath.Replace('.', '/');

            if (namespacePath.StartsWith("Modules/"))
                namespacePath = $"{namespacePath}/Module";

            var fullViewPath = $"/{namespacePath}{viewPath}";

            return fullViewPath;
        }
    }
}
