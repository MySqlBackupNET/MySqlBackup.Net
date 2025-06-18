using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Routing;

// Author: adriancs
// Github: https://github.com/adriancs2/Auto-Route

namespace adriancs
{
    public class AutoRoute
    {
        /// <summary>
        /// Route all ASPX pages, including sub-folders.
        /// </summary>
        /// <param name="folder">The web path to the folder. For example: "~/pages" or simply start from the root folder "~/"</param>
        public static void Route(string folder)
        {
            Route(folder, true);
        }

        /// <summary>
        /// Route all ASPX pages
        /// </summary>
        /// <param name="folder">The web path to the folder. For example: "~/pages" or simply start from the root folder "~/"</param>
        /// <param name="includeSubFolder">Specifies whether to include pages in sub-folders or not</param>
        public static void Route(string folder, bool includeSubFolder)
        {
            string rootFolder = HttpContext.Current.Server.MapPath("~/");

            if (folder.StartsWith("~/"))
            { }
            else if (folder.StartsWith("/"))
            {
                folder = "~" + folder;
            }
            else
            {
                folder = "~/" + folder;
            }

            folder = HttpContext.Current.Server.MapPath(folder);

            MapPageRoute(folder, rootFolder, includeSubFolder);
        }

        static void MapPageRoute(string folder, string rootFolder, bool includeSubFolder)
        {
            if (includeSubFolder)
            {
                // obtain sub-folders
                string[] folders = Directory.GetDirectories(folder);

                foreach (var subFolder in folders)
                {
                    MapPageRoute(subFolder, rootFolder, includeSubFolder);
                }
            }

            string[] files = Directory.GetFiles(folder);

            foreach (var file in files)
            {
                if (file.EndsWith(".aspx"))
                {
                    string webPath = file.Replace(rootFolder, "~/").Replace("\\", "/");

                    var filename = Path.GetFileNameWithoutExtension(file);

                    if (filename.ToLower() == "default")
                    {
                        continue;
                    }

                    RouteTable.Routes.MapPageRoute(filename, filename, webPath);
                }
            }
        }
    }
}
