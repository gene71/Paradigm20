using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Paradigm.Core;
using Paradigm.Core.Model;

namespace ParadigmTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            ScanConfig sc = new ScanConfig();
            sc.Name = "Default Extensions";
            sc.Description = "Default file extenstions used to determine lines of code";
            sc.FileExtensions.Add(".c");
            sc.FileExtensions.Add(".h");
            sc.FileExtensions.Add(".C");
            sc.FileExtensions.Add(".H");
            sc.FileExtensions.Add(".cpp");
            sc.FileExtensions.Add(".cc");
            sc.FileExtensions.Add(".CC");
            sc.FileExtensions.Add(".CPP");
            sc.FileExtensions.Add(".java");
            sc.FileExtensions.Add(".cs");
            sc.FileExtensions.Add(".py");
            sc.Patterns.Add("//.+");
            sc.Patterns.Add("/\\*");
            sc.Patterns.Add("\\*\\/");

            ParaObjSerializer po = new ParaObjSerializer();
            po.SaveParaObj("defaultExtensions.xml", sc);

        }
    }
}
