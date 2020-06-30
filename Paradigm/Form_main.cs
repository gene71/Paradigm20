using Paradigm.Controls;

using Paradigm.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Paradigm.Core.Model;
using System.IO;
using System.Text.RegularExpressions;
using Paradigm.Core.Util;
using System.Xml.Linq;
using CTLib;

namespace Paradigm
{
    public partial class Form_main : Form
    {
        #region Local Vars
        
        List<ScanResults> WorkingScans = new List<ScanResults>();
        string workingDir;
        string workingFilePath;
        List<ScanConfig> availableScanConfigs;
        ParadigmProject workingProject;
        List<string> workingScanSet;//list of directories
        #endregion
        public Form_main()
        {
            InitializeComponent();
            ParadigmInit.Run();
            initAvailScanConfigs();
            workingScanSet = new List<string>();

        }
        private void initAvailScanConfigs()
        {
            availableScanConfigs = new List<ScanConfig>();
            ParaObjSerializer pos = new ParaObjSerializer();
            FileUtil fu = new FileUtil();
            var configs = fu.GetFiles(Environment.CurrentDirectory +
                "\\ParaData\\ScanConfigs");
            foreach(var f in configs)
            {
                ScanConfig sc = pos.LoadScanConfig(f);
                availableScanConfigs.Add(sc);
            }
            comboBox_projScanConfigSelect.Items.Clear();
            foreach(var sc in availableScanConfigs)
            {
                comboBox_projScanConfigSelect.Items.Add(sc.Name);
            }
        }
        private void treeView_ScanDir_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //open node contents and display to main text box
            try
            {
                //call helper to set working file path
                var f = getFullPathFromTreeViewNode(e.Node.FullPath);
                if (f != null)
                {

                    UIVars.SetWorkingFilePath(f + e.Node.FullPath);

                    //set the text box contents
                    richTextBox_main.Text = File.ReadAllText(UIVars.GetWorkingFilePath());
                    richTextBox_output.AppendText("Displaying contents for: " + UIVars.WorkingFileName + "\n");
                }
                else
                {
                    //was null show message
                    richTextBox_output.AppendText(e.Node + ": is not a file.\n");
                }
            }
            catch (Exception ex)
            {
                //TODO: Add Logging
                richTextBox_error.Text = "Error opening: " + e.Node.FullPath + " " + ex.Message;
            }


        }

        private void directoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //call local helper
            openDirectory();
        }

        #region Helpers
        private void updateScanSetTreeView()
        {
            treeView_scanSet.Nodes.Clear();
            foreach(var dir in workingScanSet)
            {
                TreeNode dirTn = new TreeNode(dir);
                FileUtil fu = new FileUtil();
                foreach(var f in fu.GetFiles(dir))
                {
                    TreeNode fileTn = new TreeNode(f);
                    dirTn.Nodes.Add(fileTn);
                }
                treeView_scanSet.Nodes.Add(dirTn);
            }
            
        }
        private void projscanDir(string directory)
        {
            try
            {
                foreach (var item in listBox_scanConfigs.Items)
                {
                    foreach (ScanConfig sc in availableScanConfigs)
                    {
                        if (sc.Name == item.ToString())
                        {
                            //add files
                            FileUtil fu = new FileUtil();
                            sc.Files = fu.GetFiles(directory);

                            //Scan here and add results
                            Scanner scanner = new Scanner();
                            WorkingScans.Add(scanner.GetResults(sc));
                            updateScans();

                            break;
                        }//end scan here
                    }
                }
            }
            catch (Exception ex)
            {
                richTextBox_error.Text = ex.Message;
            }
            
        }
        private void loadProject(string filename)
        {
            ParaObjSerializer pos = new ParaObjSerializer();
            workingProject = pos.LoadProject(filename);
            textBox_projName.Text = workingProject.Name;
            richTextBox_ProjectDes.Text = workingProject.Description;
            listBox_scanConfigs.Items.Clear();
            foreach(var item in workingProject.ScanConfigs)
            {
                listBox_scanConfigs.Items.Add(item.Name);
            }
        }
        private void addScanConfigtoProj()
        {
            if(!listBox_scanConfigs.Items.Contains(comboBox_projScanConfigSelect.Text))
            {
                listBox_scanConfigs.Items.Add(comboBox_projScanConfigSelect.Text);
            }
            
        }
        private void openScanConfig(string filePath)
        {
            try
            {
                ParaObjSerializer pos = new ParaObjSerializer();
                ScanConfig sc = pos.LoadScanConfig(filePath);
                textBox_scanner_name.Text = sc.Name;
                richTextBox_Scanner_Description.Text = sc.Description;
                comboBox_finding_type.Text = sc.Type;
                comboBox_risk.Text = sc.Risk;

                //add patterns to list box
                listBox_Scan.Items.Clear();
                foreach (var item in sc.Patterns)
                {
                    listBox_Scan.Items.Add(item);
                }

                //add file extensions to list box
                listBox_FileExt.Items.Clear();
                foreach (var item in sc.FileExtensions)
                {
                    listBox_FileExt.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                richTextBox_error.Text = ex.Message;
            }
        }
        private void addMultiplePatterns()
        {
            var lines = richTextBox_main.Text.Split('\n');

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i] != "")
                {
                    if (!listBox_Scan.Items.Contains(lines[i]))
                    {
                        listBox_Scan.Items.Add(lines[i]);
                    }

                }
            }

            richTextBox_main.Text = "";
            richTextBox_output.Text = lines.Length + " patterns added to scanner...";
        }
        private void saveScanConfig(string fileName)
        {
            ScanConfig sc = new ScanConfig();

            sc.Name = textBox_scanner_name.Text;
            sc.Description = richTextBox_Scanner_Description.Text;
            sc.Type = comboBox_finding_type.Text;
            sc.Risk = comboBox_risk.Text;

            //add patterns
            foreach (var item in listBox_Scan.Items)
            {
                if (!sc.Patterns.Contains(item.ToString()))
                {
                    sc.Patterns.Add(item.ToString());
                }
            }

            //add file extensions
            foreach (var item in listBox_FileExt.Items)
            {
                if (!sc.FileExtensions.Contains(item.ToString()))
                {
                    sc.FileExtensions.Add(item.ToString());
                }
            }

            //serialize and save to ParaData/ScanConfigs
            ParaObjSerializer pos = new ParaObjSerializer();
            pos.SaveParaObj(fileName, sc);

        }
        private void saveProject(string fileName)
        {
            ParadigmProject pp = new ParadigmProject();
            pp.Name = textBox_projName.Text;
            pp.Description = richTextBox_ProjectDes.Text;

            foreach(string item in listBox_scanConfigs.Items)
            {
                foreach(var sc in availableScanConfigs)
                {
                    if(item == sc.Name)
                    {
                        pp.ScanConfigs.Add(sc);
                        break;
                    }
                }
            }

            //serialize and save to ParaData//Projects
            ParaObjSerializer pos = new ParaObjSerializer();
            pos.SaveParaObj(fileName, pp);



        }
        private ScanResult GetScanResult(string id)
        {
            foreach(var x in WorkingScans)
            {
                foreach(var y in x.Results)
                {
                    foreach(var z in y.Results)
                    {
                        if(z.ID == id)
                        {
                            //found
                            //ScanResult sr = new ScanResult();
                            //sr.ID = z.ID;
                            //sr.Line = z.Line;
                            //sr.LineNumber = z.LineNumber;
                            //sr.PatternUsed = z.PatternUsed;
                            //sr.ScannerName = z.ScannerName;
                            //sr.FilePath = z.FilePath;
                            //sr.Comments = z.Comments;


                            //return
                            return z;

                        
                        }
                    }
                }
            }

            return null;
        }
        private void updateScans()
        {
            try
            {
                treeView_scan_results.Nodes.Clear();//clean up previous scanns
                TreeNode ScanTree = new TreeNode("Scans");
                foreach(var srs in WorkingScans)
                {
                   
                    //add metadata from scan config as the root node of the scan
                    TreeNode tn = new TreeNode(srs.ScanTime.ToString());
                    TreeNode tnP = new TreeNode(srs.Name);

                    int findingCount = 0;
                    foreach(ScannerResults j in srs.Results)
                    {
                        findingCount += j.Results.Count;
                    }

                    tnP.Nodes.Add("Type: " + srs.Type);
                    tnP.Nodes.Add("Risk: " + srs.Risk);
                    TreeNode findings = new TreeNode("Total Findings: " + findingCount);
                    tnP.Nodes.Add(findings);

                    foreach (ScannerResults sr in srs.Results)
                    {
                        
                                     
                        foreach (ScanResult result in sr.Results)
                        {
                            TreeNode resultTn = new TreeNode("ID: " + result.ID);
                            resultTn.Nodes.Add("Found in: " + result.FilePath);
                            resultTn.Nodes.Add("Found at: " + result.LineNumber);
                            resultTn.Nodes.Add("Line: " + result.Line);
                            resultTn.Nodes.Add("Pattern: " + result.PatternUsed);
                            findings.Nodes.Add(resultTn);
                        }
                       

                    }
                    tn.Nodes.Add(tnP);
                    ScanTree.Nodes.Add(tn);
                }
                treeView_scan_results.Nodes.Add(ScanTree);
                tabControl_Panel_main_left.SelectedIndex = 1;
                
               
                

            }
            catch (Exception ex)
            {
                throw new ParadigmException(ex.Message);
            }
        }
        private string getFullPathFromTreeViewNode(string nodeFullpath)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                if (nodeFullpath.Contains("."))
                {
                    string[] sa = UIVars.WorkingDir.Split('\\');
                   
                    for (int i = 0; i < sa.Length - 1; i++)
                    {
                        sb.AppendFormat("{0}\\", sa[i]);
                    }
                    
                    //is file
                    //rt_main.Text = File.ReadAllText(sb.ToString() + e.Node.FullPath);
                    //rt_output.AppendText("Displaying contents for: " + e.Node.FullPath + "\n");
                }
                else
                {
                    //is dir
                   
                    return null;
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                richTextBox_error.Text = "Error getting full path for: " + nodeFullpath
                    + " " + ex.Message;
                return null;
            }
            
        }
        private void scanFile(string filePath)
        {
            try
            {
                ScanConfig scfg = new ScanConfig();
                scfg.Name = textBox_scanner_name.Text;
                scfg.Type = comboBox_finding_type.Text;
                scfg.Risk = comboBox_risk.Text;
                scfg.Files.Add(filePath);

                //add extensions
                foreach (var item in listBox_FileExt.Items)
                {
                    scfg.FileExtensions.Add(item.ToString());
                }

                //add paterns from listbox
                foreach (var item in listBox_Scan.Items)
                {
                    scfg.Patterns.Add(item.ToString());
                }

                //Scan here and add results
                Scanner sc = new Scanner();
                WorkingScans.Add(sc.GetResults(scfg));
                updateScans();
                               
                
            }
            catch (Exception ex)
            {
                richTextBox_error.Text = ex.Message;
            }
        }
        private void scanDir(string directory)
        {
            
            try
            {
                ScanConfig scfg = new ScanConfig();
                scfg.Name = textBox_scanner_name.Text;
                scfg.Type = comboBox_finding_type.Text;
                scfg.Risk = comboBox_risk.Text;

                //add files
                FileUtil fu = new FileUtil();
                scfg.Files = fu.GetFiles(directory);
                
                //add extensions
                foreach(var item in listBox_FileExt.Items)
                {
                    scfg.FileExtensions.Add(item.ToString());
                }
                
                //add paterns from listbox
                foreach (var item in listBox_Scan.Items)
                {
                    scfg.Patterns.Add(item.ToString());
                }

                //Scan here and add results
                Scanner sc = new Scanner();
                WorkingScans.Add(sc.GetResults(scfg));
                updateScans();


            }
            catch (Exception ex)
            {
                richTextBox_error.Text = ex.Message;
            }
        }

        /// <summary>
        /// scanLineCountDir reads every line in all files in a directory and returns a count of lines.
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>

        private int scanLineCountDir(string directory)
        {
            //add files
            FileUtil fu = new FileUtil();
            List<string> files = fu.GetFiles(directory);
            int i = 0;

            foreach(var path in files)
            {
                List<string> lines = new List<string>();
                foreach(var line in File.ReadAllLines(path))
                {
                    i++;
                }
            }

            return i;
        }
        private void scanWithUIScanList(string text)
        {
            try
            {
                
                
                TreeNode tn = new TreeNode("Active Scan");

                foreach (var item in listBox_Scan.Items)
                {
                    Scanner sc = new Scanner();
                    TreeNode resTN = new TreeNode(item.ToString());
                    var sr = sc.GetResultsfromText(text, item.ToString());
                    foreach(var r in sr.Results)
                    {
                        resTN.Nodes.Add(new TreeNode("Found at: " + r.LineNumber));
                        resTN.Nodes.Add(new TreeNode("File Path: " + r.FilePath));
                    }
                    tn.Nodes.Add(resTN);
                    //TreeViewController.AddResults(treeView_scan_results, 
                    //    sc.GetResultsfromText(text, item.ToString()), new TreeNode("Active Scan"));
                }
                treeView_scan_results.Nodes.Add(tn);//add to view
            }
            catch (Exception ex)
            {
                richTextBox_error.AppendText("\n" + ex.Message);
            }
       
            
        }
        private void textDetailUpdate()
        {
            //TODO: Add Error Handling
            int line = richTextBox_main.GetLineFromCharIndex(richTextBox_main.SelectionStart);
            int charindex = richTextBox_main.SelectionStart;
            line++; //account for 0
                                    
            textBox_text_detail.Text = "Line: " + line.ToString() + "\tChar: " 
                + charindex + "\t\t" + UIVars.WorkingFileName;
           
        }
        private void openDirectory()
        {
            try
            {
                //add spinwheel
                DialogResult result = folderBrowserDialog1.ShowDialog();
                if (result == DialogResult.OK)
                {
                    workingDir = folderBrowserDialog1.SelectedPath;
                    UIVars.WorkingDir = workingDir;//will be deprecated
                    TreeViewController.ListDirectory(treeView_ScanDir,
                        UIVars.WorkingDir, contextMenuStrip_nodeHover);
                }
            }
            catch (ParadigmException pe)
            {
                richTextBox_output.Text = pe.Message;
                //TODO: Add Logging
            }
            catch (Exception ex)
            {
                richTextBox_output.Text = "Error opening directory... " + ex.Message;
                //TODO: Log stack trace here
            }
        }
        private void getMatch(string pattern)
        {
            try
            {
                RichTextBoxController.GetMatch(richTextBox_main, pattern);
            }
            catch (ParadigmException pe)
            {
                richTextBox_error.Text = pe.Message;
            }
        }
        #endregion
         
        private void changeDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //call local helper
            openDirectory();
        }

        private void button_Scan_Match_Click(object sender, EventArgs e)
        {
            //call local helper
            getMatch(textBox_Scan.Text);//get the text from scan text box

        }

        private void button_AddtoScanner_Click(object sender, EventArgs e)
        {
            listBox_Scan.Items.Add(textBox_Scan.Text);
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox_Scan.Items.Remove(listBox_Scan.SelectedItem);
        }
        
        //Automatches for the scan input to help build regex patterns in real time
        private void textBox_Scan_TextChanged(object sender, EventArgs e)
        {
            if(checkBox_autoMatch.Checked)
            {
                getMatch(textBox_Scan.Text);
            }
        }

        private void richTextBox_main_KeyUp(object sender, KeyEventArgs e)
        {
            //call local helper
            textDetailUpdate();
        }

        private void richTextBox_main_MouseClick(object sender, MouseEventArgs e)
        {
            //call local helper
            textDetailUpdate();
        }

        private void lineNumbersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(lineNumbersToolStripMenuItem.Checked)
            {
                lineNumbersToolStripMenuItem.Checked = false;
                panel_text_info.Visible = false;
            }
            else
            {
                lineNumbersToolStripMenuItem.Checked = true;
                panel_text_info.Visible = true;
            }
            
        }

        //scan file
        private void textToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
            scanFile(UIVars.GetWorkingFilePath());

        }

        //scan text
        private void textToolStripMenuItem_scan_Text_Click(object sender, EventArgs e)
        {
            scanWithUIScanList(richTextBox_main.Text);
        }

        private void treeView_scan_results_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                if (e.Node.FullPath.Contains("ID: "))
                {
                    string[] sa = Regex.Split(e.Node.FullPath, "ID: ");
                    string s = sa[1];

                    string[] sa1 = s.Split('\\');
                    var id = sa1[0];
                    richTextBox_output.Text = "";
                    
                    ScanResult sr = GetScanResult(id);
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("Selected Details for:\nID: {0}\n", sr.ID);
                    sb.AppendFormat("Scanner: {0}\n", sr.ScannerName);
                    sb.AppendFormat("Found in: {0}\n", sr.FilePath);
                    sb.AppendFormat("Line: {0}\n", sr.LineNumber);
                    sb.AppendFormat("Pattern Used: {0}\n", sr.PatternUsed);
                    richTextBox_output.Text = sb.ToString();

                    //set main out
                    workingFilePath = sr.FilePath;
                    UIVars.SetWorkingFilePath(sr.FilePath);
                    richTextBox_main.Text = File.ReadAllText(sr.FilePath);
                    //highlight the text
                    getMatch(sr.PatternUsed);
                    textDetailUpdate();

                    tabControl_Panel_main_bottom.SelectedIndex = 2;

                }
            }
            catch (Exception ex)
            {
                richTextBox_error.Text = ex.Message;
            }
            
        }

        //scan directory
        private void textToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            //call helper
            scanDir(workingDir);

            Cursor = Cursors.Default;
        }

        private void button_Add_FileExt_Click(object sender, EventArgs e)
        {

            if (!listBox_FileExt.Items.Contains(textBox_fileExt.Text
                ) && textBox_fileExt.Text != "" && Regex.IsMatch(textBox_fileExt.Text,
                "\\.[A-z0-9]+") && !Regex.IsMatch(textBox_fileExt.Text, "[_]+"))
            {
                listBox_FileExt.Items.Add(textBox_fileExt.Text);
                //clear text box
                textBox_fileExt.Text = "";
            }
        }

        private void button_save_scan_config_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName = "ParaData//ScanConfigs//" + textBox_scanner_name.Text + ".xml";

                if (File.Exists(fileName))
                {
                    DialogResult res = MessageBox.Show("File already exists.  Do you want to save?",
                   "Warning", MessageBoxButtons.YesNo,
                   MessageBoxIcon.Warning);

                    if (res.Equals(DialogResult.Yes))
                    {
                        saveScanConfig(fileName);
                    }
                    //else do nothing

                }
                else
                {
                    saveScanConfig(fileName);
                }



            }
            catch (Exception ex)
            {
                richTextBox_error.Text = ex.Message;
            }
        }

        private void button_add_multiple_Click(object sender, EventArgs e)
        {
            try
            {
                addMultiplePatterns();
            }
            catch (Exception ex)
            {
                richTextBox_error.Text = ex.Message;
            }
            
        }

        private void button_open_config_Click(object sender, EventArgs e)
        {
            try
            {
                //setup open file dialog
                openFileDialog1.InitialDirectory = Environment.CurrentDirectory +
                    "\\ParaData\\ScanConfigs";
                openFileDialog1.FileName = "";
                DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
                if (result == DialogResult.OK) // Test result.
                {
                    //call local helper
                    openScanConfig(openFileDialog1.FileName);
                }

            }
            catch (Exception ex)
            {
                richTextBox_error.Text = ex.Message;
            }
        }

        private void button_saveProject_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName = Environment.CurrentDirectory + "\\ParaData\\Projects\\" +
                    textBox_projName.Text + ".xml";
                if (File.Exists(fileName))
                {
                    DialogResult res = MessageBox.Show("File already exists.  Do you want to save?",
                   "Warning", MessageBoxButtons.YesNo,
                   MessageBoxIcon.Warning);

                    if (res.Equals(DialogResult.Yes))
                    {
                        saveProject(fileName);
                        MessageBox.Show("Info",  textBox_projName.Text + " saved.", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    //else do nothing

                }
                else
                {
                    saveProject(fileName);
                    MessageBox.Show("Info", textBox_projName.Text + " saved.",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                
            }
            catch (Exception ex)
            {
                richTextBox_error.Text = ex.Message;
            }
        }

        private void button_addScanConfig_Click(object sender, EventArgs e)
        {
            try
            {
                //call local helper
                addScanConfigtoProj();
            }
            catch (Exception ex)
            {
                richTextBox_error.Text = ex.Message;
            }
        }

        private void removeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            listBox_scanConfigs.Items.Remove(listBox_scanConfigs.SelectedItem);
        }

        private void button_loadProject_Click(object sender, EventArgs e)
        {
            try
            {
                //setup open file dialog
                openFileDialog1.InitialDirectory = Environment.CurrentDirectory +
                    "\\ParaData\\Projects";
                openFileDialog1.FileName = "";
                DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
                if (result == DialogResult.OK) // Test result.
                {
                    //call local helper
                    loadProject(openFileDialog1.FileName);
                    
                }

            }
            catch (Exception ex)
            {
                richTextBox_error.Text = ex.Message;
            }
        }

        private void directoryToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            //call helper
            projscanDir(workingDir);
            Cursor = Cursors.Default;
        }

        private void button_refreshScanConfigs_Click(object sender, EventArgs e)
        {
            initAvailScanConfigs();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
           

            BLReport blr = new BLReport(WorkingScans, workingProject);
            blr.GenerateReport();
                //sr = new StreamWriter("report.xml", false, System.Text.Encoding.UTF8);
                //sr.WriteLine("<" + tv.Nodes[0].Text + ">");
                //foreach (TreeNode node in tv.Nodes)
                //{
                //    saveNode(node.Nodes);
                //}
                ////Close the root node
                //sr.WriteLine("</" + tv.Nodes[0].Text + ">");
                //sr.Close();


                //if(treeView_ScanDir.SelectedNode != null)
                //{
                //    StringBuilder sb = new StringBuilder();
                //    string[] sa = workingDir.Split('\\');
                //    for(int i = 0; i < sa.Length - 1; i++)
                //    {
                //        sb.AppendFormat("{0}\\", sa[i]);
                //    }

                //    var dir = sb.ToString() + "\\" + treeView_ScanDir.SelectedNode.FullPath;
                //    DirectoryInfo di = new DirectoryInfo(dir);
                //    if(di.Exists)
                //    {
                //        if(!workingScanSet.Contains(dir))
                //        {
                //            workingScanSet.Add(dir);
                //        }
                //        //Update the scan set treeview
                //        updateScanSetTreeView();


                //    }
                //    else
                //    {
                //        richTextBox_error.Text = dir + " does not exist...";
                //    }

                //}
                //else
                //{
                //    richTextBox_error.Text = "nothing selected...";
                //}

            }

        private void directoryToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            //get the line count here
            MessageBox.Show(scanLineCountDir(workingDir).ToString(), "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void directoryContentInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DirectoryInformation di = new DirectoryInformation(workingDir);
            
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Total Files: " + di.GetFileCount());
            sb.AppendLine("Total Lines: " + di.GetDirectoryLineCount());
            sb.Append("File Extensions: ");
            foreach (var ext in di.GetDirectoryExtensions())
            {
                sb.Append(ext + ", ");
            }
            sb.AppendLine();

            richTextBox_main.Text = sb.ToString();

        }
    }
}
