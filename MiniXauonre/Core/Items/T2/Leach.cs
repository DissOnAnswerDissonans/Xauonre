using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Items
{
    class Leach : Item
    {

        public const double ADScale = 0.3;
        public Leach()
        {
            Name = "Leach";
            Tier = 2;
            Cost = 750;
            AD = 50;
            A = 5;
            HP = 200;

            Explanation = (h) => "Each attack heales you by " + ADScale*100 + "% AD("
            + h.GetAttackDamage() * ADScale + ").";
            Effect = new Perk()
            {
                Name = this.Name,
                Explanation = this.Explanation,
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
                                h.GetHeal(h.GetAttackDamage() * 0.3 * h.Targets.Count);
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
