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
using MiniXauonre.Controller;
using MiniXauonre.Core;
using Point = Xauonre.Core.Point;

namespace MiniXauonre.Graphics
{
    internal class ScreenForm : Form
    {
        private MapPainter MPainter;
        private MapView View;
        
        private const int fontSize = 8;
        private const int debugFontSize = 16;

        private const int levelPanelSize = 64;
        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            DoubleBuffered = true;
            WindowState = FormWindowState.Maximized;
        }

        public ScreenForm(Map map, List<Player> players)
        {
            MPainter = new MapPainter(map, this.Size);

            var debugPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.BottomUp,
                Dock = DockStyle.Bottom,
                Height = map.UnitPositions.Count * debugFontSize * 2,
                BackColor = Color.MidnightBlue,
                Font = new Font(SystemFonts.DefaultFont.FontFamily, debugFontSize)       
            };

            var topPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = levelPanelSize,
                BackColor = Color.Black,        
            };

            var playerPanels = CreatePlayerPanels(topPanel, players);           
            
            View = new MapView(MPainter) { Dock = DockStyle.Fill };
            
            Controls.Add(View);
            Controls.Add(debugPanel);
            Controls.Add(topPanel);
           
            foreach (var unit in map.UnitPositions.Keys)
            {
                debugPanel.Controls.Add(new Label()
                {
                    Text = unit.FastStats(),
                    ForeColor = Color.White,
                    AutoSize = true,
                });
            }

            SizeChanged += (sender, args) =>
            {
                MPainter.ResizeMap(Size);
                ResizePlayerPanels(playerPanels);
                debugPanel.Text = Size.Width.ToString() + " : " + Size.Height.ToString();
            };
        }

        private Dictionary<Player, PlayerPanel> CreatePlayerPanels(Panel basePanel, List<Player> players)
        {
            var d = new Dictionary<Player, PlayerPanel>();           
            foreach (var p in players)
            {
                d.Add(p, new PlayerPanel(p, d.Count, Size.Width / players.Count, d.Count % 2 != 0));
                basePanel.Controls.Add(d[p]);
            }
            return d;
        }

        private void ResizePlayerPanels(Dictionary<Player, PlayerPanel> d)
        {
            foreach (var v in d.Values)
                v.Resize((Size.Width - 32) / d.Count);
        }

    }
}