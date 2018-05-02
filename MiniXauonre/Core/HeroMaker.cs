using MiniXauonre.Core.Heroes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core
{
    static class HeroMaker
    {
        public static List<Hero> GetAllHeroes()
        {
            return new List<Hero>
            {
                new Fe11(),
                new Thief(),
                new Drake(),
                new Sniper(),
                new Johny(),
                new Tupotrof(),
                new Micro(),
                new Immortal(),
            };
        }
    }
}
