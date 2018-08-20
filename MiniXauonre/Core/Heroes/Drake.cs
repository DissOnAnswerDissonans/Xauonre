using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Heroes
{
    class Drake : HeroWithBaseSkills
    {
        public Perk Burn { get; set; }
        public Skill Martyr { get; set; }

        public const double HpScale = 0.05;
        public const double AoeRange = 5;
        public const double APScale = 0.1;

        private bool Burning { get; set; }
        public Drake()
        {
            Name = "Drake";
            Image = Graphics.resources.Res.Drake;
            
            SetMaxHp(1200);
            SetAttackDamage(30);
            SetArmor(15);
            SetResist(25);
            SetMovementSpeed(9);
            SetRegen(15);

            Burning = false;

            Burn = new Perk
            {
                Name = "Martyr",
                Number = (h) => Math.Floor(h.GetHp() * HpScale + h.GetAbilityPower() * APScale),
                Explanation = (h) => 
                    "Dealing " + HpScale * 100 + "% you current Hp + " + APScale + "%AP ("+
                    + (h.GetHp() * HpScale + h.GetAbilityPower() * APScale)
                    + ") magic damage to you and your enemies within " + AoeRange
                    + " units around you at the end of every your turn. (You can die)",
                EndTurn = (v) => (d) =>
                {
                    var h = d.HeroValue;
                    d = v(d);
                    if (Burning)
                    {
                        var dmg = h.GetHp() * HpScale + h.GetAbilityPower() * APScale;
                        var enemiesInRange = new List<Hero>(GetEnemiesInRange(h, AoeRange));
                        var attack = new Damage(h, h.P, magic: dmg);
                        foreach (var enemy in GetEnemiesInRange(h, AoeRange))
                            enemy.GetDamage(attack);
                        h.GetDamage(attack);
                    }
                    return d;
                } 
            };

            Martyr = new Skill
            {
                Name = "Martyr",
                Explanation = () => Burning ? "Turn off martyr." : 
                    "Turnes on dealing " + HpScale * 100 + "% you current Hp + " + APScale + "%AP ("+
                    + (GetHp() * HpScale + GetAbilityPower() * APScale)
                    + ") magic damage to you and your enemies within " + AoeRange
                    + " units around you at the end of every your turn. (You can die)",

                Job = (h) =>
                {
                    Burning = !Burning;
                    if (Burning)
                        h.Perks.Add(Burn);
                    else
                        h.Perks.Remove(Burn);
                    return true;
                }
            };
            Martyr.SkillTypes.Add(SkillType.Special);
            Skills.Add(Martyr);

        }
    }
}
