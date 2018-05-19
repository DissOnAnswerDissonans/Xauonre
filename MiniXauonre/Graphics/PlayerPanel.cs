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

namespace MiniXauonre.Graphics
{
    internal class PlayerPanel : FlowLayoutPanel
    {     
        static readonly List<Color> plColors = new List<Color>
        {
            Color.Maroon,
            Color.Indigo,
            Color.DarkGreen,
            Color.DarkBlue,
        };
        
        private Player Player { get; set; }
  
        
        private Panel LevelDisp { get; set; }
        private Panel DamageDisp { get; set; }
        private List<Panel> HeroesPanels { get; set; }

        
        public PlayerPanel(Player player, int num, int width, bool dock)
        {
            DockStyle dockS = dock ? DockStyle.Right : DockStyle.Left;
            Dock = dockS;
            Width = width;
            BackColor = plColors[num % plColors.Count];
            Player = player;

            var h = 64;

            LevelDisp = new Panel
            {
                Dock = dockS,
                Height = h * 7 / 8,
                Width = h * 6 / 8,
                Padding = new Padding(h / 16),
                BackColor = Color.AntiqueWhite,
                AutoSize = true,
            };

            LevelDisp.Controls.Add(new Label
            {
                Text = Player.Level.ToString(),
                Dock = DockStyle.Right,
                Font = new Font(SystemFonts.DefaultFont.FontFamily, h / 2),
                Height = h * 7 / 8,
                Padding = Padding.Empty,
                AutoSize = true,
            });
            
            Controls.Add(LevelDisp);

            DamageDisp = new Panel
            {
                Dock = dockS,
                Height = h * 7 / 8,
                Width = h * 2,
                Padding = new Padding(h / 16),
                BackColor = plColors[num % plColors.Count],
            };

            DamageDisp.Controls.Add(new Label
            {
                Text = (player.Level == Player.Levels.Length - 1) ? (int)player.AllDamage + " DMG" :
                    (int)player.AllDamage + " / " + Player.Levels[Math.Min(player.Level + 1, Player.Levels.Count() - 1)] + " DMG",
                Dock = DockStyle.Fill,
                Font = new Font(SystemFonts.DefaultFont.FontFamily, h / 6),
                AutoSize = true,
                ForeColor = Color.White,
            });  

            DamageDisp.Controls.Add(new Label
            {
                Text = player.Name,
                Dock = DockStyle.Top,
                Font = new Font(SystemFonts.DefaultFont.FontFamily, h / 4),
                AutoSize = true,
                ForeColor = Color.White,
            });

            Controls.Add(DamageDisp);

            HeroesPanels = new List<Panel>();

            foreach (var hero in player.Heroes)
            {
                var panel = new Panel
                {
                    Size = new Size(h * 3 / 2, h * 7 / 8),
                    Padding = new Padding(h / 16),
                    BackColor = hero.Chosen ? Color.LawnGreen : Color.Black,
                    //AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowOnly
                };

                panel.Controls.Add(new Label
                {
                    Text = hero.GetMoney() + "$",
                    Dock = DockStyle.Fill,
                    //Anchor = (AnchorStyles.Bottom | AnchorStyles.Right),
                    Font = new Font(SystemFonts.DefaultFont.FontFamily, h / 4),
                    Height = h * 4 / 8,
                    //AutoSize = true,
                    ForeColor = Color.Red,
                    Padding = Padding.Empty,
                });

                panel.Controls.Add(new Label
                {
                    Text = hero.Name,
                    Dock = DockStyle.Top,
                    //Anchor = (AnchorStyles.Top | AnchorStyles.Left),
                    Font = new Font(SystemFonts.DefaultFont.FontFamily, h / 6),
                    Height = h * 3 / 8,
                    //AutoSize = true,
                    ForeColor = hero.Chosen ? Color.Black : Color.White,
                    Padding = Padding.Empty,
                });
                
                Controls.Add(panel);
                HeroesPanels.Add(panel);
            }
                 
        }

        public new void Resize(int width)
        {
            Width = width;
        }     
    }
}