using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Heroes
{
/*
    Banker
- Interest Rate: Все союзники в радиусе 10 восстонавливают(5 + 2%AP)

        недостающего запаса здоровья,
	 а при повышении уровня получают(100%AP) MaxHp.
- Investment: Передает выбранному союзнику(5 + 150%AP) Money.
    CD 5. Cost 100.
- Currency Exchange: Получает чистый урон равный(10 + 20%AP)%
	 MaxHp всех противников и получает за это опыт.CD 7. Cost 200.

Hp = 950
Armor = 15
Resist = 20
Energy = 300
ER = 15
MS = 10*/

    class Banker : HeroWithBaseSkills
    {
        public const double InteresRateHeal = 0.05;
        public const double InterestRateHealAPScale = 0.0002;
        public const double InterestRateHealRange = 10;
        public const double InterestRateHpBuffAPScale = 0.5;
        public Perk InterestRate { get; set; }

        public const double InvestmentMoney = 10;
        public const double InvestmentAPScale = 2;
        public const double InvestmentCD = 5;
        public const double InvestmentCost = 150;
        public Skill Investment { get; set; }

        public const double ExchangeHP = 0.1;
        public const double ExchangeAPScale = 0.002;
        public const double ExchangeCD = 7;
        public const double ExchangeCost = 200;
        public const double ExchangeExpCoeff = 1.5;
        public Skill CurrencyExchange { get; set; }

        public Banker()
        {
            Name = "Banker";
            Image = Graphics.resources.Res.Banker;
            SetMaxHp(950);
            SetArmor(15);
            SetResist(20);
            SetMaxEnergy(300);
            SetEnergyRegen(15);
            SetMovementSpeed(10);


            InterestRate = new Perk
            {
                Name = "Interest Rate",
                Number = (h) => ((InteresRateHeal + InterestRateHealAPScale * h.GetAbilityPower())*100),
                Explanation = (h) => "When level up, gives all allies " + InterestRateHpBuffAPScale*100 + "% AP (" + 
                h.GetAbilityPower()*InterestRateHpBuffAPScale + ") HP; At the end of turn, heals allies in " +
                InterestRateHealRange + " range for " + InteresRateHeal*100 + " + " + InterestRateHealAPScale*100 +
                "% AP (" + (InteresRateHeal + InterestRateHealAPScale * h.GetAbilityPower()) + ")% target's lost HP",
                LevelUp = (a) => (d) =>
                {
                    var h = d.HeroValue;
                    var hpBuff = GetAbilityPower() * InterestRateHpBuffAPScale;
                    var allies = h.P.Heroes.Where(hh => hh != h);
                    foreach (var ally in allies)
                    {
                        ally.AddMaxHp(hpBuff);
                        ally.AddHp(hpBuff);
                    }
                    return a(d);
                },

                EndTurn = (a) => (d) =>
                {
                    var h = d.HeroValue;
                    var targets = GetAlliesInRange(h, InterestRateHealRange);
                            //heal for % of lost HP
                            var percent = h.GetAbilityPower() * InterestRateHealAPScale + InteresRateHeal;
                    foreach (var ally in targets)
                        ally.GetHeal((ally.GetMaxHp() - ally.GetHp()) * percent);
                    return a(d);
                },
            };
            Perks.Add(InterestRate);



            Investment = new Skill
            {
                Name = "Investment",
                Explanation = () => "Give chosen ally " + InvestmentMoney + " + " + InvestmentAPScale * 100 + "%AP("+
                + (InvestmentMoney + InvestmentAPScale * GetAbilityPower()) + ") of your Money.CD " + InvestmentCD + ". Cost "
                + InvestmentCost + ".",
                CoolDown = InvestmentCD,
                EnergyCost = InvestmentCost,
                Job = (h) =>
                {
                    var moneySent = Math.Min(InvestmentMoney + h.GetAbilityPower() * InvestmentAPScale, h.GetMoney());
                    var allies = h.P.Heroes.Where(hh => hh != h).ToList();
                    if (allies.Count == 0)
                        return false;
                    var target = ChooseTarget(allies, h.P);
                    if (target == null) return false;
                    h.Targets.Add(target);
                    foreach(var t in h.Targets)
                        t.AddMoney(moneySent);
                    h.AddMoney(-moneySent);
                    return true;
                },
            };
            Investment.SkillTypes.Add(SkillType.Special);
            Skills.Add(Investment);


            CurrencyExchange = new Skill
            {
                Name = "Exchange",
                Explanation = () => "Damage yourself by (" + ExchangeHP + " + " + ExchangeAPScale * 100 + "% AP)% of MaxHp ("
                + (ExchangeHP + ExchangeAPScale * GetAbilityPower())*GetMaxHp() + "). Earns " + ExchangeExpCoeff*100 + "% ("
                + (ExchangeHP + ExchangeAPScale * GetAbilityPower()) * ExchangeExpCoeff * GetMaxHp() + ") of it as EXP. CD " + ExchangeCD
                + ". Cost "+ ExchangeCost + ".",
                CoolDown = ExchangeCD,
                EnergyCost = ExchangeCost,
                Job = (h) =>
                {
                    var hpCost = h.GetMaxHp() * (ExchangeHP + ExchangeAPScale * h.GetAbilityPower());
                    if (hpCost >= h.GetHp())
                        hpCost = h.GetHp() - 1;
                    h.AddHp(-hpCost);
                    h.P.NotifyAboutDamage(new Damage(h, h.P, 0, 0, hpCost * ExchangeExpCoeff));
                    return true;
                },
            };
            CurrencyExchange.SkillTypes.Add(SkillType.Special);
            Skills.Add(CurrencyExchange);
        }
    }
}
