using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using MiniXauonre.Core;
using MiniXauonre.Core.Heroes;
using MiniXauonre.Core.Shops;
using Xauonre.Core;

namespace MiniXauonre.Controller
{
    class GameRules
    {
        public int HeroesPerPlayer { get; protected set; }
        public int PlayersNumber { get; protected set; }
        
        public Map GameMap { get; protected set; }
        
        public Shop GameShop { get; protected set; }
        
        public List<Hero> AllowedHeroes { get; protected set; }
        
        public List<Tuple<int, PickType>> DraftSequence { get; protected set; }
        
        public Func<int, List<Point>> GetSpawnPoints { get; protected set; }
        
        public Func<Game, Hero> TurnFunction { get; protected set; }

        public GameRules()
        {
            HeroesPerPlayer = 1;
            PlayersNumber = 2;
            GameMap = new Map(4, 4);
            GameShop = new BasicShop();
            AllowedHeroes = HeroMaker.GetAllHeroes();
            DraftSequence = GenerateDraft(DraftType.Normal, PlayersNumber, HeroesPerPlayer);
            TurnFunction = GenerateTurn;
            GetSpawnPoints = (pl) =>
            {
                Point p;
                switch (pl)
                {
                    case 0:
                        p = new Point(1, 1);
                        break;
                    case 1:
                        p = new Point(GameMap.Length - 2, GameMap.Width - 2);
                        break;
                    case 2:
                        p = new Point(1, GameMap.Width - 2);
                        break;
                    case 3:
                        p = new Point(GameMap.Length - 2, 1);
                        break;
                    default:
                        throw new ArgumentException();
                }
                return new List<Point>()
                {
                    p,
                    p + new Point(1, 1),
                    p + new Point(-1, -1),
                    p + new Point(1, -1),
                    p + new Point(-1, 1),
                };
            };
        }
        
        private static Hero GenerateTurn(Game game)
        {
            if (game.CurrentHero == null)
                return game.Players.SelectMany(p => p.Heroes).First();
            
            var players = game.Players;
            var index = players.FindIndex(p => p == game.CurrentPlayer);
            var nextIndex = (index + 1) % players.Count;
            var nextPlayer = players[nextIndex];
            var heroes = nextPlayer.Heroes;
            var heroindex = heroes.FindIndex(p => p == nextPlayer.CurrentHero);
            var heronextIndex = (heroindex + 1) % heroes.Count;
            var nextHero = heroes[heronextIndex];
            return nextHero;
        }
      

        public static List<Tuple<int, PickType>> GenerateDraft(DraftType type, int pl, int h)
        {
            List<Tuple<int, PickType>> seq = new List<Tuple<int, PickType>>();
            if (type == DraftType.Normal)
            {
                for (int i = 0; i < h; i++)
                    for (int j = 0; j < pl; j++)
                        seq.Add(Tuple.Create(j, PickType.Pick));
            }
            else if (type == DraftType.Choosing)
            {
                for (int i = 0; i < h; i++)
                    for (int j = 0; j < pl; j++)
                        seq.Add(Tuple.Create(j, PickType.Choose));
            }
            else if (type == DraftType.SimpleBanPick)
            {
                for (int i = 0; i < h; i++)
                    for (int j = 0; j < pl; j++)
                        seq.Add(Tuple.Create(j, PickType.Ban));
                for (int i = 0; i < h; i++)
                    for (int j = 0; j < pl; j++)
                        seq.Add(Tuple.Create(j, PickType.Pick));
            }
            return seq;
        }
        
        
        
        public enum PickType
        {
            None,
            Pick,
            Ban,
            Give,
            Choose
        }

        public enum DraftType
        {
            Normal,
            Choosing,
            OneTwo,
            SimpleBanPick,
        }

        public enum TurnType
        {
            Normal,
        }
    }
}