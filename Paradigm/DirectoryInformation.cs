using Paradigm.Core.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paradigm
{
    /// <summary>
    /// DirectoryInformation provides some methods to capture informatio about a directoy's contents
    /// </summary>
    public class DirectoryInformation
    {
        string directory;


        public DirectoryInformation(string directory)
        {
            this.directory = directory;
        }

        /// <summary>
        /// GetFileCount returns a count of all files in the directory
        /// </summary>
        /// <returns></returns>
        public int GetFileCount()
        {
            //add files
            FileUtil fu = new FileUtil();
            List<string> files = fu.GetFiles(directory);
            return files.Count;
        }

        /// <summary>
        /// GetDirectoryLineCount reads every line in all files in a directory and returns a count of lines.
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public int GetDirectoryLineCount()
        {
            //add files
            FileUtil fu = new FileUtil();
            List<string> files = fu.GetFiles(directory);
            int i = 0;

            foreach (var path in files)
            {
                List<string> lines = new List<string>();
                foreach (var line in File.ReadAllLines(path))
                {
                    i++;
                }
            }

            return i;

        }

        public List<string> GetDirectoryExtensions()
        {
            List<string> extensions = new List<string>();

            //add files
            FileUtil fu = new FileUtil();
            List<string> files = fu.GetFiles(directory);
            foreach (var path in files)
            {
                FileInfo fi = new FileInfo(path);
                if(!extensions.Contains(fi.Extension))
                {
                    extensions.Add(fi.Extension);
                }
            }

                return extensions;
        }



    }
}
