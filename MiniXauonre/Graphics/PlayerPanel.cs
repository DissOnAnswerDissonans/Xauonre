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
using MiniXauonre.Core.Heroes;

namespace MiniXauonre.Graphics
{
    internal class PlayerPanel : FlowLayoutPanel
    {          
        private Player Player { get; set; }
        
        private Panel LevelDisp { get; set; }
        private Panel DamageDisp { get; set; }
        private Dictionary<Hero, Panel> HeroesPanels { get; set; }

        
        public PlayerPanel(Player player, int num, int width, bool dock)
        {
            DockStyle dockS = dock ? DockStyle.Right : DockStyle.Left;
            Dock = dockS;
            Width = width;
            BackColor = Colors.PlayerDarkColors[num % Colors.count];
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
                BackColor = Colors.PlayerDarkColors[num % Colors.count],
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

            HeroesPanels = new Dictionary<Hero, Panel>();

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
                    ForeColor = Color.White,
                    Padding = Padding.Empty,
                });
                
                Controls.Add(panel);
                HeroesPanels.Add(hero, panel);
            }
                 
        }

        public void PanelUpdate(Game game)
        {
            BorderStyle = BorderStyle.None;
            foreach (var v in HeroesPanels.Values)
                v.BackColor = Color.Black;
            foreach (var v in HeroesPanels.Keys)
                HeroUpdate(v);
            if (Controls.Count == 0)
                return;
            LevelDisp.Controls[0].Text = Player.Level.ToString();
            DamageDisp.Controls[0].Text = (Player.Level == Player.Levels.Length - 1)
                ? (int) Player.AllDamage + " DMG"
                : (int) Player.AllDamage + " / " +
                  Player.Levels[Math.Min(Player.Level + 1, Player.Levels.Count() - 1)] + " DMG";


            if (Player == game.CurrentPlayer)
            {
                //this.BorderStyle = BorderStyle.Fixed3D;
                HeroesPanels[Player.CurrentHero].BackColor = Color.Green;
                
            } 
        }

        public void HeroUpdate(Hero hero)
        {
            var panel = HeroesPanels[hero];
            if(panel.Controls.Count != 0)
                panel.Controls[0].Text = hero.GetMoney() + "$";
        }

        public new void Resize(int width)
        {
            Width = width;
        }     
    }
}