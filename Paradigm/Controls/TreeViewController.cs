using Paradigm.Core.Model;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Paradigm.Controls
{
    /// <summary>
    /// TreeViewController this class provides controls populating tree views
    /// </summary>
    static class TreeViewController
    {
        
        //this path is sent on call to ListDirectory
        /// <summary>
        /// 
        /// </summary>
        public static string tv_path;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="rt_main"></param>
        /// <param name="rt_output"></param>
        /// <param name="rt_error"></param>
        /// <exception cref="ParadigmException"></exception>
        public static void DisplayNodeContents(TreeNodeMouseClickEventArgs e, 
            RichTextBox rt_main, RichTextBox rt_output)
        {
            try
            {
                if (e.Node.FullPath.Contains("."))
                {
                    string[] sa = tv_path.Split('\\');
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < sa.Length - 1; i++)
                    {
                        sb.AppendFormat("{0}\\", sa[i]);
                    }
                    var s = sb.ToString();
                    //is file
                    rt_main.Text = File.ReadAllText(sb.ToString() + e.Node.FullPath);
                    rt_output.AppendText("Displaying contents for: " + e.Node.FullPath + "\n");
                }
                else
                {
                    //is dir
                    rt_output.AppendText(e.Node.FullPath + " is a directory.\n");
                }
            }
            catch (Exception ex)
            {
                //rt_error.Text = ex.Message + "\\n" + ex.StackTrace;
                throw new ParadigmException(ex.Message);
            }
            
        }
        public static void ListDirectory(TreeView treeView, string path, ContextMenuStrip ct)
        {
            try
            {
                tv_path = path;
                treeView.Nodes.Clear();
                var rootDirectoryInfo = new DirectoryInfo(tv_path);
                treeView.Nodes.Add(CreateDirectoryNode(rootDirectoryInfo, ct));
            }
            catch (Exception ex)
            {
                throw new ParadigmException(ex.Message);
                //TODO: Log stack trace here
            }
            
        }

        private static TreeNode CreateDirectoryNode(DirectoryInfo directoryInfo, ContextMenuStrip ct)
        {
            var directoryNode = new TreeNode(directoryInfo.Name);
            directoryNode.ContextMenuStrip = ct;
            foreach (var directory in directoryInfo.GetDirectories())
            {
                directoryNode.Nodes.Add(CreateDirectoryNode(directory, ct));
            }
                
            foreach (var file in directoryInfo.GetFiles())
            {
                TreeNode tn = new TreeNode(file.Name);
                
                directoryNode.Nodes.Add(tn);
            }
            return directoryNode;
        }
        
        public static void AddResults(TreeView tv, ScannerResults Results, TreeNode ScanTree)
        {
            try
            {
                TreeNode tn = new TreeNode(Results.Name);
                foreach(ScanResult r in Results.Results)
                {
                    tn.Nodes.Add("Found: " + r.PatternUsed + "\n" +
                        "At Line: " + r.LineNumber);
                }

                
                //add to scan tree
                ScanTree.Nodes.Add(tn);
                tv.Nodes.Add(ScanTree);

            }
            catch (Exception ex)
            {
                throw new ParadigmException(ex.Message);
            }
            
            
            // Create two child nodes and put them in an array.
            // ... Add the third node, and specify these as its children.
            //
            //TreeNode node2 = new TreeNode("C#");
            //TreeNode node3 = new TreeNode("VB.NET");
            //TreeNode[] array = new TreeNode[] { node2, node3 };
            ////
            //// Final node.
            ////
            //TreeNode tn = new TreeNode("Dot Net Perls", array);
            //tv.Nodes.Add(tn);

        }
    }
}
