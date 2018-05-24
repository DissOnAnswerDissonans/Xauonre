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
        public const double ADScale = 0.03;
        public const double APScale = 0.03;
        public Thief()
        {
            Name = "Thief";
            Image = Graphics.resources.Res.Thief;
            SetResist(10);
            SetAttackDamage(80);
            SetMovementSpeed(15);

            Skills.Remove(Attack);
            BuffedAttack = new Skill
            {
                Name = Attack.Name,
                Explanation = () => Attack.Explanation() + " Also steals " + Steal + " + "
                    + ADScale * 100 + "% AD + " + APScale * 100 + "% AP ("
                    + (Steal + APScale * GetAbilityPower() + ADScale * GetAttackDamage())
                    + ") MaxHp from target forever.",
                Job = (h) =>
                {
                    var st = Steal + h.GetAbilityPower() * APScale + h.GetAttackDamage() * ADScale;
                    if (Attack.Job(h))
                    {
                        foreach(var t in Targets)
                            t.AddMaxHp(-st);
                        h.AddMaxHp(st);
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
