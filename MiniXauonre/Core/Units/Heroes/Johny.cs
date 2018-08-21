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
        public const double BoomBombRange = 6;
        public const double BoomCooldown = 5;
        public const double BoomEnergyCost = 300;
        public const double BoomDamage = 50;
        public const double BoomDamageAPScale = 2.1;

        public Johny()
        {
            Name = "Johny";
            Image = Graphics.resources.Res.Johny;
            
            SetMaxHp(1200);
            SetMaxEnergy(300);
            SetAttackDamage(65);
            SetRegen(15);
            SetEnergyRegen(8);

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
                        var point = ChoosePoint(h.M.UnitPositions[h].GetPointsInDistance(0, BoomJumpRange, (p) => h.M.IsInBounds(p))
                            .Keys
                            .Where(c => h.M.CellIsFree(c)).
                            ToList(), h.P);
                        if (point == null) return false;
                        var targets = h.M.GetHeroes().Where(eh => eh.P != h.P && eh.GetPosition().GetStepsTo(point) <= BoomBombRange).ToList();
                        h.Targets = targets;
                        if (h.Targets.Count == 0) return false;
                        var damage = new Damage(h, h.P, pure: BoomDamage + BoomDamageAPScale * h.GetAbilityPower());
                        h.M.UnitPositions[h] = point;
                        foreach (var enemy in h.Targets)
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
