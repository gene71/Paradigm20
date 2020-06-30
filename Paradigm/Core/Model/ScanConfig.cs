using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paradigm.Core.Model
{
    [Serializable]
    public class ScanConfig
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Risk { get; set; }
        public string Description { get; set; }
        public bool Positive { get; set; }//if true it's a finding
        public List<string> Files { get; set; }
        public List<string> Patterns { get; set; }
        public List<string> FileExtensions { get; set; }

        public ScanConfig()
        {
            Files = new List<string>();
            Patterns = new List<string>();
            FileExtensions = new List<string>();
                        

        }

    
    }
}
