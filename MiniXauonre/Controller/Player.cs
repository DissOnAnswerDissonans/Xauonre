using MiniXauonre.Core;
using MiniXauonre.Core.Heroes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniXauonre.Graphics;
using System.Windows.Forms;

namespace MiniXauonre.Controller
{
    class Player
    {
        public string Name { get; private set; }
        public List<Hero> Heroes { get; set; }

        private int heroIterator;

        public double AllDamage { get; set; }

        public static double[] Levels =
        {
            0,
            300,
            1000,
            2000,
            3500,
            6000,
            10000,
            16000,
            30000,
            50000,
            100000
        };

        public int Level { get; set; }

        public int LevelUpMoney { get; set; }

        public Player(string name)
        {
            Level = 0;
            AllDamage = 0;
            Name = name;
            Heroes = new List<Hero>();
            heroIterator = 0;

            LevelUpMoney = 100;
        }



        public void LevelUp()
        {
            Level++;
            foreach(var hero in Heroes)
            {
                hero.AddMoney(Level * LevelUpMoney);
                hero.LevelUp();
            }
        }


        public void NotifyAboutDamage(Damage damage)
        {
            AllDamage += damage.Sum();
            while (Level < Levels.Length - 1 && Levels[Level + 1] <= AllDamage)
                LevelUp();
        }

        public Command GetCommand(List<Command> possiblComands)
        {
            var head = new List<string> { Name, AllDamage.ToString() };
            var chosenCommand = possiblComands[GetAnswer(head, possiblComands.Select(c => c.Type.ToString()).ToList())];

            if (chosenCommand.MetaData == null)
                return chosenCommand;

            var data = new List<int>();
            foreach(var question in chosenCommand.MetaData)
                data.Add(GetAnswer(head, question));

            chosenCommand.FillWithData(data);
            return chosenCommand;
        }

        private int GetAnswer(List<string> head, List<string> variants)
        {
            var n = variants.Count;
            if (n == 0)
                return -1;
            if (n == 1)
                return 0;
            /*
            var answer = -1;
            for (var i = 0; i < n; i++)
                Console.WriteLine(i + " " + variants[i]);

            while (answer < 0 || answer >= n)
                while (!int.TryParse(Console.ReadLine(), out answer)) { }
            */
            var form = new ChooseForm(head, variants);
            Application.Run(form);
            return form.Answer;
        }

        public Hero GetNextHero()
        {
            var number = Heroes.Count;
            if (number < 1)
                return null;
            if (number <= heroIterator + 1)
            {
                heroIterator = 0;
                return Heroes.Last();
            }
            heroIterator++;
            return Heroes[heroIterator - 1];
        }
    }
}
