using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MiniXauonre.Core;
using Point = Xauonre.Core.Point;

namespace MiniXauonre.Graphics
{
    class ScreenForm : Form
    {
        private MapPainter MPainter;
        private MapView View;
        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            DoubleBuffered = true;
            WindowState = FormWindowState.Maximized;
        }

        public ScreenForm(Map map)
        {
            MPainter = new MapPainter(map, this.Size);
            View = new MapView(MPainter) { Dock = DockStyle.Fill };
            Controls.Add(View);

            SizeChanged += (sender, args) =>
            {
                MPainter.ResizeMap(Size);
            };
        }
        
        

        public void Update()
        {
            
        }


    }
}