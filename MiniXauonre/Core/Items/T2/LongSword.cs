using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Items
{
    class LongSword : Item
    {
        public double Border = 5;
        public double Bonus = 2;

        public LongSword()
        {
            Name = "Long Sword";
            Cost = 810;
            Tier = 2;
            AS = 1;
            AD = 30;
            E = 150;

            Explanation = (h) => "If your Attack Range not more than " + Border + ", then it increases by " + Bonus + ".";
            var worked = false;
            Effect = new Perk()
            {
                Name = this.Name,
                Explanation = this.Explanation,
                GetAttackRange = (g) =>
                {
                    var a = g();
                    if (a < Border)
                    {
                        worked = true;
                        return () => a + Bonus;
                    }
                    worked = false;
                    return () => g();
                },
                SetAttackRange = (s) =>
                {
                    if (worked)
                        return (v) => s(v - Bonus);
                    return (v) => s(v);
                },
                GetAbilityPower = (g) => () => g() + Bonus,
                SetAbilityPower = (s) => (v) => s(v - Bonus),
            };

            //694
            Parts = new List<Item>
            {
                new Booster(),
                new Blade(),
                new Battery(),
            };
        }
    }
}
