using MiniXauonre.Controller;
using MiniXauonre.Core;
using MiniXauonre.Core.Heroes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xauonre.Core;

namespace MiniXauonre
{
    class Program
    {
        static void Main(string[] args)
        {

            var w = 5;
            var l = 5;
            var heroesNumber = 1;
            var pl1Name = Console.ReadLine();
            var pl1Spawn = new Point(0, 0);
            var pl2Name = Console.ReadLine();
            var pl2Spawn = new Point(l - 1, w - 1);

            var game = new Game(new List<Tuple<string, Point>>{ Tuple.Create(pl1Name, pl1Spawn),
                Tuple.Create(pl2Name, pl2Spawn) },
                heroesNumber,
                l,
                w);
            //foreach (var player in game.Players)player.AllDamage = 1000000000;
            game.StartGame();
        }
    }
}
