using System;
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
                Explanation = Attack.Explanation + ", steal " + Steal + " Hp forever",
                Job = (m, p, h) =>
                {
                    var st = Steal + GetAbilityPower() * APScale + GetAttackPower() * ADScale;
                    if (Attack.Job(m, p, h))
                    {
                        Target.AddMaxHp(-st);
                        Target.AddHp(-st);
                        AddMaxHp(st);
                        AddHp(st);
                        return true;
                    }
                    return false;
                }
            };
            Skills.Add(BuffedAttack);
        }
    }
}
