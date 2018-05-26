using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MiniXauonre.Controller;
using MiniXauonre.Core;
using MiniXauonre.Core.Heroes;

namespace MiniXauonre.Graphics
{
    class HeroInfo : TableLayoutPanel
    {        
        private Hero Hero { get; set; }
        
        private static readonly Size iconSize = new Size(32, 32);
        private readonly Dictionary<string, Bitmap> StatIcons = resources.IconLoader.GetIcons(iconSize);

        private readonly Size panelSize = new Size(iconSize.Width*4, iconSize.Height);
        
        private readonly Font textFont = new Font(FontFamily.GenericSansSerif, 20);
 
        public HeroInfo(Hero h)
        {
            BackColor = Color.Black;
            Padding = new Padding(8);
            RowCount = 8;
            ColumnCount = 2;
            Hero = h;
            AutoSize = true;
            MinimumSize = new Size(256, 0);
            MaximumSize = new Size(256, 1000);
            var counter = 0;
            foreach (var stat in h.GetAllStats())
            {
                if (!StatIcons.ContainsKey(stat.Key)) continue;
                var panel = new Panel{Size = panelSize,};
                panel.Controls.Add(new Label
                {
                    ForeColor = Color.White,
                    Dock = DockStyle.Fill,
                    Text = (((int)(stat.Value * 10)) / 10f).ToString(),
                    Font = textFont,
                    TextAlign = (stat.Key == "HP" || stat.Key == "E") ?
                        ContentAlignment.MiddleRight : ContentAlignment.MiddleCenter,
                });
                panel.Controls.Add(new PictureBox
                {
                    Dock = DockStyle.Left,
                    Image = StatIcons[stat.Key],
                    Size = iconSize
                });                
                Controls.Add(panel, counter % 2, counter / 2);
                ++counter;
                if (stat.Key != "HP" && stat.Key != "E") continue;
                var additPanel = new Panel{Size = panelSize};
                switch (stat.Key)
                {
                    case "HP":
                        additPanel.Controls.Add(new Label
                        {
                            ForeColor = Color.White,
                            Dock = DockStyle.Fill,
                            Text = @"/ " + ((int)Hero.GetMaxHp()),
                            Font = textFont,
                            TextAlign = ContentAlignment.MiddleLeft,
                        });
                        break;
                    case "E":
                        additPanel.Controls.Add(new Label
                        {
                            ForeColor = Color.White,
                            Dock = DockStyle.Fill,
                            Text = @"/ " + ((int)Hero.GetMaxEnergy()),
                            Font = textFont,
                            TextAlign = ContentAlignment.MiddleLeft,
                        });
                        break;
                }
                Controls.Add(additPanel, counter % 2, counter / 2);
                ++counter;
            } 
        }
    }
}