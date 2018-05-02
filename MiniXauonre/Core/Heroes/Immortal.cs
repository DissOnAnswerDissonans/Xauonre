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

        public const int StormDuration = 3;

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
        }
    }
}
