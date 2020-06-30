using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paradigm.Core.Model
{
    class ParadigmException : Exception
    {
        public ParadigmException(string Message) : base("ParadigmException: " + Message)
        {

        }
            

    }
}
