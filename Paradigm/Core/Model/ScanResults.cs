using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paradigm.Core.Model
{
    public class ScanResults
    {
        public List<ScannerResults> Results;
        public DateTime ScanTime { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Risk { get; set; }

        public ScanResults()
        {
            Results = new List<ScannerResults>();
        }
    }
}
