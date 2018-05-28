using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Heroes
{
    class Immortal : HeroWithBaseSkills
    {
        public const double ShieldMaxDamage = 40;
        public const double ShieldEnergyRegen = 0.02;
        public const double ShieldEnergyExpCoeff = 0.5;
        public Perk Shield { get; set; }

        public const int StormDuration = 2;
        public const double StormRange = 7;
        public const double StormMaxEnergyScale = 0.1;
        public const double StormAPScale = 0.5;
        public const double StormCost = 0.3;
        public const double StormCD = 6;
        public Skill Storm { get; set; }
        public Perk Reflection { get; set; }

        public Immortal()
        {
            Name = "Immortal";
            Image = Graphics.resources.Res.Immortal;
            SetMaxHp(500);
            SetEnergyRegen(5);
            SetMaxEnergy(500);
            SetArmor(10);
            SetResist(10);
            SetMovementSpeed(10);
            SetAttackDamage(50);
            SetAttackRange(8);


            Shield = new Perk
            {
                Name = "Shield",
                Explanation = (h) => "When taking damage up to " + ShieldMaxDamage + " of it goes in to HP, others goes energy;" + 
                " And in end of turn restore " + ShieldEnergyRegen*100 + "% of lost energy",
                GetDamage = (a) => (d) =>
                {
                    var h = d.HeroValue;
                    
                    var damage = d.DamageValue;

                    var arm = h.GetArmor();
                    var res = h.GetResist();
                    var armored = damage.Phys > arm ? arm : damage.Phys;
                    var resisted = damage.Magic > arm ? arm : damage.Magic;
                    var resDamage = new Damage(h, h.P, damage.Phys - armored,
                        damage.Magic - resisted,
                        damage.Pure);
                    var damageShould = resDamage.Sum();
                    if(damageShould > ShieldMaxDamage)
                    {
                        var over = damageShould - ShieldMaxDamage;
                        var energyLeft = h.GetEnergy();
                        var absorbed = energyLeft >= over ? over : energyLeft;
                        var damageLeft = over - absorbed + ShieldMaxDamage;
                        var coeff = damageLeft / damageShould;
                        resDamage.Phys *= coeff;
                        resDamage.Magic *= coeff;
                        resDamage.Pure *= coeff;
                        h.AddEnergy(-absorbed);
                        d.PlayerValue.AllDamage += absorbed * ShieldEnergyExpCoeff;
                    }
                    resDamage.Phys += armored;
                    resDamage.Magic += resisted;
                    d.DamageValue = resDamage;
                    return a(d);
                },

                EndTurn = (a) => (d) =>
                {
                    var h = d.HeroValue;
                    h.AddEnergy(ShieldEnergyRegen * (h.GetMaxEnergy() - h.GetEnergy()));
                    return a(d);
                }
            };
            Perks.Add(Shield);

            Reflection = new Perk
            {
                Name = "Reflection",
                Explanation = (h) => "Everybody who attacks you or stay within " + StormRange + 
                   " radius at the end of your turn gets" + 
                      (StormMaxEnergyScale * h.GetMaxEnergy() + StormAPScale * h.GetAbilityPower()) + " spell damage",
                GetDamage = (a) => (d) =>
                {
                    var h = d.HeroValue;
                    if (h.P.Heroes.Contains(d.DamageValue.Creator))
                        return a(d);
                    var maxEnergy = h.GetMaxEnergy();
                    var ap = h.GetAbilityPower();
                    d.DamageValue.Creator
                        .GetDamage(new Damage(h, h.P,
                        magic: maxEnergy * StormMaxEnergyScale
                        + ap * StormAPScale));
                    return a(d);
                },


                EndTurn = (a) => (d) =>
                {
                    var h = d.HeroValue;
                    var maxEnergy = h.GetMaxEnergy();
                    var ap = h.GetAbilityPower();
                    var damage = new Damage(h, h.P,
                        magic: maxEnergy * StormMaxEnergyScale
                        + ap * StormAPScale);
                    var targets = GetEnemiesInRange(h, StormRange);
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
                CoolDown = StormCD,
                Job = (h) =>
                {
                    var ef = new Effect(h, StormDuration - 1)
                    {
                        Activate = (eh) => eh.Perks.Add(Reflection),
                        Disactivate = (eh) => eh.Perks.Remove(Reflection),
                    };
                    h.M.Effects.Add(ef);
                    ef.Activate(h);
                    h.SetEnergy(h.GetEnergy() * (1 - StormCost));
                    return true;
                }
            };

            Storm.SkillTypes.Add(SkillType.Special);
            Skills.Add(Storm);
        }
    }
}
