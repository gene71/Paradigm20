using Paradigm.Core.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paradigm.Controls
{
    public static class UIVars
    {

        private static string workingFilePath;
        public static string WorkingDir { get; set; }
        public static string WorkingFileName { get; set; }
        public static void SetWorkingFilePath(string filePath)
        {
            workingFilePath = filePath;
            FileInfo fi = new FileInfo(workingFilePath);
            WorkingFileName = fi.Name;
        }
        public static string GetWorkingFilePath()
        {
            return workingFilePath;
        }
       
    }
}
