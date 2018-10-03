using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace Lucid.Infrastructure.Lib.MvcApp.RazorFolders
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

            var assemblyPath = potentialCompiledPath.Substring(compiledViewStart.Length);
            var separator = assemblyPath.IndexOf('/');
            var viewPath = assemblyPath.Substring(separator, assemblyPath.Length - separator);
            assemblyPath = assemblyPath.Substring(0, assemblyPath.IndexOf('/'));
            assemblyPath = assemblyPath.Replace('.', '/');

            if (assemblyPath.StartsWith("Modules/"))
                assemblyPath = $"{assemblyPath}/Module";

            var fullViewPath = $"/{assemblyPath}{viewPath}";

            return fullViewPath;
        }
    }
}
