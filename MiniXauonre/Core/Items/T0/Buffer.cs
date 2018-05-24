using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Items
{
    class Buffer : Item
    {
        public Buffer()
        {
            Name = "Buffer";
            Cost = 1750;

            E = 750;
            HP = 550;
            HR = 17;
            R = 8;
            AP = 100;
            ER = 20;


            Effect = new Perk
            {
                GetDamage = (f) => (d) =>
                {
                    var gotten = d.DamageValue.Sum() * 0.1;
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
                        if(map.UnitPositions[ally].GetStepsTo(map.UnitPositions[me]) <= 9)
                        {
                            ally.GetHeal((me.GetArmor() + me.GetResist() + me.GetAbilityPower()) * 0.3);
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
