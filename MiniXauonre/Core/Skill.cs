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
        public List<SkillType> SkillTypes { get; set; }
        public Func<string> Explanation { get; set; }
        public string Name { get; set; }
        public double CoolDown { get; set; }
        public double EnergyCost { get; set; }
        public double Timer { get; set; }
        public Func<Hero, bool> Job  { get; set; }
        
        public Func<Hero, bool> Availiable { get; set; }

        public Skill()
        {
            EnergyCost = 0;
            CoolDown = 0;
            SkillTypes = new List<SkillType>();
            Name = "Base";
            Explanation = () => "Nothing";
            Job = (h) => true;
            Availiable = (h) => Timer <= 0 && h.GetEnergy() >= EnergyCost;
        }

        public void Work(Hero hero)
        {
            hero.Targets = new List<Hero>();
            if (Availiable(hero) && Job(hero))
            {
                if(CoolDown != 0)
                    Timer = Math.Max(CoolDown - hero.GetCDReduction(), 1);
                hero.AddEnergy(-EnergyCost);
            }
        }

        public void Tick(double time)
        {
            Timer -= time;
            if (Timer < 0)
                Timer = 0;
        }
    }

    enum SkillType
    {
        Attack,
        Move,
        Special,
        Dot,
        Mag,
        Phys,
        Pure
    }
}
