using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Heroes
{
    class Sniper : HeroWithBaseSkills
    {
        public Skill Snipe { get; set; }
        public const double SnipeRange = 30;
        public const double SnipeDamage = 50;
        public const double SnipeApScale = 1;
        public const double SnipeCooldown = 5;
        public const double SnipeEnergyCost = 100;

        public Sniper()
        {
            Name = "Sniper";
            SetMaxHp(800);
            SetAttackPower(60);
            SetAbilityPower(20);
            SetAttackRange(10);
            SetRegen(2);
            SetMaxEnergy(200);
            SetEnergyRegen(4);

            Snipe = new Skill
            {
                Name = "Snipe",
                Explanation = "Deals damage to enemy and root him",
                Job = (m, p, h) =>
                {
                    var enemiesInRange = GetEnemiesInRange(p, m, SnipeRange);
                    if (enemiesInRange.Count != 0)
                    {
                        Target = ChooseTarget(enemiesInRange, p);
                        var damage = new Damage(phys: SnipeDamage + SnipeApScale * GetAbilityPower());
                        Target.GetDamage(damage);
                        return true;
                    }
                    return false;
                },
                CoolDown = SnipeCooldown,
                EnergyCost = SnipeEnergyCost,
            };
            Snipe.SkillTypes.Add(SkillType.Special);
            Skills.Add(Snipe);
        }
    }
}
