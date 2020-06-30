using Paradigm.Core.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Paradigm.Controls
{
    static class RichTextBoxController
    {
        public static void GetMatch(RichTextBox rt, string pattern)
        {
            if (pattern == "." || pattern == "\\")//do not process these without another char due to performance
            {
                return;
            }
            try
            {
                
                    rt.SelectAll();
                    rt.SelectionBackColor = Color.White;
                    foreach (Match m in Regex.Matches(rt.Text, pattern))
                    {
                        rt.Select(m.Index, m.Length);
                        rt.SelectionBackColor = Color.Aqua;

                    }
                    rt.DeselectAll();
               


            }
            catch (Exception ex)
            {
                //does nothing to allow a match for partial regex
                throw new ParadigmException("Error matching expression " + pattern + " - " + ex.Message);
            }

        }
    }
}
