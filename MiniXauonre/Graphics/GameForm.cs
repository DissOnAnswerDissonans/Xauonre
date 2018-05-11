using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MiniXauonre.Controller;
using MiniXauonre.Core;

namespace MiniXauonre.Graphics
{
    public class GameForm : Form
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            DoubleBuffered = true;
            //WindowState = FormWindowState.Maximized;
        }

        public GameForm()
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
        }
    }
}
