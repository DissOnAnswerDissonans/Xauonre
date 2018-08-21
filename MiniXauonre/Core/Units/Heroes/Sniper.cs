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
        public const double SnipeRange = 25;
        public const double SnipeDamage = 50;
        public const double SnipeApScale = 1.25;
        public const double SnipeCooldown = 5;
        public const double SnipeEnergyCost = 100;
        public const int SnipeRootTime = 1;

        public Sniper()
        {
            Name = "Sniper";
            Image = Graphics.resources.Res.Sniper;
            
            SetMaxHp(800);
            SetAttackDamage(60);
            SetAbilityPower(20);
            SetAttackRange(13);
            SetRegen(7);
            SetMaxEnergy(200);
            SetEnergyRegen(10);
            SetMovementSpeed(9);
            

            Snipe = new Skill
            {
                Name = "Snipe",
                Explanation = () => "Deals to enemy " + SnipeDamage 
                    + " + " + SnipeApScale * 100 + "% AP (" 
                    + (SnipeDamage + SnipeApScale * GetAbilityPower()) 
                    + ") physycal damage and root for " + SnipeRootTime + "turn. Range " + SnipeRange + 
                    ". Energy cost " + SnipeEnergyCost + ". Cooldown " + SnipeCooldown + ".",
                Job = (h) =>
                {
                    var enemiesInRange = GetEnemiesInRange(h, SnipeRange);
                    if (enemiesInRange.Count != 0)
                    {
                        var tg = ChooseTarget(enemiesInRange, h.P);
                        if (tg == null) return false;
                        h.Targets.Add(tg);
                        var damage = new Damage(h, h.P, phys: SnipeDamage + SnipeApScale * h.GetAbilityPower());
                        foreach(var t in h.Targets)t.GetDamage(damage);

                        var enemyMoveSkills = h.Targets[0].Skills.Where(s => s.SkillTypes.Contains(SkillType.Move)).ToList();
                        var target = h.Targets[0];

                        var perk = new Perk();
                        perk.Name = "Rooted";
                        
                        var root = new Effect(target)
                        {
                            Timer = SnipeRootTime - 1,
                            Activate = (eh) => 
                                {
                                    eh.Perks.Add(perk);    
                                    foreach (var sk in enemyMoveSkills)
                                        eh.Skills.Remove(sk);
                                },
                            Disactivate = (eh) =>
                                {
                                    eh.Perks.Remove(perk);
                                    foreach(var sk in enemyMoveSkills)
                                        eh.Skills.Add(sk);
                                },
                        };

                        perk.Number = (eh) => root.Timer + 1;                      

                        h.M.Effects.Add(root);
                        root.Activate(target);
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
