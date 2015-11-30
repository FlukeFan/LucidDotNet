using System.Linq;
using System.Web.Mvc;

namespace Lucid.Web.Routing
{
    public class LucidRazorViewEngine : RazorViewEngine
    {
        public LucidRazorViewEngine(string rootFolder)
        {
            foreach (var fileExtension in FileExtensions)
            {
                var folderViewLocations = new string[]
                {
                    "~/" + rootFolder + "/{1}/{0}." + fileExtension,
                };

                ViewLocationFormats = folderViewLocations.Union(ViewLocationFormats).ToArray();
                MasterLocationFormats = folderViewLocations.Union(MasterLocationFormats).ToArray();
                PartialViewLocationFormats = folderViewLocations.Union(PartialViewLocationFormats).ToArray();

                var areaFolderViewLocations = new string[]
                {
                    "~/" + rootFolder + "/{2}/{1}/{0}." + fileExtension,
                };

                AreaViewLocationFormats = areaFolderViewLocations.Union(AreaViewLocationFormats).ToArray();
                AreaMasterLocationFormats = areaFolderViewLocations.Union(AreaMasterLocationFormats).ToArray();
                AreaPartialViewLocationFormats = areaFolderViewLocations.Union(AreaPartialViewLocationFormats).ToArray();
            }
        }
    }
}