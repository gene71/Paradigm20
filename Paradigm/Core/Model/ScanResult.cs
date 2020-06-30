using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paradigm.Core.Model
{
    public class ScanResult
    {
        public string FilePath { get; set; }
        public int LineNumber { get; set; }
        public string Line { get; set; }
        public string ID { get; set; }
        public string PatternUsed { get; set; }
        public string Comments { get; set; }
        public string ScannerName { get; set; }
        public string Risk { get; set; }
        public string Type { get; set; }
    }
}
