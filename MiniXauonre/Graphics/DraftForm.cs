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
        private readonly Size iconSize = new Size(128, 128);
        private readonly Padding iconBorders = new Padding(8,8,8,8);
        private const int TopPanelHeight = 50;

        protected override void OnLoad(EventArgs e)
        { 
            base.OnLoad(e);
            DoubleBuffered = true;
        }
        
        private Dictionary<Player, FlowLayoutPanel> PlayersPickedHeroesPanel { get; set; }   
        private Dictionary<Player, Panel> PlayersPickedHeroesState { get; set; }
        private FlowLayoutPanel HeroesToPickPanel { get; set; }
        private Panel PickingPlayerNamePanel { get; set; }
        
        private Label CurrentHeroLabel { get; set; }
        
        private List<Button> heroPickButtons { get; set; }
        
        private Game Game { get; set; }
        
        public DraftForm(Game game)
        {
            Game = game;
            ClientSize = new Size(
                (iconSize.Width + iconBorders.Left + iconBorders.Right) 
                    * (Game.Players.Count + Game.AvailibleHeroes.Count / Game.HeroesPerPlayer + 1),
                (iconSize.Height + iconBorders.Top + iconBorders.Bottom)
                    * (Game.HeroesPerPlayer) + TopPanelHeight
            );
            PlayersPickedHeroesPanel = new Dictionary<Player, FlowLayoutPanel>();
            PlayersPickedHeroesState = new Dictionary<Player, Panel>();
            PickingPlayerNamePanel = new Panel()
            {
                Dock = DockStyle.Top,
                Height = TopPanelHeight,
                BackColor = Color.White,
            };

            CurrentHeroLabel = new Label()
            {
                Font = new Font(SystemFonts.DefaultFont.FontFamily, TopPanelHeight * 2 / 3),
                ForeColor = Color.Black,
                AutoSize = true,
            };
            
            PickingPlayerNamePanel.Controls.Add(CurrentHeroLabel);
            
            HeroesToPickPanel = new FlowLayoutPanel()
            {
                FlowDirection = FlowDirection.TopDown,
                Dock = DockStyle.Fill,
                BackColor = Color.Black,
                AutoScroll = true
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
                    if (game.PickStep >= game.PickSeq.Count)
                        Finish();
                    else
                        UpdateView();
                };
                HeroesToPickPanel.Controls.Add(zzz);
            }
            
            Controls.Add(HeroesToPickPanel);
            
            Controls.Add(PickingPlayerNamePanel);
            
            for (var p = 0; p < Game.Players.Count; ++p)
            {
                var player = Game.Players[p];
                var statePanel = new Panel()
                {
                    Bounds = new Rectangle(
                        (iconSize.Width + iconBorders.Horizontal) * (Game.Players.Count - p - 1), 
                        ClientSize.Height - TopPanelHeight, 
                        iconSize.Width, 
                        TopPanelHeight
                    ),
                    BackColor = Color.Black,
                    Padding = Padding.Empty,
                    Height = TopPanelHeight - iconBorders.Vertical,                
                };
                
                statePanel.Controls.Add(new Label()
                {
                    Dock = DockStyle.Fill,
                    Font = new Font(SystemFonts.DefaultFont.FontFamily, TopPanelHeight / 3),
                    Text = player.Name,
                    ForeColor = Color.White,
                    TextAlign = ContentAlignment.MiddleRight,
                    Padding = Padding.Empty
                });
                
                var panel = new FlowLayoutPanel()
                {
                    FlowDirection = FlowDirection.TopDown,
                    Dock = DockStyle.Left,
                    BackColor = Colors.PlayerDarkColors[PlayersPickedHeroesPanel.Count % 4],
                    Width = iconSize.Width + iconBorders.Left + iconBorders.Right,
                    WrapContents = false,
                    AutoScroll = true,
                };  
                PlayersPickedHeroesState.Add(player, statePanel);
                PlayersPickedHeroesPanel.Add(player, panel); 
                Controls.Add(statePanel); 
            }
            foreach (var player in Game.Players)
                Controls.Add(PlayersPickedHeroesPanel[player]);
            
            UpdateView();
        }

        private void Finish()
        { 
            foreach (var v in PlayersPickedHeroesState.Values)
            {
                v.BackColor = Color.Black;
                v.Controls[0].ForeColor = Color.White;
            }
            HeroesToPickPanel.Controls.Clear();
            HeroesToPickPanel.Click += (sender, args) => { Close();};
            CurrentHeroLabel.Text = @"Ready to play";
        }

        private void UpdateView()
        {
            var stage = Game.PickSeq[Game.PickStep];
            CurrentHeroLabel.Text = stage.Item2 + @": " + Game.Players[stage.Item1].Name;
            foreach (var v in PlayersPickedHeroesState.Values)
            {
                v.BackColor = Color.Black;
                v.Controls[0].ForeColor = Color.White;
            }

            Player p = Game.Players[Game.PickSeq[Game.PickStep].Item1];
            switch (Game.PickSeq[Game.PickStep].Item2)
            {
                case GameRules.PickType.Ban:
                    PlayersPickedHeroesState[p].Controls[0].ForeColor = Color.Black;
                    PlayersPickedHeroesState[p].BackColor = Color.OrangeRed; break;
                case GameRules.PickType.Pick:
                    PlayersPickedHeroesState[p].Controls[0].ForeColor = Color.Black;
                    PlayersPickedHeroesState[p].BackColor = Color.LimeGreen; break;
                case GameRules.PickType.Choose:
                    PlayersPickedHeroesState[p].Controls[0].ForeColor = Color.Black;
                    PlayersPickedHeroesState[p].BackColor = Color.DeepSkyBlue; break;
                default:
                    PlayersPickedHeroesState[p].Controls[0].ForeColor = Color.White;
                    PlayersPickedHeroesState[p].BackColor = Color.Black; break;
            }
            
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