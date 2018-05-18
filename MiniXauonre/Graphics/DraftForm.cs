using MiniXauonre.Core;
using MiniXauonre.Core.Heroes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.InteropServices;
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
        private FlowLayoutPanel HeroesToPickPanel { get; set; }
        private Panel PickingPlayerNamePanel { get; set; }
        
        private Label CurrentHeroLabel { get; set; }
        
        private List<Button> heroPickButtons { get; set; }
        
        private Game Game { get; set; }
        
        public DraftForm(Game game)
        {
            Game = game;
            PlayersPickedHeroesPanel = new Dictionary<Player, FlowLayoutPanel>();
            PickingPlayerNamePanel = new Panel()
            {
                Dock = DockStyle.Top,
                Height = iconSize.Height * 2 / 5 - 4,
                BackColor = Color.White,
            };

            CurrentHeroLabel = new Label()
            {
                Font = new Font(SystemFonts.DefaultFont.FontFamily, iconSize.Height / 4),
                ForeColor = Color.Black,
                AutoSize = true,
            };
            
            PickingPlayerNamePanel.Controls.Add(CurrentHeroLabel);
            
            HeroesToPickPanel = new FlowLayoutPanel()
            {
                FlowDirection = FlowDirection.TopDown,
                Dock = DockStyle.Fill,
                BackColor = Color.Black,
            };

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
                    if (!game.AvailibleHeroes.Contains(hero)) return;
                    switch (game.PickSeq[game.PickStep].Item2)
                    {
                        case GameRules.PickType.Pick:
                            AddHeroToView(hero);
                            RemoveHeroFromView(hero, zzz);
                            break;
                
                        case GameRules.PickType.Ban:
                            RemoveHeroFromView(hero, zzz);
                            break;
                
                        case GameRules.PickType.Choose:
                            AddHeroToView(hero);
                            break;
                    }
                    game.NextPick();
                    if (game.PickStep >= game.HeroesPerPlayer * game.Players.Count)
                        Finish();
                    else
                        UpdateView();
                };
                HeroesToPickPanel.Controls.Add(zzz);
            }
            
            Controls.Add(HeroesToPickPanel);
            
            Controls.Add(PickingPlayerNamePanel);
            
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
                Controls.Add(panel);
            }
                 
            UpdateView();
        }

        private void Finish()
        { 
            HeroesToPickPanel.Controls.Clear();
            HeroesToPickPanel.Click += (sender, args) => { Close();};
            CurrentHeroLabel.Text = @"Ready to play";
        }

        private void UpdateView()
        {
            var stage = Game.PickSeq[Game.PickStep];
            CurrentHeroLabel.Text = stage.Item2 + @": " + Game.Players[stage.Item1].Name;
        }

        private void AddHeroToView(Hero hero)
        {
            var pl = Game.Players[Game.PickSeq[Game.PickStep].Item1];  
            pl.Heroes.Add(HeroMaker.GetCopy(hero));
            PlayersPickedHeroesPanel[pl].Controls.Add(new Button // вместо кнопки надо чтото ещё
            {
                Dock = DockStyle.Left,
                Size = iconSize,
                Margin = iconBorders,
                Image = hero.GetImage(),
                Text = hero.Name,
                BackColor = Color.White,
                TextAlign = ContentAlignment.TopCenter,
            });
        }

        private void RemoveHeroFromView(Hero hero, Button b)
        {
            Game.AvailibleHeroes.Remove(hero);
            b.BackColor = Color.SlateGray;
        }
    }
}