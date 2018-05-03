using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Heroes
{
    class Banker : HeroWithBaseSkills
    {
        public const double InteresRateHeal = 0.05;
        public const double InterestRateHealAPScale = 0.02;
        public const double InterestRateHealRange = 10;
        public const double InterestRateHpBuffAPScale = 1;
        public Effect InterestRate { get; set; }

        public const double InvestmentMoney = 5;
        public const double InvestmentAPScale = 0.1;
        public const double InvestmentCD = 8;
        public const double InvestmentCost = 30;
        public Skill Investment { get; set; }

        public const double ExchangeHP = 0.1;
        public const double ExchangeAPScale = 0.1;
        public const double ExchangeCD = 6;
        public const double ExchangeCost = 40;
        public Skill CurrencyExchange { get; set; }

        public Banker()
        {
            Name = "Banker";
            SetMaxHp(950);
            SetArmor(15);
            SetResist(20);
            SetMaxEnergy(100);
            SetEnergyRegen(5);
            SetMovementSpeed(10);


            InterestRate = new Effect(this, int.MaxValue)
            {
                Activate = (he) =>
                {
                    var perk = new Perk
                    {

                    };
                },
            };
            M.Effects.Add(InterestRate);
            InterestRate.Activate(this);
        }
    }
}
