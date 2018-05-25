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
                new Cyprys(),
                new Banker(),
                new Geneva(),
            };
        }

        public static Hero CreateHero(string name)
        {
            switch (name)
            {
                    case "Fe11": return new Fe11();
                    case "Thief": return new Thief();
                    case "Drake": return new Drake();
                    case "Sniper": return new Sniper();
                    case "Johny": return new Johny();
                    case "Tupotrof": return new Tupotrof();
                    case "Micro": return new Micro();
                    case "Immortal": return new Immortal();
                    case "Cyprys": return new Cyprys();
                    case "Banker": return new Banker();
                    case "Geneva": return new Geneva();
                    default: return new Hero();
            }
        }

        public static Hero GetCopy(Hero hero)
        {
            return new Hero(hero);
        }
    }
}
