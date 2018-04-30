﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xauonre.Core;

namespace MiniXauonre.Core.Heroes
{
    class Anthrax : HeroWithBaseSkills
    {
        public Skill Boom { get; set; }
        public const double BoomJumpRange = 9;
        public const double BoomBombRange = 5;
        public const double BoomCooldown = 10;
        public const double BoomEnergyCost = 100;
        public const double BoomDamage = 100;
        public const double BoomDamageAPScale = 1.5;

        public Anthrax()
        {
            Name = "Anthrax";
            SetMaxHp(1200);
            SetMaxEnergy(150);
            SetRegen(5);
            SetEnergyRegen(5);

            Boom = new Skill
            {
                Name = "Boom",
                Explanation = "Jumps on enemy and does boom",
                Job = (m, p, h) =>
                {
                    var enemiesInRange = GetEnemiesInRange(p, m, BoomJumpRange);
                    if (enemiesInRange.Count != 0)
                    {
                        Target = ChooseTarget(enemiesInRange, p);
                        var damage = new Damage(pure: BoomDamage + BoomDamageAPScale * GetAbilityPower());
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
            Skills.Add(Boom);
        }
    }
}
