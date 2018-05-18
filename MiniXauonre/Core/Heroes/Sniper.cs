﻿using System;
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
        public const int SnipeRootTime = 1;
        public Perk RootPerk { get; protected set; }

        public Sniper()
        {
            Name = "Sniper";
            Image = Graphics.resources.Res.Sniper;
            
            SetMaxHp(800);
            SetAttackDamage(60);
            SetAbilityPower(20);
            SetAttackRange(14);
            SetRegen(2);
            SetMaxEnergy(200);
            SetEnergyRegen(4);

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
                    var enemiesInRange = GetEnemiesInRange(P, M, SnipeRange);
                    if (enemiesInRange.Count != 0)
                    {
                        Targets.Add(ChooseTarget(enemiesInRange, P));
                        var damage = new Damage(this, P, phys: SnipeDamage + SnipeApScale * GetAbilityPower());
                        foreach(var t in Targets)t.GetDamage(damage);

                        var enemyMoveSkills = Targets.SelectMany(t => t.Skills.Where(s => s.SkillTypes.Contains(SkillType.Move))).ToList();
                        var target = Targets;
                        var root = new Effect(this)
                        {
                            Timer = SnipeRootTime,
                            Activate = (eh) => 
                                {
                                    foreach (var sk in enemyMoveSkills)
                                        eh.Skills.Remove(sk);
                                },
                            Disactivate = (eh) =>
                                {
                                    foreach(var sk in enemyMoveSkills)
                                        foreach(var t in target)t.Skills.Add(sk);
                                },
                        };

                        M.Effects.Add(root);
                        foreach(var t in target) root.Activate(t);
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
