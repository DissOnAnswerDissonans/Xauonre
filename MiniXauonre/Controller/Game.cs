using MiniXauonre.Core;
using MiniXauonre.Core.Heroes;
using MiniXauonre.Core.Shops;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MiniXauonre.Graphics;
using Xauonre.Core;

namespace MiniXauonre.Controller
{
    class Game
    {
        public ScreenForm MainForm { get; set; }
        
        public List<Player> Players { get; private set; }
        public Map Maze { get; private set; }
        public Shop Shop { get; private set; }
        public int HeroesPerPlayer { get; private set; }    
        public List<Hero> AvailibleHeroes { get; private set; }

        public int PickStep { get; private set; }
        public List<Tuple<int, GameRules.PickType>> PickSeq { get; private set; }
        
        public Player CurrentPlayer { get; private set; }
        public Hero CurrentHero { get; private set; }
        
        public Hero ChosenHero { get; set; }

        private List<Point> PointTargets { get; set; }
        
        public Func<Game, Hero> TurnFunc { get; private set; } 
        
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
                Players.Add(new Player(this, nameForm.PlayerName));
            }

            Shop = rules.GameShop;
            AvailibleHeroes = rules.AllowedHeroes;
            PickStep = 0;
            PickSeq = rules.DraftSequence;
            GetSpawnPoints = rules.GetSpawnPoints;
            TurnFunc = rules.TurnFunction;
        }

        public void StartGame()
        {
            HeroDraft();
            if (!GamePreparing()) return;
            GameProcess();
            GameFinish();
        }

        private void HeroDraft()
        {
            var draftForm = new DraftForm(this);
            Application.Run(draftForm);
        }

        public void NextPick() => ++PickStep;
        
        private bool GamePreparing()
        {
            try
            {     
                for (int pl = 0; pl < Players.Count; pl++)
                {
                    var v = GetSpawnPoints(pl);
                    for (int h = 0; h < HeroesPerPlayer; h++)
                    {
                        Maze.UnitPositions.Add(Players[pl].Heroes[h], v[h % v.Count]);
                        Players[pl].Heroes[h].Init(Players[pl], Maze, Shop);
                    }

                    Players[pl].InitPlayer();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
            NextHero();
            CurrentHero.StartTurn();
            return true;
        }

        public void ClickedOnTile(Point point, MouseButtons button)
        {
            switch (button)
            {
                case MouseButtons.Left: // Если текущий герой НЕ активный просто выбор героя
                    ChosenHero = Maze.UnitPositions.Where(p => p.Value.Equals(point)).Select(p => p.Key).FirstOrDefault();
                    //List<Effect> pointEffects = Maze.Effects. TODO: 
                    var tile = Maze.MapTiles[point.X, point.Y];
                    
                    break;
                case MouseButtons.Right: 
                    // TODO: если выбран скилл и акт. герой, применить в point
                    break;
                case MouseButtons.Middle:
                    break;
            }
        }

        private void GameProcess()
        {
            MainForm = new ScreenForm(this);
            Application.Run(MainForm);
        }

        private void NextHero()
        {
            CurrentHero = TurnFunc(this);
            CurrentPlayer = CurrentHero.P;
            CurrentPlayer.CurrentHero = CurrentHero;
            ChosenHero = CurrentHero;
        }

        public void EndTurn()
        {
            CurrentPlayer.EndTurn();
            CurrentHero.EndTurn();
            Maze.TickTalents(CurrentPlayer);
            NextHero();
            CurrentHero.StartTurn();
        }

        private Shop GetShop() => CurrentPlayer.GetShop();

        public Hero ChooseTarget(List<Hero> targets)
        {
            PointTargets = targets.Select(x => Maze.UnitPositions[x]).ToList();
            var point = MainForm.ChoosePoint(PointTargets);
            if (point == null) return null;
            return Maze.UnitPositions.Where(x => x.Value.Equals(point)).Select(x => x.Key).FirstOrDefault();
        }
        
        public Point ChoosePoint(List<Point> points) => MainForm.ChoosePoint(points);    

        private void GameFinish()
        {
            
        }
  
    }
}
