using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MiniXauonre.Controller;
using MiniXauonre.Core;
using MiniXauonre.Core.Heroes;

namespace MiniXauonre.Graphics
{
    class CurrentHeroPanel : Panel
    {
        private Hero Hero { get; set; }
        
        private TableLayoutPanel ItemsPanel { get; set; }

        public CurrentHeroPanel(Hero h, ScreenForm f)
        {
            Hero = h;

            AutoSize = true;
            
            Padding = new Padding(2);
            
            //BackColor = Color.AliceBlue;
            
            var b = new Button()
            {
                Image = Hero.GetImage(),
                Dock = DockStyle.Bottom,
                AutoSize = false,
                BackColor = Colors.PlayerDarkColors[h.P.Game.Players.IndexOf(h.P) % Colors.count],
                Margin = new Padding(4),
                Size = new Size(128, 128),
            };

            b.Click += (sender, args) =>
            {
                h.P.Game.ChosenHero = h;
                f.CenterOnPoint(h.M.UnitPositions[h]);
                f.ControlPanelUpdate();
                f.StatPanelUpdate();
            };

            Controls.Add(b);

            ItemsPanel = new TableLayoutPanel()
            {
                Padding = new Padding(0,2,0,0),
                RowCount = 4,
                RowStyles = { new RowStyle(SizeType.AutoSize)},
                Dock = DockStyle.Top,
                Size = new Size(128,0),
                AutoSize = true,
                MaximumSize = new Size(128, 1000),
            };
            
            Controls.Add(ItemsPanel);

            UpdateItems();
        }

        public void UpdateItems()
        {
            ItemsPanel.Controls.Clear();

            foreach (var item in Hero.Items)
            {
                ItemsPanel.Controls.Add(new Label
                {
                    Margin = new Padding(0,0,0,2),
                    Size = new Size(128, 24),
                    BackColor = Color.Black,
                    ForeColor = Color.Beige,
                    Text = item.Name,
                    Font = new Font(FontFamily.GenericSansSerif, 12),
                    TextAlign = ContentAlignment.MiddleCenter,
                    
                });
            }
        }
    }
}