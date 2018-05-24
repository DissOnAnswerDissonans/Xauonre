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
        public const double BoomCooldown = 7;
        public const double BoomEnergyCost = 100;
        public const double BoomDamage = 100;
        public const double BoomDamageAPScale = 1.5;

        public Johny()
        {
            Name = "Johny";
            Image = Graphics.resources.Res.Johny;
            
            SetMaxHp(1200);
            SetMaxEnergy(150);
            SetRegen(15);
            SetEnergyRegen(5);

            Boom = new Skill
            {
                Name = "Here's Johny",
                Explanation = () => "Jumps on enemy in " + BoomJumpRange + " units from you and deales " +
                    + BoomDamage + " + " + BoomDamageAPScale * 100 + "% AP (" 
                    + (BoomDamage + BoomDamageAPScale * GetAbilityPower()) + ") Pure damage to all enemies in "
                    + BoomBombRange + " units around you. Energy cost " + BoomEnergyCost + ". Cooldown " + BoomCooldown + ".",
                Job = (h) =>
                {
                    var enemiesInRange = GetEnemiesInRange(h, BoomJumpRange);
                    if (enemiesInRange.Count != 0)
                    {
                        var target = ChooseTarget(enemiesInRange, h.P);
                        if (target == null) return false;
                        h.Targets.Add(target);
                        if (h.Targets.Count == 0) return false;
                        var damage = new Damage(h, h.P, pure: BoomDamage + BoomDamageAPScale * h.GetAbilityPower());
                        h.M.UnitPositions[h] = h.M.UnitPositions[h.Targets[0]] + new Point();
                        var enemiesInBombRange = GetEnemiesInRange(h, BoomBombRange);
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
