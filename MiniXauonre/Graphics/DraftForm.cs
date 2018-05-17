using MiniXauonre.Core;
using MiniXauonre.Core.Heroes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MiniXauonre.Controller;
using Xauonre.Core;

namespace MiniXauonre.Graphics
{
    class DraftForm : Form  
    {     
        static readonly List<Color> plColors = new List<Color>
        {
            Color.Maroon,
            Color.Indigo,
            Color.DarkGreen,
            Color.DarkBlue,
        };
        
        private readonly Size iconSize = new Size(128, 128);
        private readonly Padding iconBorders = new Padding(8,8,8,8);
        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            DoubleBuffered = true;
            WindowState = FormWindowState.Maximized;
        }
        
        private Dictionary<Player, FlowLayoutPanel> PlayersPickedHeroesPanel { get; set; }   
        private Dictionary<Player, List<Panel>> PlayersPickedHeroesElements { get; set; }
        private FlowLayoutPanel heroesToPickPanel { get; set; }
        private Panel PickingPlayerNamePanel { get; set; }
        
        private List<Button> heroPickButtons { get; set; }
        
        private Game Game { get; set; }
        
        public DraftForm(Game game)
        {
            Game = game;
            PlayersPickedHeroesPanel = new Dictionary<Player, FlowLayoutPanel>();
            PickingPlayerNamePanel = new Panel()
            {
                Dock = DockStyle.Top,
                Height = iconSize.Height / 2,
                BackColor = Color.White,
                ForeColor = Color.Black,
                Font = new Font(SystemFonts.DefaultFont.FontFamily, iconSize.Height / 5),
                Text = "AaaAaAaaAaAAAAaAAaaAAaaaAAa"
            };
            
            heroesToPickPanel = new FlowLayoutPanel()
            {
                FlowDirection = FlowDirection.TopDown,
                Dock = DockStyle.Fill,
                BackColor = Color.Black,
            };

            Controls.Add(PickingPlayerNamePanel);

            foreach (var h in Game.AvailibleHeroes)
            {
                var hero = h;
                var zzz = new Button()
                {
                    Dock = DockStyle.Left,
                    Size = iconSize,
                    Margin = iconBorders,
                    Image = hero.GetImage(),
                    Text = hero.Name,
                    BackColor = Color.White,
                    TextAlign = ContentAlignment.TopCenter,
                };
                zzz.Click += (sender, args) =>
                {
                    if (game.DraftHeroPick(hero))
                    {
                        heroesToPickPanel.Controls.Remove(zzz);
                        UpdateView();
                    }
                };
                heroesToPickPanel.Controls.Add(zzz);
            }
            
            Controls.Add(heroesToPickPanel);
            
            foreach (var player in Game.Players)
            {
                var panel = new FlowLayoutPanel()
                {
                    FlowDirection = FlowDirection.TopDown,
                    Dock = DockStyle.Left,
                    BackColor = plColors[PlayersPickedHeroesPanel.Count % 4],
                    Width = iconSize.Width + iconBorders.Left + iconBorders.Right,
                };
                PlayersPickedHeroesPanel.Add(player, panel);
            }
            
            Controls.AddRange(PlayersPickedHeroesPanel.Values.ToArray()); // Здесь обитает гага        
            UpdateView();
        }

        private void UpdateView()
        {
            foreach (var player in Game.Players)
            {
                var panel = PlayersPickedHeroesPanel[player];
                panel.Controls.Clear();
                foreach (var hero in player.Heroes)
                {
                    panel.Controls.Add(new Button() // TODO: Find good class
                    {
                        Dock = DockStyle.Top,
                        Size = iconSize,
                        Margin = iconBorders,
                        Image = hero.GetImage(),
                        Text = hero.Name,
                        BackColor = Color.White,
                        TextAlign = ContentAlignment.TopCenter,
                    });
                }
            }

            PickingPlayerNamePanel.Text = "AAA"; // curr. pick. player
        }

        private void AddHeroToView()
        {
            
        }

        private void PlayerNameForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}