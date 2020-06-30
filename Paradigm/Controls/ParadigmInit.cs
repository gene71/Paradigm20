using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paradigm.Controls
{
    public static class ParadigmInit
    {
        public static void Run()
        {
            makeDirs();
        }
        private static void makeDirs()
        {
            if(!Directory.Exists("ParaData//Projects"))
            {
                Directory.CreateDirectory("ParaData//Projects");
            }

            if (!Directory.Exists("ParaData//ScanConfigs"))
            {
                Directory.CreateDirectory("ParaData//ScanConfigs");
            }

            if (!Directory.Exists("ParaData//Logs"))
            {
                Directory.CreateDirectory("ParaData//Logs");
            }
        }
        
    }
}
