using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Heroes
{
    class Drake : HeroWithBaseSkills
    {
        private Perk Burn { get; set; }
        public const double HpScale = 0.01;
        public const double AoeRange = 5;
        public const double APScale = 0.1;

        public Drake()
        {
            Name = "Drake";
            SetMaxHp(1500);
            SetAttackPower(30);
            SetArmor(10);
            SetResist(15);
            SetMovementSpeed(10);
            SetRegen(7);

            Burn = new Perk
            {
                NextTurn = (v) => (d) =>
                {
                    var dmg = GetHp() * HpScale + GetAbilityPower() * APScale;
                    v(d);
                    var enemiesInRange = GetEnemiesInRange(d.PlayerValue, d.MapValue, AoeRange);
                    var attack = new Damage(magic: dmg);
                    foreach (var enemy in enemiesInRange)
                        enemy.GetDamage(attack);
                    return d;
                } 
            };
            Perks.Add(Burn);
        }
    }
}
