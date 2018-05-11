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
            var gameForm = new GameForm();
            Application.Run(gameForm);
        }
    }
}
