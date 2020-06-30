using Paradigm.Core.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTLib
{
    public class BLReport
    {
        string webdir = "Web\\";
        string meetpng = "meets.png";
        string nomeetpng = "nomeet.png";
        string partialpng = "partial.png";
        string filePath = "ParadigmReport.html";
        string name;
        ParadigmProject parp;
        List<ScanResults> ws; 
        public BLReport(List<ScanResults> WorkingScans, ParadigmProject pp)
        {
            this.ws = WorkingScans;
            this.parp = pp;
        }
        public void GenerateReport()
        {
            try
            {
                if(ws != null)
                {

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("<!DOCTYPE html>");
                    sb.AppendLine("<html>");
                    sb.AppendLine("<head>");
                    sb.AppendLine("<link rel=\"stylesheet\" type=\"text/css\" href=\"theme.css\">");
                    sb.AppendLine("<script type=\"text/javascript\" src=\"https://www.gstatic.com/charts/loader.js\"></script>");

                    //foreach(var srs in ws)
                    //{
                    //    sb.AppendLine(buildVerPieChart(srs));
                    //}
                    sb.AppendLine(buildVerPieChart());
                    sb.AppendLine(buildNatPieChart());
                    //sb.AppendLine(buildTemPieChart());
                    //sb.AppendLine(buildTarPieChart());
                    sb.AppendLine("</head>");
                    sb.AppendLine("<body>");
                    sb.AppendLine("<h1>Paradigm</h1>");
                    sb.AppendLine("<p>This report provides scan results for the " + parp.Name + " project.</br>");
                    sb.AppendLine("Scan Time: " + DateTime.Now + "</br>");
                    sb.AppendLine("Name: " + parp.Name + "</br>");
                    sb.AppendLine("Project Description: " + parp.Description + "</br>");

                    //foreach(var scanner in parp.ScanConfigs)
                    //{
                    //    sb.AppendLine("Scan Config Used: " + scanner.Name + "</br>");
                        
                    //}
                    
                    sb.AppendLine("<h2>Summary</h2>");
                    sb.AppendLine("<table>");
                    sb.AppendLine("<tr>");
                    sb.AppendLine("<td><div id=\"verpiechart\" style=\"width: 400px; height: 300px; \"></div></td>");
                    sb.AppendLine("<td><div id=\"natpiechart\" style=\"width: 400px; height: 300px; \"></div></td>");
                    sb.AppendLine("<td><div id=\"tempiechart\" style=\"width: 400px; height: 300px; \"></div></td>");
                    sb.AppendLine("<td><div id=\"tarpiechart\" style=\"width: 400px; height: 300px; \"></div></td>");
                    sb.AppendLine("</tr>");
                    sb.AppendLine("</table>");
                    sb.AppendLine("<hr>");
                    sb.AppendLine("<h2>Source Detail</h2>");
                    sb.AppendLine("<table>");
                    sb.AppendLine("<tr class=\"toprow\">");
                    sb.AppendLine("<td><h3>Item</h3></td>");
                    sb.AppendLine("<td><h3>Risk</h3></td>");
                    sb.AppendLine("<td><h3>Type</h3></td>");
                    sb.AppendLine("<td><h3>File : Hash</h3></td>");
                    sb.AppendLine("<td><h3>Line No.</h3></td>");
                    sb.AppendLine("<td><h3>Line</h3></td>");
                    sb.AppendLine(" <td><h3>Fix</h3></td>");
                    sb.AppendLine("</tr>");
                    sb.AppendLine(buildDetailTable());
                    sb.AppendLine("</table>");
                    sb.AppendLine("</body>");
                    sb.AppendLine("</html>");
                    File.WriteAllText(filePath, sb.ToString());
                }
                
            }
            catch (Exception ex)
            {
                //throw new CISSATException("error generating report " + ex.Message);
            }
        }

        /*
        public void GenerateTablePart()
        {
            try
            {
                if (cs != null)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("<!DOCTYPE html>");
                    sb.AppendLine("<html>");
                    sb.AppendLine("<head>");
                    sb.AppendLine("<link rel=\"stylesheet\" type=\"text/css\" href=\"theme.css\">");
                    sb.AppendLine("<script type=\"text/javascript\" src=\"https://www.gstatic.com/charts/loader.js\"></script>");
                    //sb.AppendLine(buildVerPieChart());
                    //sb.AppendLine(buildNatPieChart());
                    //sb.AppendLine(buildTemPieChart());
                    //sb.AppendLine(buildTarPieChart());
                    sb.AppendLine("</head>");
                    sb.AppendLine("<body>");
                    //sb.AppendLine("<h1>Security Baseline</h1>");
                    //sb.AppendLine("<p>This report provides details for the security control implemented for:</br>");
                    //sb.AppendLine("System: " + cs.System + "</br>");
                    //sb.AppendLine("Name: " + cs.Name + "</br>");
                    //sb.AppendLine("SCF: " + cs.ThreatModifier + "</p>");
                    //sb.AppendLine("<h2>Control Summary</h2>");
                    //sb.AppendLine("<table>");
                    //sb.AppendLine("<tr>");
                    //sb.AppendLine("<td><div id=\"verpiechart\" style=\"width: 400px; height: 300px; \"></div></td>");
                    //sb.AppendLine("<td><div id=\"natpiechart\" style=\"width: 400px; height: 300px; \"></div></td>");
                    //sb.AppendLine("<td><div id=\"tempiechart\" style=\"width: 400px; height: 300px; \"></div></td>");
                    //sb.AppendLine("<td><div id=\"tarpiechart\" style=\"width: 400px; height: 300px; \"></div></td>");
                    //sb.AppendLine("</tr>");
                    //sb.AppendLine("</table>");
                    sb.AppendLine("<hr>");
                    sb.AppendLine("<h2>Control Detail</h2>");
                    sb.AppendLine("<table>");
                    sb.AppendLine("<tr class=\"toprow\">");
                    sb.AppendLine("<td><h3>Control Name</h3></td>");
                    sb.AppendLine("<td><h3>Control Description</h3></td>");
                    sb.AppendLine("<td><h3>Control Implementation</h3></td>");
                    sb.AppendLine("<td><h3>Notes</h3></td>");
                    sb.AppendLine(" <td><h3>Verified</h3></td>");
                    sb.AppendLine("</tr>");
                    sb.AppendLine(buildDetailTable());
                    sb.AppendLine("</table>");
                    sb.AppendLine("</body>");
                    sb.AppendLine("</html>");
                    File.WriteAllText(webdir + "tablepart.html", sb.ToString());
                }

            }
            catch (Exception ex)
            {
                throw new CISSATException("error generating report " + ex.Message);
            }

        }
        */
        private string buildVerPieChart()
        {
            try
            {
               
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<script type=\"text/javascript\">");//start chart script
                sb.AppendLine("google.charts.load('current', {'packages':['corechart']});");
                sb.AppendLine("google.charts.setOnLoadCallback(drawChart);");
                sb.AppendLine("function drawChart() {");
                sb.AppendLine("var data = google.visualization.arrayToDataTable([");
                sb.AppendLine("['Status', 'Control'],");
                //sb.AppendLine("['Meets',  0],");
                //sb.AppendLine("['Does Not Meet', " + n + "],");
                //sb.AppendLine("['Partially Meets', " + p + "],");
                //List<ScanResult> results = new List<ScanResult>();
                //foreach (var sr in srs.Results)
                //{

                //    foreach (var res in sr.Results)
                //    {
                //        results.Add(res);
                //        //sb.AppendLine("['" + res.PatternUsed + "', " + sr.Results.Count + "],");
                //    }

                //}
                var results = BuildResults();
                List<string> pattern = new List<string>();
                foreach(var r in results)
                {
                    if(!pattern.Contains(r.ScannerName))
                    {
                        //pattern.Add(r.PatternUsed);
                        pattern.Add(r.ScannerName);
                    }
                }

                foreach(var p in pattern)
                {
                    List<string> pu = new List<string>();
                    foreach(var ps in results)
                    {
                        //if(ps.PatternUsed == p)
                        //{
                        //    pu.Add(p);
                        //}
                        if (ps.ScannerName == p)
                        {
                            pu.Add(p);
                        }


                    }
                    //write to chart
                    sb.AppendLine("['" + pu[0] + "', " + pu.Count + "],");
                }
                



                sb.AppendLine("['na'," + 0 + "]");
                sb.AppendLine("]);");
                sb.AppendLine("var options = {");
                sb.AppendLine("title: 'Scan Coverage'");
                sb.AppendLine(" };");
                sb.AppendLine("var chart = new google.visualization.PieChart(document.getElementById('verpiechart'));");
                sb.AppendLine("chart.draw(data, options);");
                sb.AppendLine("}");
                sb.AppendLine("</script>");//end chart script
                return sb.ToString();
            }
            catch (Exception ex)
            {
                //throw new CISSATException("error generating verpiechart " + ex.Message);
                return null;
            }
            
        }

        private List<ScanResult> BuildResults()
        {
            List<ScanResult> results = new List<ScanResult>();
            foreach (var srs in ws)
            {
                
                foreach (var sr in srs.Results)
                {

                    foreach (var res in sr.Results)
                    {
                        results.Add(res);
                        
                    }

                }
                //sb.AppendLine(buildVerPieChart(srs));
            }
            return results;
        }

        
        private string buildNatPieChart()
        {
            try
            {
                int t = 0;
                int pr = 0;
                int phy = 0;
                if (ws != null)
                {
                    foreach (var c in ws)
                    {
                        if (c.Risk == "Moderate")
                        {
                            t++;
                        }
                        if (c.Type == "INDICATOR")
                        {
                            pr++;
                        }
                        if (c.Type == "CAT II")
                        {
                            phy++;
                        }
                    }
                }
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<script type=\"text/javascript\">");//start chart script
                sb.AppendLine("google.charts.load('current', {'packages':['corechart']});");
                sb.AppendLine("google.charts.setOnLoadCallback(drawChart);");
                sb.AppendLine("function drawChart() {");
                sb.AppendLine("var data = google.visualization.arrayToDataTable([");
                sb.AppendLine("['Nature', 'Control'],");
                sb.AppendLine("['Moderate', " + t + "],");
                sb.AppendLine("['INDICATOR', " + pr + "],");
                sb.AppendLine("['CAT II', " + phy + "]");
                sb.AppendLine("]);");
                sb.AppendLine("var options = {");
                sb.AppendLine("title: 'Risks and Types'");
                sb.AppendLine(" };");
                sb.AppendLine("var chart = new google.visualization.PieChart(document.getElementById('natpiechart'));");
                sb.AppendLine("chart.draw(data, options);");
                sb.AppendLine("}");
                sb.AppendLine("</script>");//end chart script
                return sb.ToString();
            }
            catch (Exception ex)
            {
                //throw new CISSATException("error generating verpiechart " + ex.Message);
                return null;
            }
        }
        /*
        private string buildTemPieChart()
        {
            try
            {
                int p = 0;
                int d = 0;
                int cr = 0;
                if (cs != null)
                {
                    foreach (var c in cs.controls)
                    {
                        if (c.Temporal == "Preventive")
                        {
                            p++;
                        }
                        if (c.Temporal == "Detective")
                        {
                            d++;
                        }
                        if (c.Temporal == "Corrective")
                        {
                            cr++;
                        }
                    }
                }
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<script type=\"text/javascript\">");//start chart script
                sb.AppendLine("google.charts.load('current', {'packages':['corechart']});");
                sb.AppendLine("google.charts.setOnLoadCallback(drawChart);");
                sb.AppendLine("function drawChart() {");
                sb.AppendLine("var data = google.visualization.arrayToDataTable([");
                sb.AppendLine("['Tempo', 'Control'],");
                sb.AppendLine("['Preventive', " + p + "],");
                sb.AppendLine("['Detective', " + d + "],");
                sb.AppendLine("['Corrective', " + cr + "]");
                sb.AppendLine("]);");
                sb.AppendLine("var options = {");
                sb.AppendLine("title: 'Control Tempo'");
                sb.AppendLine(" };");
                sb.AppendLine("var chart = new google.visualization.PieChart(document.getElementById('tempiechart'));");
                sb.AppendLine("chart.draw(data, options);");
                sb.AppendLine("}");
                sb.AppendLine("</script>");//end chart script
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new CISSATException("error generating verpiechart " + ex.Message);
            }
        }
        private string buildTarPieChart()
        {
            try
            {
                int s = 0;
                int o = 0;
               
                if (cs != null)
                {
                    foreach (var c in cs.controls)
                    {
                        if (c.Entity == "System")
                        {
                            s++;
                        }
                        if (c.Entity == "Organization")
                        {
                            o++;
                        }
                        
                    }
                }
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<script type=\"text/javascript\">");//start chart script
                sb.AppendLine("google.charts.load('current', {'packages':['corechart']});");
                sb.AppendLine("google.charts.setOnLoadCallback(drawChart);");
                sb.AppendLine("function drawChart() {");
                sb.AppendLine("var data = google.visualization.arrayToDataTable([");
                sb.AppendLine("['Target', 'Control'],");
                sb.AppendLine("['System', " + s + "],");
                sb.AppendLine("['Organization', " + o + "]");
                sb.AppendLine("]);");
                sb.AppendLine("var options = {");
                sb.AppendLine("title: 'Control Target'");
                sb.AppendLine(" };");
                sb.AppendLine("var chart = new google.visualization.PieChart(document.getElementById('tarpiechart'));");
                sb.AppendLine("chart.draw(data, options);");
                sb.AppendLine("}");
                sb.AppendLine("</script>");//end chart script
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new CISSATException("error generating verpiechart " + ex.Message);
            }
        }
        */
        private string buildDetailTable()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                
                if(ws != null)
                {
                    int i = 1;
                    
                    foreach (var c in BuildResults())
                    {
                        sb.AppendLine("<tr>");
                        sb.AppendLine("<td>" + i + "</td>");//Item
                        sb.AppendLine("<td>" + c.Risk + "</td>");//Risk
                        sb.AppendLine("<td>" + c.Type + "</td>");//Type
                        sb.AppendLine("<td>" + c.FilePath + " : " + c.ID + "</td>");//Description
                        sb.AppendLine("<td>" + c.LineNumber + "</td>");//Name
                        sb.AppendLine("<td>" + c.Line + "</td>");//Implementation
                        sb.AppendLine("<td>" + c.Comments + "</td>");//Notes
                        sb.AppendLine("</tr>");
                        i++;
                    }
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                //throw new CISSATException("error building detail table " + ex.Message);
                return null;
            }
            
        }

        

    }


}
