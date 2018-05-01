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

        public const double GrowthADPercentBuff = 0.1;
        public const double GrowthMSBuff = 1;
        public const double CutterDamage = 50;
        public const double CutterADScale = 1.2;
        public const double CutterCooldown = 3;

        public Tupotrof()
        {
            Name = "Tupotrof";
            SetMaxHp(900);
            SetAttackPower(40);
            SetAttackSpeed(2);
            SetMovementSpeed(12);

            Growth = new Perk
            {
                LevelUp = (d) =>
                {
                    AddAttackPower(GetAttackPower() * GrowthADPercentBuff);
                    AddMovementSpeed(GrowthMSBuff);
                    return d;
                },
            };

            Cutter = new Skill
            {
                Name = "Cutter",
                Explanation = () => "Deales " + CutterDamage + " + "
                    + CutterADScale * 100 + "% AD ("
                    + (CutterDamage + CutterADScale * GetAttackPower()) + ") phyc damage"
                    + " to all enemies within AA range (" + GetAttackRange() + ").",
                CoolDown = CutterCooldown,
                Job = (m, p, h) =>
                {
                    var damage = new Damage(p, phys: CutterDamage + CutterADScale * GetAttackPower());
                    var targets = GetEnemiesInRange(p, m, GetAttackRange());
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
