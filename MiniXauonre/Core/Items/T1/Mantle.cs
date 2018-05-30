using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Items
{
    class Mantle : Item
    {

        public const double APScale = 0.1;
        public const double Range = 5;
        public const double CDReduce = 1;
        public const double CDLeft = 2;


        public Mantle()
        {
            Name = "Mantle";
            Tier = 1;
            Cost = 300;
            ER = 10;
            AP = 15;

            Explanation = (h) => "After you use any skill with cooldown you deal " + APScale * 100 + "% AP(" + APScale * h.GetAbilityPower()
            + ") Spell damage to all enemies in " + Range + " range. For each enemy damaged cooldown of this skill reduces by " + CDReduce
            + " until " + CDLeft + " left.";


            Effect = new Perk
            {
                Name = this.Name,
                Explanation = this.Explanation,
                
                SkillFix = (s) =>
                {
                    if (s.CoolDown < 1)
                    {
                        return s;
                    }
                    var newSkill = new Skill()
                    {
                        Name = s.Name,
                        CoolDown = s.CoolDown,
                        SkillTypes = s.SkillTypes,
                        Explanation = s.Explanation,
                    };
                    newSkill.Job = (h) =>
                    {
                        var res = s.Job(h);
                        if (res)
                        {
                            var targets =  h.M.GetHeroes().Where(eh => 
                                eh.P != h.P && h.M.UnitPositions[eh].GetDistanceTo(h.M.UnitPositions[h]) <= Range);
                            var damage = new Damage(h, h.P, magic: (h.GetAbilityPower() * APScale));
                            foreach (var tg in targets)
                                tg.GetDamage(damage);
                            var count = targets.Count();
                            s.Timer = Math.Max(CDLeft, newSkill.CoolDown - h.GetCDReduction() - count * CDReduce);
                            return true;
                        }
                        return false;
                    };
                    return newSkill;
                }
            };
            
        }
    }
}
