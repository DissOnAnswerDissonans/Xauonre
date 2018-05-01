using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xauonre.Core;

namespace MiniXauonre.Core.Heroes
{
    class Johny : HeroWithBaseSkills
    {
        public Skill Boom { get; set; }
        public const double BoomJumpRange = 9;
        public const double BoomBombRange = 5;
        public const double BoomCooldown = 10;
        public const double BoomEnergyCost = 100;
        public const double BoomDamage = 100;
        public const double BoomDamageAPScale = 1.5;

        public Johny()
        {
            Name = "Johny";
            SetMaxHp(1200);
            SetMaxEnergy(150);
            SetRegen(5);
            SetEnergyRegen(5);

            Boom = new Skill
            {
                Name = "Here comes Johny",
                Explanation = () => "Jumps on enemy in " + BoomJumpRange + " units from you and deales " +
                    + BoomDamage + " + " + BoomDamageAPScale * 100 + "% AP (" 
                    + (BoomDamage + BoomDamageAPScale * GetAbilityPower()) + ") Pure damage to all enemies in "
                    + BoomBombRange + " units around you. Energy cost " + BoomEnergyCost + ". Cooldown " + BoomCooldown + ".",
                Job = (m, p, h) =>
                {
                    var enemiesInRange = GetEnemiesInRange(p, m, BoomJumpRange);
                    if (enemiesInRange.Count != 0)
                    {
                        Target = ChooseTarget(enemiesInRange, p);
                        var damage = new Damage(p, pure: BoomDamage + BoomDamageAPScale * GetAbilityPower());
                        m.UnitPositions[this] = m.UnitPositions[Target] + new Point();
                        var enemiesInBombRange = GetEnemiesInRange(p, m, BoomBombRange);
                        foreach (var enemy in enemiesInBombRange)
                            enemy.GetDamage(damage);
                        return true;
                    }
                    return false;
                },
                CoolDown = BoomCooldown,
                EnergyCost = BoomEnergyCost,
            };
            Boom.SkillTypes.Add(SkillType.Special);
            Skills.Add(Boom);
        }
    }
}
