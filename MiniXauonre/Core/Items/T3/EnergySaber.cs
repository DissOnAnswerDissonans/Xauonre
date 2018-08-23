using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Items
{
    class EnergySaber : Item
    {
        public double Steal = 100;
        public EnergySaber()
        {
            Name = "Energy Saber";
            Tier = 3;
            Cost = 2500;

            HP = 500;
            AS = 2;
            AD = 80;
            E = 260;

            Explanation = (h) => "Your attacks Steal " + Steal + " energy from all targets.";

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
                                foreach (var t in h.Targets)
                                {
                                    var was = t.GetEnergy();
                                    if (was < Steal)
                                    {
                                        t.AddEnergy(-was);
                                        h.AddEnergy(was);
                                    }
                                    else
                                    {
                                        t.AddEnergy(-Steal);
                                        h.AddEnergy(Steal);
                                    }
                                }
                                return true;
                                
                            }
                            return false;
                        },
                        Explanation = () => s.Explanation() + " + Energy STTEAALL",
                    };

                    return newSkill;

                }

            };

            //1644
            Parts = new List<Item>()
            {
                new Leaven(),
                new LongSword(),
                new EnergeticClaws(),
                new Booster(),
            };
        }
    }
}
