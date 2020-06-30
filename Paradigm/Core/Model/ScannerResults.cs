using Paradigm.Core.Model;
using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paradigm.Core.Model
{
    public class ScannerResults
    {
        public List<ScanResult> Results;
        public string Name;
        public DateTime ScanTime {get; set;}

        public ScannerResults(string ScannerName)
        {
            Results = new List<ScanResult>();
            Name = ScannerName;
        }
    }
}
