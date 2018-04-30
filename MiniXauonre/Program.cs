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
            var pl1Name = Console.ReadLine();
            var pl1Spawn = new Point(0, 0);
            var pl2Name = Console.ReadLine();
            var pl2Spawn = new Point(1, 1);

            var game = new Game(new List<Tuple<string, Point>> { Tuple.Create(pl1Name, pl1Spawn), Tuple.Create(pl2Name, pl2Spawn) });
            game.StartGame();
        }
    }
}
