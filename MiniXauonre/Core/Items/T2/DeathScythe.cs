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
        public const double APScale = 0.5;
        public const int DamageDelay = 2;

        public DeathScythe()
        {
            Name = "Death Scythe";
            Cost = 777;
            Tier = 2;
            HP = 100;
            A = 5;
            AD = 45;
            HR = 8;

            Explanation = (h) => "Every attack deales bonus " + APScale*100 + "% AP("+h.GetAbilityPower()*APScale+") magic damage after " + DamageDelay + " turns.";

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
                                var tgts = h.Targets;
                                var effect = new Effect(h)
                                {
                                    Activate = (he) => { },
                                    Disactivate = (he) =>
                                    {
                                        var damage = new Damage(he, he.P, magic: h.GetAbilityPower() * APScale);
                                        foreach (var t in tgts) t.GetDamage(damage);
                                    },
                                    Timer = DamageDelay,
                                };
                                h.M.Effects.Add(effect);
                                effect.Activate(h);
                                return true;
                            }
                            return false;
                        },
                        Explanation = s.Explanation,
                    };

                    return newSkill;

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