using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace VS2005Commons
{
    public class ApplicationUtils
    {
        public static string CurrentWindowsAppPath()
        {
            return Application.StartupPath;
        }

        public static string CurrentPath(string file)
        {
            return Path.Combine(Application.StartupPath, file);
        }
    }
}
