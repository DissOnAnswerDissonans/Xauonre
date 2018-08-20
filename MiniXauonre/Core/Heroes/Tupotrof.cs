using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Heroes
{
    class Tupotrof : HeroWithBaseSkills
    {
        public Skill Cutter { get; set; }
        public Perk Growth { get; set; }

        public const double GrowthADPercentBuff = 0.08;
        public const double GrowthMSBuff = 0.4;
        public const double CutterDamage = 50;
        public const double CutterADScale = 1.2;
        public const double CutterCooldown = 3;

        public Tupotrof()
        {
            Name = "Tupotrof";
            Image = Graphics.resources.Res.Tupotrof;
            
            SetMaxHp(600);
            SetAttackDamage(45);
            SetAttackSpeed(2);
            SetMovementSpeed(11);

            Growth = new Perk
            {
                Name = "Growth",
                Number = (h) => h.Level,
                LevelUp = (a) => (d) =>
                {
                    AddAttackDamage(GetAttackDamage() * GrowthADPercentBuff);
                    AddMovementSpeed(GrowthMSBuff);
                    return a(d);
                },
            };

            Cutter = new Skill
            {
                Name = "Cutter",
                Explanation = () => "Deales " + CutterDamage + " + "
                    + CutterADScale * 100 + "% AD ("
                    + (CutterDamage + CutterADScale * GetAttackDamage()) + ") phyc damage"
                    + " to all enemies within AA range (" + GetAttackRange() + ").",
                CoolDown = CutterCooldown,
                Job = (h) =>
                {
                    var damage = new Damage(h, h.P, phys: CutterDamage + CutterADScale * h.GetAttackDamage());
                    var targets = GetEnemiesInRange(h, h.GetAttackRange());
                    foreach (var target in targets)
                        target.GetDamage(damage);
                    return true;
                },
            };

            Cutter.SkillTypes.Add(SkillType.Special);
            Skills.Add(Cutter);
            Perks.Add(Growth);
        }
    }
}
