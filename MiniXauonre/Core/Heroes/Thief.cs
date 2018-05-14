﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Heroes
{
    class Thief : HeroWithBaseSkills
    {
        private Skill BuffedAttack { get; set; }
        public const double Steal = 2;
        public const double ADScale = 0.01;
        public const double APScale = 0.01;
        public Thief()
        {
            Name = "Thief";
            SetResist(10);
            SetMovementSpeed(15);

            Skills.Remove(Attack);
            BuffedAttack = new Skill
            {
                Name = Attack.Name,
                Explanation = () => Attack.Explanation() + " Also steals " + Steal + " + "
                    + ADScale * 100 + "% AD + " + APScale * 100 + "% AP ("
                    + (Steal + APScale * GetAbilityPower() + ADScale * GetAttackPower())
                    + ") MaxHp from target forever.",
                Job = (h) =>
                {
                    var st = Steal + GetAbilityPower() * APScale + GetAttackPower() * ADScale;
                    if (Attack.Job(h))
                    {
                        Target.AddMaxHp(-st);
                        AddMaxHp(st);
                        return true;
                    }
                    return false;
                }
                
            };
            BuffedAttack.SkillTypes.Add(SkillType.Attack);
            Skills.Add(BuffedAttack);
        }
    }
}
