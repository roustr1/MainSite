using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Application.Dal
{
    public static class ImagePath
    {
        public static class New {
            private static string RootPath { get; set; } = "/images/layout_icons/News/"; //Directory.Combine(Server.Path + "/wwwroot/content/layou_icons/News/");
            public static string AvailabilityFiles { get; } = RootPath + "file.svg";
            public static string MissingFiles { get; } = RootPath + "information.svg";
            public static string UpdateNew { get; } = RootPath + "real-time.svg";
        }

    }
}
