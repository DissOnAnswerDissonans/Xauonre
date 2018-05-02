using MiniXauonre.Controller;
using MiniXauonre.Core.Heroes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core
{
    class Effect
    {
        public int Timer { get; set; }

        public Hero Creator { get; set; }

        public Action<Map, Player, Hero> Activate { get; set; }

        public Action<Map, Player, Hero> Disactivate { get; set; }

        public Effect(Hero creator, int timer = 1)
        {
            Creator = creator;

            Activate = (m, p, h) => { };
            Disactivate = (m, p, h) => { };
            Timer = timer;
        }


        public void Tick(Map map, Player player, Hero hero)
        {
            Timer--;
            if(Timer < 0)
                Disactivate(map, player, hero);
        }
    }
}
