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

        public const double HpScale = 0.02;
        public const double AoeRange = 5;
        public const double APScale = 0.1;

        private bool Burning { get; set; }
        public Drake()
        {
            Name = "Drake";
            SetMaxHp(1600);
            SetAttackPower(30);
            SetArmor(10);
            SetResist(15);
            SetMovementSpeed(10);
            SetRegen(7);

            Burning = false;

            Burn = new Perk
            {
                EndTurn = (v) => (d) =>
                {
                    if (Burning)
                    {
                        var dmg = GetHp() * HpScale + GetAbilityPower() * APScale;
                        v(d);
                        var enemiesInRange = GetEnemiesInRange(d.PlayerValue, d.MapValue, AoeRange);
                        var attack = new Damage(d.PlayerValue, magic: dmg);
                        foreach (var enemy in enemiesInRange)
                            enemy.GetDamage(attack);
                        GetDamage(attack);
                    }
                    return d;
                } 
            };
            Perks.Add(Burn);

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
                    return true;
                }
            };
            Martyr.SkillTypes.Add(SkillType.Special);
            Skills.Add(Martyr);

        }
    }
}
