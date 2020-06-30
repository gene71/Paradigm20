using Paradigm.Core.Model;
using Paradigm.Core.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Paradigm.Core
{
    public class Scanner
    {
        public ScannerResults GetResultsfromText(string text, string pattern)
        {


            try
            {
                var results = new ScannerResults(pattern);
                
                MatchCollection mc = Regex.Matches(text, pattern);
                char[] ca = text.ToArray();

                foreach (Match m in mc)
                {
                    int line = 1;

                    //get line count for match
                    for (int i = 1; i < m.Index; i++)
                    {

                        if (ca[i] == '\n')
                        {
                            //increment line count
                            line++;
                        }

                    }
                    //make scan result
                    ScanResult sr = new ScanResult();
                    sr.LineNumber = line;
                    sr.PatternUsed = pattern;
                    results.Results.Add(sr);

                }//end foreach match

                return results;
            }
            catch (Exception ex)
            {
                throw new ParadigmException(ex.Message);
            }

        }

        public ScanResults GetResults(ScanConfig sc)
        {
            try
            {
                ScanResults results = new ScanResults();
                ScannerResults srs = new ScannerResults(sc.Name);
                results.Name = srs.Name;
                results.ScanTime = DateTime.Now;
                results.Risk = sc.Risk;
                results.Type = sc.Type;
                
                //foreach file in files scan
                foreach(var f in sc.Files)
                {

                    //switch on file extension
               
                    try
                    {
                        FileInfo fi = new FileInfo(f);
                        foreach (var x in sc.FileExtensions)
                        {
                            if (fi.Extension == x)
                            {
                                //foreach pattern in patterns scan file
                                foreach (var p in sc.Patterns)
                                {
                                    //scan andadd result to results
                                    results.Results.Add(getResult(f, p, sc.Name, sc.Description,
                                        sc.Risk, sc.Type));
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //TODO: add logging
                        //throw new Exception("Error on FileInfo " + f + " " + ex.Message);
                    }
  

                }
                

                //return results
                return results;


            }
            catch (Exception ex)
            {
                throw new ParadigmException(ex.Message);
            }
            
        }

        private ScannerResults getResult(string filePath, string pattern, string scannername,
            string des, string risk, string type)
        {
            try
            {
                string text = File.ReadAllText(filePath);
                ScannerResults srs = new ScannerResults(scannername);
                
                MatchCollection mc = Regex.Matches(text, pattern);
                char[] ca = text.ToArray();
                foreach (Match m in mc)
                {
                    ScanResult sr = new ScanResult();
                    sr.ScannerName = scannername;
                    sr.Comments = des;
                    sr.Risk = risk;
                    sr.Type = type;
                    int line = 1;

                    //get line count for match
                    for (int i = 1; i < m.Index; i++)
                    {

                        if (ca[i] == '\n')
                        {
                            //increment line count
                            line++;
                        }

                    }

                    //get line
                    var lines = File.ReadAllLines(filePath);
                    sr.Line = lines[line - 1];//account for zero numbering
                    sr.ID = Hasher.GetMD5Hash(sr.Line);
                    sr.FilePath = filePath;
                    sr.LineNumber = line;
                    sr.PatternUsed = pattern;
                    srs.Results.Add(sr);

                }
                
                return srs;
            }
            catch (Exception ex)
            {
                throw new ParadigmException("Error Scanner.getResult: " + ex.Message);
            }
           
        }

    }
}
