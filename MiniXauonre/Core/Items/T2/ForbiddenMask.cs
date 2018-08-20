using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Items
{
    class ForbiddenMask : Item
    {
        public const double ExpToHpCoeff = 0.5;

        public ForbiddenMask()
        {
            Name = "Forbidden Mask";
            Tier = 2;
            HP = 300;
            AP = 75;
            HR = 25;
            Cost = 920;//700
            //460
            Explanation = (h) => "When you use skill restore HP equal to " +ExpToHpCoeff*100+ "% of EXP gotten.";

            Effect = new Perk()
            {
                Name = this.Name,
                Explanation = this.Explanation,
                SkillFix = (s) =>
                {
                    var newSkill = new Skill()
                    {
                        Name = s.Name,
                        CoolDown = s.CoolDown,
                        SkillTypes = s.SkillTypes,
                        Job = (h) =>
                        {
                            var was = h.P.AllDamage;
                            var res = s.Job(h);
                            if (res)
                            {
                                var became = h.P.AllDamage;
                                h.GetHeal(Math.Max(0, became - was) * ExpToHpCoeff);
                            }
                            return res;
                        },
                        Explanation = s.Explanation,
                    };

                    return newSkill;
                }
            };

            Parts = new List<Item>
            {
                new Shield(),
                new MagicRelic(),
            };
        }
    }
}
