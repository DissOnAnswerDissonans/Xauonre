using MiniXauonre.Core;
using MiniXauonre.Core.Heroes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Controller
{
    class Player
    {
        public string Name { get; private set; }
        public List<Hero> Heroes { get; set; }

        private int heroIterator;

        public Player(string name)
        {
            Name = name;
            Heroes = new List<Hero>();
            heroIterator = 0;
        }

        public Command GetCommand(List<Command> possiblComands)
        {
            Console.WriteLine();
            Console.WriteLine(Name);
            var chosenCommand = possiblComands[GetAnswer(possiblComands.Select(c => c.Type.ToString()).ToList())];

            if (chosenCommand.MetaData == null)
                return chosenCommand;


            var data = new List<int>();
            foreach(var question in chosenCommand.MetaData)
                data.Add(GetAnswer(question));

            chosenCommand.FillWithData(data);
            return chosenCommand;
        }

        private int GetAnswer(List<string> variants)
        {
            var n = variants.Count;
            if (n == 0)
                return -1;
            if (n == 1)
                return 0;
            var answer = -1;
            for (var i = 0; i < n; i++)
                Console.WriteLine(i + " " + variants[i]);

            while (answer < 0 || answer >= n)
                while (!int.TryParse(Console.ReadLine(), out answer)) { }
            return answer;
        }

        public Hero GetNextHero()
        {
            var number = Heroes.Count;
            if (number < 1)
                return null;
            if (number == heroIterator + 1)
            {
                heroIterator = 0;
                return Heroes.Last();
            }
            heroIterator++;
            return Heroes[heroIterator - 1];
        }
    }
}
