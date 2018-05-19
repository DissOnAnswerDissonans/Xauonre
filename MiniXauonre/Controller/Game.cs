using MiniXauonre.Core;
using MiniXauonre.Core.Heroes;
using MiniXauonre.Core.Shops;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MiniXauonre.Graphics;
using Xauonre.Core;

namespace MiniXauonre.Controller
{
    class Game
    {
        public List<Player> Players { get; private set; }
        public Map Maze { get; private set; }
        public Shop Shop { get; private set; }
        public int HeroesPerPlayer { get; private set; }    
        public List<Hero> AvailibleHeroes { get; private set; }

        public int PickStep { get; private set; }
        public List<Tuple<int, GameRules.PickType>> PickSeq { get; private set; }
        
        public Func<int, List<Point>> GetSpawnPoints { get; private set; }
        
        public Game(GameRules rules)
        {
            HeroesPerPlayer = rules.HeroesPerPlayer;
            Maze = rules.GameMap;
            Players = new List<Player>();
            
            for (int i = 0; i < rules.PlayersNumber; i++)
            {
                var nameForm = new PlayerNameForm(i + 1);
                Application.Run(nameForm);
                Players.Add(new Player(nameForm.PlayerName));
            }

            Shop = rules.GameShop;
            AvailibleHeroes = rules.AllowedHeroes;
            PickStep = 0;
            PickSeq = rules.DraftSequence;
            GetSpawnPoints = rules.GetSpawnPoints;
        }

        public void StartGame()
        {
            HeroDraft();
            HeroPlacing();
            GameProcess();
            GameFinish();
        }

        private void HeroDraft()
        {
            var draftForm = new DraftForm(this);
            Application.Run(draftForm);
        }

        public void NextPick() => ++PickStep;
        
        private void HeroPlacing()
        {
            for (int pl = 0; pl < Players.Count; pl++)
            {
                var v = GetSpawnPoints(pl);
                for (int h = 0; h < HeroesPerPlayer; h++)
                {
                    Maze.UnitPositions.Add(Players[pl].Heroes[h], v[h % v.Count]);
                    Players[pl].Heroes[h].Init(Players[pl], Maze, Shop);
                }
            }
        }

        public void ClickedOnTile(Point point, MouseButtons button)
        {
            Console.WriteLine(point.X + " " + point.Y + " " + button);
        }

        private void GameProcess()
        {
            var form = new ScreenForm(this);
            Application.Run(form);
        }
        
        private void GameFinish()
        {
            
        }
        
    }
}
