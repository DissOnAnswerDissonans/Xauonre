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
        public Perk Reflection { get; set; }

        public Immortal()
        {
            Name = "Immortal";
            SetMaxHp(500);
            SetEnergyRegen(5);
            SetMaxEnergy(1000);
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
                    var armored = damage.Phys > arm ? arm : damage.Phys;
                    var resisted = damage.Magic > arm ? arm : damage.Magic;
                    var resDamage = new Damage(this, d.PlayerValue, damage.Phys - armored,
                        damage.Magic - resisted,
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
                    resDamage.Phys += armored;
                    resDamage.Magic += resisted;
                    d.DamageValue = resDamage;
                    a(d);
                    return d;
                },

                EndTurn = (a) => (d) =>
                {
                    AddEnergy(ShieldEnergyRegen * (GetMaxEnergy() - GetEnergy()));
                    return a(d);
                }
            };
            Perks.Add(Shield);

            Reflection = new Perk
            {
                GetDamage = (a) => (d) =>
                {
                    var maxEnergy = GetMaxEnergy();
                    var ap = GetAbilityPower();
                    d.DamageValue.Creator
                        .GetDamage(new Damage(this, P,
                        magic: maxEnergy * StormMaxEnergyScale
                        + ap * StormAPScale));
                    a(d);
                    return d;
                },


                EndTurn = (a) => (d) =>
                {
                    var maxEnergy = GetMaxEnergy();
                    var ap = GetAbilityPower();
                    var damage = new Damage(this, P,
                        magic: maxEnergy * StormMaxEnergyScale
                        + ap * StormAPScale);
                    var targets = GetEnemiesInRange(P, M, StormRange);
                    foreach (var target in targets)
                        target.GetDamage(damage);
                    return a(d);
                },
            };

            Storm = new Skill
            {
                Name = "Storm",
                Explanation = () => "For next " + StormDuration
                + " turns all enemies in " + StormRange
                + " units from you and each time they do damage to" +
                " this hero they take " + StormMaxEnergyScale * 100 +
                "% MaxEnergy + " + StormAPScale * 100 + "%AP ("
                + (StormMaxEnergyScale * GetMaxEnergy() + StormAPScale * GetAbilityPower()) +
                ") magic damage. Cost " + StormCost + "%Energy. CD " +
                StormCD,
                CoolDown = 6,
                Job = (h) =>
                {
                    var ef = new Effect(h, StormDuration)
                    {
                        Activate = (eh) => eh.Perks.Add(Reflection),
                        Disactivate = (eh) => eh.Perks.Remove(Reflection),
                    };
                    h.M.Effects.Add(ef);
                    ef.Activate(this);
                    SetEnergy(GetEnergy() * (1 - StormCost));
                    return true;
                }
            };

            Storm.SkillTypes.Add(SkillType.Special);
            Skills.Add(Storm);
        }
    }
}
