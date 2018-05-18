using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Heroes
{


    /*
     Banker
        - Interest Rate: Âñå ñîþçíèêè â ðàäèóñå 10 âîññòîíàâëèâàþò (5 + 2%AP) 
	        íåäîñòàþùåãî çàïàñà çäîðîâüÿ,
	        à ïðè ïîâûøåíèè óðîâíÿ ïîëó÷àþò (100%AP) MaxHp.
        - Investment: Ïåðåäàåò âûáðàííîìó ñîþçíèêó (5 + 10%AP) Money.
	        CD 8. Cost 30.
        - Currency Exchange: Ïîëó÷àåò ÷èñòûé óðîí ðàâíûé (10 + 10%AP)%
	        MaxHp âñåõ ïðîòèâíèêîâ è ïîëó÷àåò çà ýòî îïûò. CD 6. Cost 40.

        Hp = 950
        Armor = 15
        Resist = 20
        Energy = 100
        ER = 5
        MS = 10
        */

    class Banker : HeroWithBaseSkills
    {
        public const double InteresRateHeal = 0.05;
        public const double InterestRateHealAPScale = 0.002;
        public const double InterestRateHealRange = 10;
        public const double InterestRateHpBuffAPScale = 1;
        public Perk InterestRate { get; set; }

        public const double InvestmentMoney = 5;
        public const double InvestmentAPScale = 0.1;
        public const double InvestmentCD = 8;
        public const double InvestmentCost = 30;
        public Skill Investment { get; set; }

        public const double ExchangeHP = 0.1;
        public const double ExchangeAPScale = 0.001;
        public const double ExchangeCD = 6;
        public const double ExchangeCost = 40;
        public const double ExchangeExpCoeff = 2;
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


            InterestRate = new Perk
            {
                LevelUp = (a) => (d) =>
                {
                    var hpBuff = GetAbilityPower() * InterestRateHpBuffAPScale;
                    var allies = P.Heroes.Where(h => h != this);
                    foreach (var ally in allies)
                    {
                        ally.AddMaxHp(hpBuff);
                        ally.AddHp(hpBuff);
                    }
                    return a(d);
                },

                EndTurn = (a) => (d) =>
                {
                    var targets = GetAlliesInRange(P, M, InterestRateHealRange);
                            //heal for % of lost HP
                            var percent = GetAbilityPower() * InterestRateHealAPScale + InteresRateHeal;
                    foreach (var ally in targets)
                        ally.GetHeal((ally.GetMaxHp() - ally.GetHp()) * percent);
                    return a(d);
                },
            };
            Perks.Add(InterestRate);



            Investment = new Skill
            {
                Name = "Investment",
                Explanation = () => "Blank",
                CoolDown = InvestmentCD,
                EnergyCost = InvestmentCost,
                Job = (he) =>
                {
                    var moneySent = Math.Min(InvestmentMoney + GetAbilityPower() * InvestmentAPScale, GetMoney());
                    var allies = P.Heroes.Where(h => h != this).ToList();
                    if (allies.Count == 0)
                        return false;
                    Targets.Add(ChooseTarget(allies, P));
                    foreach(var t in Targets)AddMoney(moneySent);
                    AddMoney(-moneySent);
                    return true;
                },
            };
            Investment.SkillTypes.Add(SkillType.Special);
            Skills.Add(Investment);


            CurrencyExchange = new Skill
            {
                Name = "CurrecnyExchange",
                Explanation = () => "Blank",
                CoolDown = ExchangeCD,
                EnergyCost = ExchangeCost,
                Job = (he) =>
                {
                    var hpCost = GetMaxHp() * (ExchangeHP + ExchangeAPScale * GetAbilityPower());
                    AddHp(-hpCost);
                    P.AllDamage += hpCost * ExchangeExpCoeff;
                    return true;
                },
            };
            CurrencyExchange.SkillTypes.Add(SkillType.Special);
            Skills.Add(CurrencyExchange);
        }
    }
}
