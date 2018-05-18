using System;
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
        
        public List<Tuple<int, PickType>> DraftSequence { get; protected set; }

        public GameRules()
        {
            HeroesPerPlayer = 3;
            PlayersNumber = 2;
            GameMap = new Map();
            GameShop = new BasicShop();
            AllowedHeroes = HeroMaker.GetAllHeroes();
            DraftSequence = GenerateDraft(DraftType.Normal, PlayersNumber, HeroesPerPlayer);
        }

        public List<Tuple<int, PickType>> GenerateDraft(DraftType type, int pl, int h)
        {
            List<Tuple<int, PickType>> seq = new List<Tuple<int, PickType>>();
            if (type == DraftType.Normal)
            {
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
            Give
        }

        public enum DraftType
        {
            Normal,
            OneTwo,
        }
    }
}