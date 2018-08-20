using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Items
{
    class Buffer : Item
    {
        public const double EnergyRestore = 0.1;
        public const double HealScale = 0.4;
        public const double HealRange = 9;
        public Buffer()
        {
            Name = "Buffer";
            Cost = 1900;
            Tier = 3;
            E = 750;
            HP = 550;
            HR = 34;
            R = 20;
            AP = 100;
            ER = 30;

            Explanation = (h) => "When attacked restores " + EnergyRestore * 100 + "% of damage. At the end of turn Heales all" +
            " allies in "+HealRange+" range by " + HealScale * 100 + "%(Armor + Resist + AP) ("
            + ((h.GetArmor() + h.GetResist() + h.GetAbilityPower()) * HealScale) + ").";
            Effect = new Perk
            {
                Name = this.Name,
                Explanation = this.Explanation,
                GetDamage = (f) => (d) =>
                {
                    var gotten = d.DamageValue.Sum() * EnergyRestore;
                    d.HeroValue.AddEnergy(gotten);
                    return f(d);
                },

                EndTurn = (f) => (d) =>
                {
                    var me = d.HeroValue;
                    var map = me.M;
                    var player = me.P;
                    foreach(var ally in player.Heroes)
                    {
                        if(map.UnitPositions[ally].GetStepsTo(map.UnitPositions[me]) <= HealRange)
                        {
                            ally.GetHeal((me.GetArmor() + me.GetResist() + me.GetAbilityPower()) * HealScale);
                        }
                    }
                    return f(d);
                },
            };



            //1420
            Parts = new List<Item>
            {
                new NanoArmor(),
                new MagicCrystal(),
            };
        }
    }
}
