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
using Point = Xauonre.Core.Point;

namespace MiniXauonre.Graphics
{
    internal class ScreenForm : Form
    {
        private MapPainter MPainter;
        private MapView View;
        private Game Game { get; set; }
        
        private const int fontSize = 8;
        private const int debugFontSize = 16;

        private const int levelPanelSize = 64;

        private FlowLayoutPanel StatPanel { get; set; }
   
        private FlowLayoutPanel ControlPanel { get; set; }

        private Dictionary<Player, PlayerPanel> PlayerPanels { get; set; }
        
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
                Location = new System.Drawing.Point(ClientSize.Width - (256 + 16), ClientSize.Height - (32 + 16)),
                Anchor = (AnchorStyles.Right | AnchorStyles.Bottom),
                Size = new Size(256, 32),
                BackColor = Color.OrangeRed,   
                FlowDirection = FlowDirection.BottomUp
            };       

            ControlPanel = new FlowLayoutPanel
            {
                Bounds = new Rectangle(ClientSize.Width - 272, levelPanelSize + 16, 256, 512),  
                BackColor = Color.MidnightBlue,
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
            
            Controls.Add(ControlPanel);   
            Controls.Add(StatPanel);
            Controls.Add(View);
            Controls.Add(topPanel);
            
            foreach (var v in PlayerPanels.Values)
                v.PanelUpdate(Game);

            SizeChanged += (sender, args) =>
            {
                //StatPanel.Bounds = new Rectangle(ClientSize.Width - 272, ClientSize.Height - 528, 256, 512);
                MPainter.ResizeMap(Size);
                ResizePlayerPanels(PlayerPanels);
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

        public void ClickedOnMap(Point point, MouseButtons b)
        {
            if (Game.Maze.IsInBounds(point))
                Game.ClickedOnTile(point, b);
            StatPanel.Controls.Clear();
            
            var button = new Button()
            {
                Width = StatPanel.Width + 28,
                Height = 32,
                Text = "END TURN",
                Font = new Font(FontFamily.GenericSerif, 16),
                ForeColor = Color.White,
                BackColor = Colors.PlayerDarkColors
                    [Game.Players.IndexOf(Game.CurrentPlayer) % Colors.count]
            };
            button.Click += (sender, args) =>
            {
                Game.EndTurn();
                foreach (var v in PlayerPanels.Values)
                    v.PanelUpdate(Game);
                ClickedOnMap(new Point(-1, -1), MouseButtons.None);
            };
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

    }
}