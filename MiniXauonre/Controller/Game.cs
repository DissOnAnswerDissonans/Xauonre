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
        public List<Player> Players { get; set; }
        public Map Maze { get; set; }
        public Shop Shop { get; set; }
        public int HeroesPerPlayer { get; set; }    
        public List<Hero> AvailibleHeroes { get; set; }
        
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
        
        public bool DraftHeroPick(Hero hero)
        {
            return true;
        }
        
        private void HeroPlacing()
        {
            
        }

        private void GameProcess()
        {
            
        }
        
        private void GameFinish()
        {
            
        }

        
    }
}
