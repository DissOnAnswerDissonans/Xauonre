using System.Collections.Generic;
using MiniXauonre.Core;
using MiniXauonre.Core.Heroes;
using MiniXauonre.Core.Shops;

namespace MiniXauonre.Controller
{
    class GameRules
    {
        public int HeroesPerPlayer { get; protected set; }
        public int PlayersNumber { get; protected set; }
        
        public Map GameMap { get; protected set; }
        
        public Shop GameShop { get; protected set; }
        
        public List<Hero> AllowedHeroes { get; protected set; }

        public GameRules()
        {
            HeroesPerPlayer = 3;
            PlayersNumber = 2;
            GameMap = new Map();
            GameShop = new BasicShop();
            AllowedHeroes = HeroMaker.GetAllHeroes();
        }
    }
}