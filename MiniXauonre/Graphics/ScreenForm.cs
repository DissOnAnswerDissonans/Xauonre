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
using Point = Xauonre.Core.Point;

namespace MiniXauonre.Graphics
{
    internal class ScreenForm : Form
    {
        private MapPainter MPainter { get; set; }
        private MapView View { get; set; }
        private Game Game { get; set; }
        
        private const int fontSize = 8;
        private const int debugFontSize = 16;

        private const int levelPanelSize = 64;

        private FlowLayoutPanel StatPanel { get; set; }
   
        private FlowLayoutPanel ControlPanel { get; set; }

        private Dictionary<Player, PlayerPanel> PlayerPanels { get; set; }
        
        private ShopPanel ShopPanel { get; set; }
        
        private HeroSkillPanel SkillPanel { get; set; }
        
        private CurrentHeroPanel HeroPanel { get; set; }
        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            DoubleBuffered = true;
            WindowState = FormWindowState.Maximized;
        }

        public ScreenForm(Game game)
        {
            MPainter = new MapPainter(game.Maze, this);
            Game = game;

            StatPanel = new FlowLayoutPanel
            {
                Padding = Padding.Empty,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Location = new System.Drawing.Point(ClientSize.Width - (256 + 16), ClientSize.Height - (32 + 16)),
                Anchor = (AnchorStyles.Right | AnchorStyles.Bottom),
                Size = new Size(256, 32),
                BackColor = Color.OrangeRed,   
                FlowDirection = FlowDirection.BottomUp
            };       

            ControlPanel = new FlowLayoutPanel
            {
                Padding = Padding.Empty,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Bounds = new Rectangle(ClientSize.Width - 272, levelPanelSize + 16, 256, 0),  
                BackColor = Color.MidnightBlue,
                FlowDirection = FlowDirection.TopDown,
            };

            var topPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = levelPanelSize,
                BackColor = Color.Black,    
            };
            for (int i = 0; i < Game.Players.Count; i++)
                topPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / Game.Players.Count));

            PlayerPanels = CreatePlayerPanels(topPanel, Game.Players);           
            
            View = new MapView(MPainter) { Dock = DockStyle.Fill };
            
            HeroPanel = new CurrentHeroPanel(Game.CurrentHero, this)
            {
                Size = new Size(128, 128),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
                Location = new System.Drawing.Point(32, ClientSize.Height - 160)
            };
            
            Controls.Add(HeroPanel);
            Controls.Add(ControlPanel);   
            Controls.Add(StatPanel);
            Controls.Add(View);
            Controls.Add(topPanel);
            
            CenterOnPoint(Game.Maze.UnitPositions[Game.CurrentHero]);
            
            PlayerPanelUpdate();
            ControlPanelUpdate();
            StatPanelUpdate();

            SkillPanel = new HeroSkillPanel(Game, Game.CurrentHero, this, ControlPanel.Width);
            
            ShopPanel = new ShopPanel(Game.CurrentHero, this,
                new Rectangle(ClientSize.Width / 4, ClientSize.Height / 4, ClientSize.Width / 2, ClientSize.Height / 2)); 
            
            SizeChanged += (sender, args) =>
            {
                Controls.Remove(ShopPanel);
                Controls.Remove(SkillPanel);
                //StatPanel.Bounds = new Rectangle(ClientSize.Width - 272, ClientSize.Height - 528, 256, 512);
                MPainter.ResizeMap(Size);
                ResizePlayerPanels(PlayerPanels);

                SkillPanel = new HeroSkillPanel(Game, Game.CurrentHero, this, ControlPanel.Width);
                ShopPanel = new ShopPanel(Game.CurrentHero, this,
                    //new Rectangle(ClientSize.Width / 4, ClientSize.Height / 4, ClientSize.Width / 2, ClientSize.Height / 2));
                    new Rectangle(ClientSize.Width / 2 - 640, ClientSize.Height / 2 - 360, 1280, 720));
            }; 
        }

        private Dictionary<Player, PlayerPanel> CreatePlayerPanels(TableLayoutPanel basePanel, List<Player> players)
        {
            var d = new Dictionary<Player, PlayerPanel>();           
            foreach (var p in players)
            {
                d.Add(p, new PlayerPanel(p, d.Count, Size.Width / players.Count, d.Count % 2 != 0));
                basePanel.Controls.Add(d[p], d.Count - 1, 0);
            }
            return d;
        }

        private void ResizePlayerPanels(Dictionary<Player, PlayerPanel> d)
        {
            foreach (var v in d.Values)
                v.Resize((Size.Width - 32) / d.Count);
        }

        public void ControlPanelUpdate()
        {
            ControlPanel.Controls.Clear();
            var panel = new Panel()
            {
                Size = new Size(256, 32),
                BackColor = Color.Black,
            };
            
            var b = new Button()
            {
                ForeColor = Color.Goldenrod,
                Text = "SHOP",
                Font = new Font(FontFamily.GenericSansSerif, 16),
                AutoSize = true,
                Dock = DockStyle.Right,
            };
            b.Click += (sender, args) =>
            {
                Game.ChosenHero = Game.CurrentHero;
                StatPanelUpdate();
                Controls.Add(ShopPanel);
                ShopPanel.BringToFront();
            };
            
            panel.Controls.Add(new Label
            {
                ForeColor = Game.ChosenHero != null ?
                    Colors.PlayerLightColors[Game.Players.IndexOf(Game.ChosenHero.P) % Colors.count]
                    : Colors.PlayerLightColors[Game.Players.IndexOf(Game.CurrentPlayer) % Colors.count],
                BackColor = Game.ChosenHero == Game.CurrentHero || Game.ChosenHero == null ? Color.Black : Color.DarkSlateGray,
                Text = Game.ChosenHero != null ? Game.ChosenHero.Name : Game.CurrentHero.Name,
                Font = new Font(FontFamily.GenericSansSerif, 20),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
            });
  
            ControlPanel.Controls.Add(panel);
            panel.Controls.Add(b);

            if (Game.ChosenHero != null)
            {
                SkillPanel = new HeroSkillPanel(Game, Game.ChosenHero, this, ControlPanel.Width);
                ControlPanel.Controls.Add(SkillPanel);
            }
        }

        public void PlayerPanelUpdate()
        {
            foreach (var v in PlayerPanels.Values)
            {
                v.PanelUpdate(Game);
            }
            HeroPanel.UpdateItems();
        }

        public void MapUpdate() => View.Invalidate();

        public void StatPanelUpdate()
        {
            StatPanel.Controls.Clear();
            var button = new Button()
            {
                Width = 256 + 28,
                Height = 64,
                Text = "END TURN",
                Font = new Font(FontFamily.GenericSerif, 32),
                ForeColor = Color.White,
                BackColor = Colors.PlayerDarkColors
                    [Game.Players.IndexOf(Game.CurrentPlayer) % Colors.count]
            };
            if (Game.Winner == null)
                button.Click += (sender, args) =>
                {
                    EveryUpdate();
                    Controls.Remove(ShopPanel);
                    Game.EndTurn();
                    PlayerPanelUpdate();
                    StatPanelUpdate(); 
                    Controls.Remove(HeroPanel);
                    HeroPanel = new CurrentHeroPanel(Game.CurrentHero, this)
                    {
                        Size = new Size(128, 128),
                        Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
                        Location = new System.Drawing.Point(32, ClientSize.Height - 160)
                    };
                    Controls.Add(HeroPanel);
                    HeroPanel.BringToFront();
                    ShopPanel = new ShopPanel(Game.CurrentHero, this,
                        new Rectangle(ClientSize.Width / 2 - 640, ClientSize.Height / 2 - 360, 1280, 720));
                    
                    SkillPanel = new HeroSkillPanel(Game, Game.CurrentHero, this, ControlPanel.Width);  
                    CenterOnPoint(Game.Maze.UnitPositions[Game.CurrentHero]);
                    ControlPanelUpdate();
                    MapUpdate();
                };
            else
            {
                button.Text = "END GAME";
                button.Click += (sender, args) => this.Close();
            }
            StatPanel.Controls.Add(button);
            
            if (Game.ChosenHero != null)
            { 
                var h = new HeroInfo(Game.ChosenHero);
                
                StatPanel.Controls.Add(h);
                StatPanel.Controls.Add(new Label
                {
                    BackColor = Color.Black,
                    ForeColor = Colors.PlayerLightColors
                        [Game.Players.IndexOf(Game.ChosenHero.P) % Colors.count],
                    Text = Game.ChosenHero.Name,
                    Font = new Font(FontFamily.GenericSansSerif, 16),
                    Width = StatPanel.Width - 6,
                    Height = 32,
                    TextAlign = ContentAlignment.MiddleCenter
                });
            }
            StatPanel.Refresh();
        }

        private Point ClickedPoint { get; set; }

        public Point ChoosePoint(List<Point> points)
        {
            Application.DoEvents();
            MPainter.Av = points;
            View.Invalidate();        
            ClickedPoint = null;
            while (ClickedPoint == null)
            {
                Application.DoEvents();
            }

            var point = ClickedPoint;
            MPainter.Av = new List<Point>();
            Game.ChosenHero = Game.CurrentHero;
            Application.DoEvents();           
            return point;
        }
        
        /*
        private Task<Point> CatchClick()
        {
            var task = new Task<Point>(() => {
                    while (ClickedPoint == null)
                    {
                        Application.DoEvents();
                    }
                    return ClickedPoint;
                }
            );
            task.Start();
            return task;
        }
        */


        public void EveryUpdate()
        {
            Game.GameCheck();
        }

        public void CenterOnPoint(Point point)
        {
            var scale = 128 / (float)Math.Pow(2, View.ZoomScale);
            View.CenterLogicalPos = new PointF(point.X * scale + scale/2, point.Y * scale + scale/2);
            View.Invalidate();
        }

        public void ClickedOnMap(Point point, MouseButtons b)
        {
            EveryUpdate();
            ClickedPoint = point;
            if (Game.Maze.IsInBounds(point))
                Game.ClickedOnTile(point, b);
            
            Controls.Remove(ShopPanel);
            EveryUpdate();
            StatPanelUpdate();     
            ControlPanelUpdate();
        }


        public void GameFinish()
        {
            if (Game.Winner == null)
            {
                MessageBox.Show(
                    "Похоже, что на карте произошел какой-то капец и все умудрились сдохнуть, из-за этого турнир накрылся медным тазом, а директор стадиона обанкротился",
                    "ЖОПА!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                Close();
            }
            else
            {
                MessageBox.Show(
                    "Итак, остался только один игрок, и это — " + Game.Winner.Name + "!\nПоздравляем победителя!",
                    "Победа!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Asterisk
                );
            }
        }

    }
}