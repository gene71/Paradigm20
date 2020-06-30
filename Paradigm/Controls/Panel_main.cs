using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Paradigm.Controls
{
    class Panel_main : Panel
    {
        public Panel_main()
        {
            this.ResizeRedraw = true;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var rc = new Rectangle(this.ClientSize.Width - grab, this.ClientSize.Height - grab, grab, grab);
            //ControlPaint.DrawSizeGrip(e.Graphics, this.BackColor, rc);
        }
        
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                //0x00000040;
                //cp.Style |= 0x840000;  // Turn on WS_BORDER + WS_THICKFRAME
                cp.Style |= 0x840000;
                return cp;
            }
        }
        private const int grab = 16;

        //this can be used on other items
        //protected override void WndProc(ref Message m)
        //{
        //    base.WndProc(ref m);
        //    if (m.Msg == 0x84)
        //    {  // Trap WM_NCHITTEST
        //        var pos = this.PointToClient(new Point(m.LParam.ToInt32()));
        //        if (pos.X >= this.ClientSize.Width - grab && pos.Y >= this.ClientSize.Height - grab)
        //            m.Result = new IntPtr(17);  // HT_BOTTOMRIGHT
        //    }
        //}
    }
}
