using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniXauonre.Core.Heroes;

namespace MiniXauonre.Core.Perks
{
    class TestPerk : Perk
    {
        public TestPerk()
        {
            GetMaxHp = (g) => () => g() * 2;
            SetMaxHp = (s) => (v) => s(v / 2);
            GetHp = (g) => () => g() * 2;
            SetHp = (s) => (v) => s(v / 2);
            GetAttackSpeed = (g) => () => g() + 2;
            SetAttackSpeed = (s) => (v) => s(v - 2);
            GetMovementSpeed = (g) => () => g() * 1.2;
            SetMovementSpeed = (s) => (v) => s(v / 1.2);

            GetDamage = (fn) => (d) =>
            {
                d.DamageValue.Magic *= 0.5;
                var res = fn(d);
                res.DamageValue.Magic /= 0.5;
                return res;
            };
        }
    }
}
