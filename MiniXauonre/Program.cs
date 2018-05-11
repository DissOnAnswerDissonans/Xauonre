using MiniXauonre.Controller;
using MiniXauonre.Core;
using MiniXauonre.Core.Heroes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xauonre.Core;
using MiniXauonre.Graphics;
using System.Windows.Forms;

namespace MiniXauonre
{
    class Program
    {
        static void Main(string[] args)
        {
            var w = 5;
            var l = 5;
            var heroesNumber = 1;
            var pl1Spawn = new Xauonre.Core.Point(0, 0);
            var pl2Spawn = new Xauonre.Core.Point(l - 1, w - 1);

            var nameForm = new PlayerNameForm();
            Application.Run(nameForm);
            var p1 = nameForm.Player1Name;
            var p2 = nameForm.Player2Name;

            var game = new Game(new List<Tuple<string, Xauonre.Core.Point>>{ Tuple.Create(p1, pl1Spawn),
                    Tuple.Create(p2, pl2Spawn) },
                heroesNumber,
                l,
                w);
            //foreach (var player in game.Players)player.AllDamage = 1000000000;
            game.StartGame();
            
            // 1. game.ChooseHeroes(game.HeroesNumber, HeroMaker.GetAllHeroes());
            
            // 2. game.AddAllHeroesOnMap();
            
            // 3. game.GameProcess();
        }
    }
}
