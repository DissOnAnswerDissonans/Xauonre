using MiniXauonre.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Heroes
{
    class Hero
    {
        public List<Skill> Skills { get; protected set; }
        public List<Perk> Perks { get; protected set; }
        public string Name { get; protected set; }

        private double maxHp;
        private double hp;
        private double armor;
        private double resist;
        private double regen;
        private double money;

        private double abilityPower;
        private double attackPower;
        private double attackSpeed;
        private double attackRange;
        private double movementSpeed;

        private double maxEnergy;
        private double energy;
        private double energyRegen;

        public int AttacksLeft { get; set; }
        public double MovementLeft { get; set; }

        public Hero()
        {
            Name = "Hero";
            Perks = new List<Perk>();
            Skills = new List<Skill>();

            //Default Stats
            maxHp = 1000;
            hp = maxHp;
            armor = 0;
            resist = 0;
            regen = 3;

            money = 0;
            abilityPower = 0;
            attackPower = 50;
            attackSpeed = 1;
            attackRange = 3;
            movementSpeed = 9;
            maxEnergy = 0;
            energy = maxEnergy;
            energyRegen = 0;


        }


        ///Gets Sets
        enum Chars
        {
            MaxHp,
            Hp,
            Armor,
            Resist,
            Regen,
            Money,
            AbilityPower,
            AttackPower,
            AttackSpeed,
            AttackRange,
            MovementSpeed,
            Energy,
            MaxEnergy,
            EnergyRegen,
            //TODO: Add CDR and put into Refresh
        }

        public void UseSkill(int id, Map map, Player player)
        {
            if(id > 0 && id < Skills.Count)
            {
                var skill = Skills[id];

                skill.Work(map, player, this);
            }
        }







        public double GetMaxHp() => GetWithPerks(Chars.MaxHp);
        public void SetMaxHp(double v) => SetWithPerks(Chars.MaxHp, v);
        public void AddMaxHp(double v) => SetMaxHp(GetMaxHp() + v);

        public double GetHp() => GetWithPerks(Chars.Hp);
        public void SetHp(double v) => SetWithPerks(Chars.Hp, v);
        public void AddHp(double v) => SetHp(GetHp() + v);
        private void FSetHp(double v)
        {
            hp = v;
            var tMaxHp = GetMaxHp();
            if (GetHp() > tMaxHp)
                SetHp(tMaxHp);
        }

        public double GetArmor() => GetWithPerks(Chars.Armor);
        public void SetArmor(double v) => SetWithPerks(Chars.Armor, v);
        public void AddArmor(double v) => SetArmor(GetArmor() + v);

        public double GetResist() => GetWithPerks(Chars.Resist);
        public void SetResist(double v) => SetWithPerks(Chars.Resist, v);
        public void AddResist(double v) => SetResist(GetResist() + v);

        public double GetRegen() => GetWithPerks(Chars.Regen);
        public void SetRegen(double v) => SetWithPerks(Chars.Regen, v);
        public void AddRegen(double v) => SetRegen(GetRegen() + v);

        public double GetMoney() => GetWithPerks(Chars.Money);
        public void SetMoney(double v) => SetWithPerks(Chars.Money, v);
        public void AddMoney(double v) => SetMoney(GetMoney() + v);

        public double GetAbilityPower() => GetWithPerks(Chars.AbilityPower);
        public void SetAbilityPower(double v) => SetWithPerks(Chars.AbilityPower, v);
        public void AddAbilityPower(double v) => SetAbilityPower(GetAbilityPower() + v);

        public double GetAttackPower() => GetWithPerks(Chars.AttackPower);
        public void SetAttackPower(double v) => SetWithPerks(Chars.AttackPower, v);
        public void AddAttackPower(double v) => SetAttackPower(GetAttackPower() + v);

        public double GetAttackSpeed() => GetWithPerks(Chars.AttackSpeed);
        public void SetAttackSpeed(double v) => SetWithPerks(Chars.AttackSpeed, v);
        public void AddAttackSpeed(double v) => SetAttackSpeed(GetAttackSpeed() + v);

        public double GetAttackRange() => GetWithPerks(Chars.AttackRange);
        public void SetAttackRange(double v) => SetWithPerks(Chars.AttackRange, v);
        public void AddAttackRange(double v) => SetAttackRange(GetAttackRange() + v);

        public double GetMovementSpeed() => GetWithPerks(Chars.MovementSpeed);
        public void SetMovementSpeed(double v) => SetWithPerks(Chars.MovementSpeed, v);
        public void AddMovementSpeed(double v) => SetMovementSpeed(GetMovementSpeed() + v);

        public double GetEnergy() => GetWithPerks(Chars.Energy);
        public void SetEnergy(double v) => SetWithPerks(Chars.Energy, v);
        public void AddEnergy(double v) => SetEnergy(GetEnergy() + v);

        public double GetMaxEnergy() => GetWithPerks(Chars.MaxEnergy);
        public void SetMaxEnergy(double v) => SetWithPerks(Chars.MaxEnergy, v);
        public void AddMaxEnergy(double v) => SetMaxEnergy(GetMaxEnergy() + v);

        public double GetEnergyRegen() => GetWithPerks(Chars.EnergyRegen);
        public void SetEnergyRegen(double v) => SetWithPerks(Chars.EnergyRegen, v);
        public void AddEnergyRegen(double v) => SetEnergyRegen(GetEnergyRegen() + v);


        //TODO: add money, abilityPower, attackPower
        private double GetWithPerks(Chars ch)
        {
            Func<double> tempFunc;
            switch (ch)
            {
                case Chars.MaxHp:
                    tempFunc = () => maxHp;
                    foreach (var perk in Perks)
                        tempFunc = perk.GetMaxHp(tempFunc);
                    return tempFunc();
                case Chars.Hp:
                    tempFunc = () => hp;
                    foreach (var perk in Perks)
                        tempFunc = perk.GetHp(tempFunc);
                    return tempFunc();
                case Chars.Armor:
                    tempFunc = () => armor;
                    foreach (var perk in Perks)
                        tempFunc = perk.GetArmor(tempFunc);
                    return tempFunc();
                case Chars.Resist:
                    tempFunc = () => resist;
                    foreach (var perk in Perks)
                        tempFunc = perk.GetResist(tempFunc);
                    return tempFunc();
                case Chars.Regen:
                    tempFunc = () => regen;
                    foreach (var perk in Perks)
                        tempFunc = perk.GetRegen(tempFunc);
                    return tempFunc();
                case Chars.Money:
                    tempFunc = () => money;
                    foreach (var perk in Perks)
                        tempFunc = perk.GetMoney(tempFunc);
                    return tempFunc();
                case Chars.AbilityPower:
                    tempFunc = () => abilityPower;
                    foreach (var perk in Perks)
                        tempFunc = perk.GetAbilityPower(tempFunc);
                    return tempFunc();
                case Chars.AttackPower:
                    tempFunc = () => attackPower;
                    foreach (var perk in Perks)
                        tempFunc = perk.GetAttackPower(tempFunc);
                    return tempFunc();
                case Chars.AttackRange:
                    tempFunc = () => attackRange;
                    foreach (var perk in Perks)
                        tempFunc = perk.GetAttackRange(tempFunc);
                    return tempFunc();
                case Chars.AttackSpeed:
                    tempFunc = () => attackSpeed;
                    foreach (var perk in Perks)
                        tempFunc = perk.GetAttackSpeed(tempFunc);
                    return tempFunc();
                case Chars.MovementSpeed:
                    tempFunc = () => movementSpeed;
                    foreach (var perk in Perks)
                        tempFunc = perk.GetMovementSpeed(tempFunc);
                    return tempFunc();
                case Chars.Energy:
                    tempFunc = () => energy;
                    foreach (var perk in Perks)
                        tempFunc = perk.GetEnergy(tempFunc);
                    return tempFunc();
                case Chars.MaxEnergy:
                    tempFunc = () => maxEnergy;
                    foreach (var perk in Perks)
                        tempFunc = perk.GetMaxEnergy(tempFunc);
                    return tempFunc();
                case Chars.EnergyRegen:
                    tempFunc = () => energyRegen;
                    foreach (var perk in Perks)
                        tempFunc = perk.GetEnergyRegen(tempFunc);
                    return tempFunc();
            }
            return 0;
        }

        private void SetWithPerks(Chars ch, double v)
        {
            Action<double> tempAction;
            switch (ch)
            {
                case Chars.MaxHp:
                    tempAction = (s) => maxHp = s;
                    foreach (var perk in Perks)
                        tempAction = perk.SetMaxHp(tempAction);
                    break;
                case Chars.Hp:
                    tempAction = (s) => FSetHp(s);
                    foreach (var perk in Perks)
                        tempAction = perk.SetHp(tempAction);
                    break;
                case Chars.Armor:
                    tempAction = (s) => armor = s;
                    foreach (var perk in Perks)
                        tempAction = perk.SetArmor(tempAction);
                    break;
                case Chars.Resist:
                    tempAction = (s) => resist = s;
                    foreach (var perk in Perks)
                        tempAction = perk.SetResist(tempAction);
                    break;
                case Chars.Regen:
                    tempAction = (s) => regen = s;
                    foreach (var perk in Perks)
                        tempAction = perk.SetRegen(tempAction);
                    break;
                case Chars.AbilityPower:
                    tempAction = (s) => abilityPower = s;
                    foreach (var perk in Perks)
                        tempAction = perk.SetAbilityPower(tempAction);
                    break;
                case Chars.AttackPower:
                    tempAction = (s) => attackPower = s;
                    foreach (var perk in Perks)
                        tempAction = perk.SetAttackPower(tempAction);
                    break;
                case Chars.AttackSpeed:
                    tempAction = (s) => attackSpeed = s;
                    foreach (var perk in Perks)
                        tempAction = perk.SetAttackSpeed(tempAction);
                    break;
                case Chars.MovementSpeed:
                    tempAction = (s) => movementSpeed = s;
                    foreach (var perk in Perks)
                        tempAction = perk.SetMovementSpeed(tempAction);
                    break;
                case Chars.AttackRange:
                    tempAction = (s) => attackRange = s;
                    foreach (var perk in Perks)
                        tempAction = perk.SetAttackRange(tempAction);
                    break;
                case Chars.Energy:
                    tempAction = (s) => energy = s;
                    foreach (var perk in Perks)
                        tempAction = perk.SetEnergy(tempAction);
                    break;
                case Chars.MaxEnergy:
                    tempAction = (s) => maxEnergy = s;
                    foreach (var perk in Perks)
                        tempAction = perk.SetMaxEnergy(tempAction);
                    break;
                case Chars.EnergyRegen:
                    tempAction = (s) => energyRegen = s;
                    foreach (var perk in Perks)
                        tempAction = perk.SetEnergyRegen(tempAction);
                    break;
                default:
                    tempAction = (s) => { };
                    break;
            }
            tempAction(v);
        }



        //Funcs
        enum Actions
        {
            GetDamage,
            GetHeal,
            NextTurn,
            Init,
        }

        public void Init() => DoWithPerks(Actions.Init, new FuncData());

        private FuncData FInit(FuncData data)
        {
            SetHp(GetMaxHp());
            SetEnergy(GetMaxEnergy());
            return data;
        }

        public void NextTurn(Map m, Player p) => DoWithPerks(Actions.NextTurn, new FuncData(mapvalue: m, playerValue: p));

        private FuncData FNextTurn(FuncData data)
        {
            Regenerate();
            RefreshAttacks();
            RefreshMovement();
            foreach (var skill in Skills)
                skill.Tick(1);
            return data;
        }

        public void RefreshMovement() => MovementLeft = GetMovementSpeed();

        public void Regenerate() => GetHeal(GetRegen());

        public void RefreshAttacks() => AttacksLeft = (int)GetAttackSpeed();

        public void GetDamage(Damage damage) => DoWithPerks(Actions.GetDamage, new FuncData(dmgV: damage));

        private FuncData FGetDamage(FuncData damage)
        {
            var arm = GetArmor();
            var res = GetResist();
            var resDamage = new Damage(damage.DamageValue.Phys > arm ? damage.DamageValue.Phys - arm : 0,
                damage.DamageValue.Magic > res ? damage.DamageValue.Magic - res : 0,
                damage.DamageValue.Pure);
            AddHp(-resDamage.Sum());
            return new FuncData(dmgV: resDamage);
        }


        public void GetHeal(double heal) => DoWithPerks(Actions.GetHeal, new FuncData(dV: heal));

        private FuncData FGetHeal(FuncData heal)
        {
            AddHp(heal.DoubleValue);
            return heal;
        }
    



        private FuncData DoWithPerks(Actions act, FuncData data)
        {
            switch (act)
            {
                case Actions.GetDamage:
                    Func<FuncData, FuncData> getDamage = FGetDamage;
                    foreach (var perk in Perks)
                        getDamage = perk.GetDamage(getDamage);
                    return getDamage(data);
                case Actions.GetHeal:
                    Func<FuncData, FuncData> getHeal = FGetHeal;
                    foreach (var perk in Perks)
                        getHeal = perk.GetHeal(getHeal);
                    return getHeal(data);
                case Actions.NextTurn:
                    Func<FuncData, FuncData> nextTurn = FNextTurn;
                    foreach (var perk in Perks)
                        nextTurn = perk.NextTurn(nextTurn);
                    return nextTurn(data);
                case Actions.Init:
                    Func<FuncData, FuncData> init = FInit;
                    foreach (var perk in Perks)
                        init = perk.Init(init);
                    return init(data);
                default:
                    return new FuncData();
            }
        }



        public void FastPrintStats() => Console.WriteLine(
            Name
            + " : MaxHp-" + GetMaxHp()
            + ", Hp-" + GetHp()
            + (GetMaxEnergy() > 1 ? ", MaxEnergy-"+GetMaxEnergy() + ", Energy-"+GetEnergy() : "") 
            + ", AttackPower-" + GetAttackPower()
            + ", AbilityPower-" + GetAbilityPower()
            + ", MoveLeft-" + MovementLeft
            + ", AttacksLeft-" + AttacksLeft);

    }

}
