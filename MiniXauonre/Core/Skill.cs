using MiniXauonre.Controller;
using MiniXauonre.Core.Heroes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core
{
    class Skill
    {
        public List<SkillType> SkillTypes { get; protected set; }
        public string Explanation { get; set; }
        public string Name { get; set; }
        public double CoolDown { get; set; }
        public double EnergyCost { get; set; }
        private double Timer { get; set; }
        public Func<Map, Player, Hero, bool> Job  { get; set; }

        public Skill()
        {
            EnergyCost = 0;
            CoolDown = 0;
            SkillTypes = new List<SkillType>();
            Name = "Base";
            Explanation = "Nothing";
            Job = (m, p, h) => true;
        }

        public void Work(Map map, Player player, Hero hero)
        {
            if (Timer <= 0 && hero.GetEnergy() >= EnergyCost && Job(map, player, hero))
            {
                Timer = CoolDown;
                hero.AddEnergy(-EnergyCost);
            }
        }

        public void Tick(double CDR)
        {
            Timer -= CDR;
            if (Timer < 0)
                Timer = 0;
        }
    }

    enum SkillType
    {
        Attack,
        Move,
    }
}
