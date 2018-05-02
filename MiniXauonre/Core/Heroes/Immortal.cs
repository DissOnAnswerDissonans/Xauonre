using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Heroes
{
    class Immortal : HeroWithBaseSkills
    {
        public const double ShieldMaxDamage = 20;
        public const double ShieldEnergyRegen = 0.05;
        public Perk Shield { get; set; }

        public const int StormDuration = 3;
        public const double StormRange = 7;
        public const double StormMaxEnergyScale = 0.1;
        public const double StormAPScale = 0.5;
        public const double StormCost = 0.2;
        public const double StormCD = 6;
        public Skill Storm { get; set; }

        public Immortal()
        {
            Name = "Immortal";
            SetMaxHp(500);
            SetEnergyRegen(5);
            SetEnergy(1000);
            SetArmor(10);
            SetResist(10);
            SetMovementSpeed(10);
            SetAttackPower(50);
            SetAttackRange(8);


            Shield = new Perk
            {
                GetDamage = (a) => (d) =>
                {
                    var damage = d.DamageValue;
                    var arm = GetArmor();
                    var res = GetResist();
                    var resDamage = new Damage(d.PlayerValue, damage.Phys > arm ? damage.Phys - arm : 0,
                        damage.Magic > res ? damage.Magic - res : 0,
                        damage.Pure);
                    var damageShould = resDamage.Sum();
                    if(damageShould > ShieldMaxDamage)
                    {
                        var over = damageShould - ShieldMaxDamage;
                        var energyLeft = GetEnergy();
                        var absorbed = energyLeft >= over ? over : energyLeft;
                        var damageLeft = over - absorbed + ShieldMaxDamage;
                        var coeff = damageLeft / damageShould;
                        resDamage.Phys *= coeff;
                        resDamage.Magic *= coeff;
                        resDamage.Pure *= coeff;
                        AddEnergy(-absorbed);
                    }
                    d.DamageValue = resDamage;
                    return d;
                },

                EndTurn = (a) => (d) =>
                {
                    AddEnergy(ShieldEnergyRegen * (GetMaxEnergy() - GetEnergy()));
                    return d;
                }
            };

            Storm = new Skill
            {
                Name = "Storm",
            };

            Storm.SkillTypes.Add(SkillType.Special);
        }
    }
}
