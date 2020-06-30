using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paradigm.Core.Model
{
    [Serializable]
    public class ParadigmProject
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> ScanSet { get; set; }
        public List<ScanConfig> ScanConfigs { get; set; }

        public ParadigmProject()
        {
            ScanConfigs = new List<ScanConfig>();
        }

    }
}
