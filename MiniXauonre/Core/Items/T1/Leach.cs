using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Items
{
    class Leach : Item
    {
        public Leach()
        {
            Name = "Leach";

            Cost = 750;
            AD = 50;
            A = 5;
            HP = 200;


            Effect = new Perk()
            {
                SkillFix = (s) =>
                {
                    if (!s.SkillTypes.Contains(SkillType.Attack))
                    {
                        return s;
                    }
                    var newSkill = new Skill()
                    {
                        Name = s.Name,
                        CoolDown = s.CoolDown,
                        SkillTypes = s.SkillTypes,
                        Job = (h) =>
                        {
                            var res = s.Job(h);
                            if (res)
                            {
                                h.GetHeal(h.GetAttackDamage() * 0.5 * h.Targets.Count);
                                return true;
                            }
                            return false;
                        },
                        Explanation = () => s.Explanation() + " + LIIIFFESTTEAALL",
                    };

                    return newSkill;

                }

            };


            //440
            Parts = new List<Item>
            {
                new XPeke(),
                new Razor(),
                new Razor(),
                new Razor(),
            };
        }
    }
}
