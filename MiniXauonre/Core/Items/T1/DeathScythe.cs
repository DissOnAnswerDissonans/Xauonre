using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Items
{
    class DeathScythe : Item
    {
        private Skill FixedAttack { get; set; }
        public const double APScale = 0.2;

        public DeathScythe()
        {
            Name = "Death Scythe";
            Cost = 777;


            HP = 50;
            A = 5;
            AD = 45;
            HR = 2.5;

            Effect = new Perk()
            {
                SkillFix = (s) =>
                {
                    if (s.SkillTypes.Contains(SkillType.Attack))
                    {
                        var prev = s.Job;
                        var attack = new Skill
                        {
                            Name = s.Name,
                            Explanation = s.Explanation,
                            EnergyCost = s.EnergyCost,
                            CoolDown = s.CoolDown,
                            Job = (h) =>
                            {
                                if (prev(h))
                                {
                                    var damage = new Damage(h, h.P, magic: h.GetAbilityPower() * APScale);
                                    foreach (var t in h.Targets) t.GetDamage(damage);
                                    return true;
                                }
                                return false;
                            },
                        };
                    }
                    return s;
                }
            };

            //300
            Parts = new List<Item> {
                new XPeke(),
                new Razor(),
            };



        }







    }
}
