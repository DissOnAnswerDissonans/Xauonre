using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Items
{
    class IdeaGenerator : Item
    {
        public double AbilityPowerBuff = 0.3;
        public IdeaGenerator()
        {
            Name = "Idea Generator";
            Cost = 1111;

            Tier = 2;
            E = 200;
            AP = 100;
            ER = 20;
            HP = 300;


            var ap = 0.0;
            Explanation = (h) =>
            {
                ap = (h.GetAbilityPower() - ap) * AbilityPowerBuff;
                return "You also get " + AbilityPowerBuff * 100 + "%(" + h.GetAbilityPower() * AbilityPowerBuff + ") of your AP. This item is very strange  bugged, so glglgl)";
            };
            Effect = new Perk
            {
                Name = this.Name,
                Explanation = this.Explanation,
                GetAbilityPower = (g) => () => g() + ap,
                SetAbilityPower = (s) => (v) => s(v - ap),
                
            };


            //500
            Parts = new List<Item>
            {
                new Accumulator(),
                new MagicRelic(),
            };
        }
    }
}
