using MiniXauonre.Controller;
using MiniXauonre.Core.Shops;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;


namespace MiniXauonre.Core.Heroes
{
    class Hero : Unit
    {
        public List<Skill> Skills { get; protected set; }
        public List<Perk> Perks { get; protected set; }
        public List<Item> Items { get; protected set; }
        public string Name { get; protected set; }
        

        public double MaxNumberOfItems { get; protected set; }

        public bool Chosen { get; private set; }
        public List<Hero> Targets { get; set; }

        public Func<Dictionary<string, double>> StatsGetter { get; protected set; }

        private double maxHp;
        private double hp;
        private double armor;
        private double resist;
        private double regen;
        private double money;

        private double abilityPower;
        private double attackDamage;
        private double attackSpeed;
        private double attackRange;
        private double movementSpeed;

        private double maxEnergy;
        private double energy;
        private double energyRegen;
        private double cdReduction;

        public Map M { get; set; }
        public Player P { get; set; }
        public Shop S { get; set; }

        public int Level { get; set; }

        public int AttacksLeft { get; set; }
        public double MovementLeft { get; set; }

        public Hero()
        {
            Name = "Hero";
            Perks = new List<Perk>();
            Skills = new List<Skill>();
            S = null;
            Items = new List<Item>();
            Image = Graphics.resources.Res.DefaultHero;
            Targets = new List<Hero>();
            StatsGetter = GetAllStats;
            Solid = true;
            //Default Stats
            maxHp = 1000;
            hp = maxHp;
            armor = 0;
            resist = 0;
            regen = 10;

            money = 0;
            abilityPower = 0;
            attackDamage = 50;
            attackSpeed = 1;
            attackRange = 3;
            movementSpeed = 9;
            maxEnergy = 0;
            energy = maxEnergy;
            energyRegen = 0;

            Level = 0;
            MaxNumberOfItems = 4;
        }

        public Hero(Hero hero) //  ГООООООВВНННООООООО!!!!!!!!
        {
            Name = hero.Name;
            Perks = hero.Perks;
            Skills = hero.Skills;
            S = hero.S;
            M = hero.M;
            P = hero.P;
            Items = hero.Items;
            Image = hero.Image;
            Targets = hero.Targets;

            maxHp = hero.maxHp;
            hp = hero.hp;
            armor = hero.armor;
            resist = hero.resist;
            regen = hero.regen;

            money = hero.money;
            abilityPower = hero.abilityPower;
            attackDamage = hero.attackDamage;
            attackSpeed = hero.attackSpeed;
            attackRange = hero.attackRange;
            movementSpeed = hero.movementSpeed;
            maxEnergy = hero.maxEnergy;
            energy = hero.energy;
            energyRegen = hero.energyRegen;

            Level = hero.Level;
            MaxNumberOfItems = hero.MaxNumberOfItems;
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
            AttackDamage,
            AttackSpeed,
            AttackRange,
            MovementSpeed,
            Energy,
            MaxEnergy,
            EnergyRegen,
            CDReduction,
        }

        public void UseSkill(int id)
        {
            if (id >= 0 && id < Skills.Count)
            {
                var skill = Skills[id];
                foreach (var perk in Perks)
                    skill = perk.SkillFix(skill);
                skill.Work(this);
                Skills[id].Timer = skill.Timer;
            }
        }


        public bool BuyItem(Item item)
        {
            var itemsAreParts = item.Parts.Count(p => Items.Contains(p));
            return !(Items.Count - itemsAreParts >= MaxNumberOfItems) && S.Buy(this, item);
        }

        public void Kill()
        {
            M.UnitPositions.Remove(this);
            P.Heroes.Remove(this);
        }

        public double GetMaxHp() => GetWithPerks(Chars.MaxHp);
        public void SetMaxHp(double v) => SetWithPerks(Chars.MaxHp, v);
        public void AddMaxHp(double v) { SetMaxHp(GetMaxHp() + v); AddHp(Math.Max(v, 0)); }

        public double GetHp() => GetWithPerks(Chars.Hp);
        public void SetHp(double v) => SetWithPerks(Chars.Hp, v);
        public void AddHp(double v) => SetHp(GetHp() + v);
        private void FSetHp(double v)
        {
            hp = v;
            var tMaxHp = GetMaxHp();
            if (GetHp() > tMaxHp)
                SetHp(tMaxHp);
            if (P != null)
                P.Game.GameCheck();
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

        public double GetAttackDamage() => GetWithPerks(Chars.AttackDamage);
        public void SetAttackDamage(double v) => SetWithPerks(Chars.AttackDamage, v);
        public void AddAttackDamage(double v) => SetAttackDamage(GetAttackDamage() + v);

        public double GetAttackSpeed() => GetWithPerks(Chars.AttackSpeed);
        public void SetAttackSpeed(double v) => SetWithPerks(Chars.AttackSpeed, v);
        public void AddAttackSpeed(double v) { SetAttackSpeed(GetAttackSpeed() + v); AttacksLeft += (int)(v); }

        public double GetAttackRange() => GetWithPerks(Chars.AttackRange);
        public void SetAttackRange(double v) => SetWithPerks(Chars.AttackRange, v);
        public void AddAttackRange(double v) => SetAttackRange(GetAttackRange() + v);

        public double GetMovementSpeed() => GetWithPerks(Chars.MovementSpeed);
        public void SetMovementSpeed(double v) => SetWithPerks(Chars.MovementSpeed, v);
        public void AddMovementSpeed(double v)
        {
            SetMovementSpeed(GetMovementSpeed() + v);
            MovementLeft += v;
        }

        public double GetEnergy() => GetWithPerks(Chars.Energy);
        public void SetEnergy(double v) => SetWithPerks(Chars.Energy, v);
        public void AddEnergy(double v) { SetEnergy(GetEnergy() + v); }

        public double GetMaxEnergy() => GetWithPerks(Chars.MaxEnergy);
        public void SetMaxEnergy(double v) => SetWithPerks(Chars.MaxEnergy, v);
        public void AddMaxEnergy(double v)
        {
            SetMaxEnergy(GetMaxEnergy() + v);
            AddEnergy(Math.Max(0, v));
            if(GetMaxEnergy() < GetEnergy())
                SetEnergy(GetMaxEnergy());
        }

        public double GetEnergyRegen() => GetWithPerks(Chars.EnergyRegen);
        public void SetEnergyRegen(double v) => SetWithPerks(Chars.EnergyRegen, v);
        public void AddEnergyRegen(double v) => SetEnergyRegen(GetEnergyRegen() + v);
        
        public double GetCDReduction() => GetWithPerks(Chars.CDReduction);
        public void SetCDReduction(double v) => SetWithPerks(Chars.CDReduction, v);
        public void AddCDReduction(double v) => SetCDReduction(GetCDReduction() + v);


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
                case Chars.AttackDamage:
                    tempFunc = () => attackDamage;
                    foreach (var perk in Perks)
                        tempFunc = perk.GetAttackDamage(tempFunc);
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
                case Chars.CDReduction:
                    tempFunc = () => cdReduction;
                    foreach (var perk in Perks)
                        tempFunc = perk.GetCDReduction(tempFunc);
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
                case Chars.AttackDamage:
                    tempAction = (s) => attackDamage = s;
                    foreach (var perk in Perks)
                        tempAction = perk.SetAttackDamage(tempAction);
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
                case Chars.Money:
                    tempAction = (s) => money = s;
                    foreach (var perk in Perks)
                        tempAction = perk.SetMoney(tempAction);
                    break;
                case Chars.CDReduction:
                    tempAction = (s) => cdReduction = s;
                    foreach (var perk in Perks)
                        tempAction = perk.SetCDReduction(tempAction);
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
            StartTurn,
            EndTurn,
            Init,
            LevelUp,
        }

        public void LevelUp() => DoWithPerks(Actions.LevelUp, new FuncData(this));

        private FuncData FLevelUp(FuncData data) {
            Level++;
            return data;
        }

        public void Init(Player pl, Map mp, Shop s) => DoWithPerks(Actions.Init, new FuncData(this,playerValue: pl, mapvalue: mp, sV: s));

        private FuncData FInit(FuncData data)
        {
            SetHp(GetMaxHp());
            SetEnergy(GetMaxEnergy());
            P = data.PlayerValue;
            M = data.MapValue;
            S = data.ShopValue;
            return data;
        }

        public void StartTurn() => DoWithPerks(Actions.StartTurn, new FuncData(this));

        private FuncData FStartTurn(FuncData data)
        {
            Chosen = true;
            RefreshAttacks();
            RefreshMovement();
            foreach (var skill in Skills)
                skill.Tick(1);
            return data;
        }

        public void EndTurn() => DoWithPerks(Actions.EndTurn, new FuncData(this));

        private FuncData FEndTurn(FuncData data)
        {
            Regenerate();
            Chosen = false;
            return data;
        }

        public void RefreshMovement() => MovementLeft = GetMovementSpeed();

        public void GetEnergized(double v)
        {
            AddEnergy(GetEnergyRegen());
            var max = GetMaxEnergy();
            if (GetEnergy() > max)
                SetEnergy(max);
        }

        public void Regenerate()
        {
            GetHeal(GetRegen());
            GetEnergized(GetEnergyRegen());
        }

        public void RefreshAttacks() => AttacksLeft = (int)GetAttackSpeed();

        public void GetDamage(Damage damage) => DoWithPerks(Actions.GetDamage, new FuncData(this,playerValue: damage.Pl, dmgV: damage));

        private FuncData FGetDamage(FuncData damage)
        {
            var arm = GetArmor();
            var res = GetResist();
            var resDamage = new Damage(this, damage.PlayerValue, damage.DamageValue.Phys > arm ? damage.DamageValue.Phys - arm : 0,
                damage.DamageValue.Magic > res ? damage.DamageValue.Magic - res : 0,
                damage.DamageValue.Pure);
            AddHp(-resDamage.Sum());
            resDamage.NotifyPlayer();
            return new FuncData(this,dmgV: resDamage);
        }


        public void GetHeal(double heal) => DoWithPerks(Actions.GetHeal, new FuncData(this,dV: heal));

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
                case Actions.StartTurn:
                    Func<FuncData, FuncData> startTurn = FStartTurn;
                    foreach (var perk in Perks)
                        startTurn = perk.StartTurn(startTurn);
                    return startTurn(data);
                case Actions.EndTurn:
                    Func<FuncData, FuncData> endTurn = FEndTurn;
                    foreach (var perk in Perks)
                        endTurn = perk.EndTurn(endTurn);
                    return endTurn(data);
                case Actions.Init:
                    Func<FuncData, FuncData> init = FInit;
                    foreach (var perk in Perks)
                        init = perk.Init(init);
                    return init(data);
                case Actions.LevelUp:
                    Func<FuncData, FuncData> levelUp = FLevelUp;
                    foreach (var perk in Perks)
                        levelUp = perk.LevelUp(levelUp);
                    return levelUp(data);
                default:
                    return new FuncData(this);
            }
        }

        public Dictionary<string, double> GetAllStats() => new Dictionary<string, double>
        {
            {"MHP", GetMaxHp()},
            {"HP", GetHp()},
            {"ME", GetMaxEnergy()},
            {"E", GetEnergy()},
            {"AD", GetAttackDamage()},
            {"AP", GetAbilityPower()},
            {"A", GetArmor()},
            {"R", GetResist()},
            {"AR", GetAttackRange()},
            {"AS", GetAttackSpeed()},
            {"MS", GetMovementSpeed()},
            {"CDR", GetCDReduction()},
            {"HR", GetRegen()},
            {"ER", GetEnergyRegen()},
            {"M", GetMoney()},
        };

        public Xauonre.Core.Point GetPosition() => M.UnitPositions[this];
        public string FastStats() =>
            Name
            + " : MaxHp-" + GetMaxHp()
            + ", Hp-" + GetHp()
            + (GetMaxEnergy() > 1 ? ", MaxEnergy-" + GetMaxEnergy() + ", Energy-" + GetEnergy() : "")
            + ", AttackDamage-" + GetAttackDamage()
            + ", AbilityPower-" + GetAbilityPower()
            + ", MoveLeft-" + MovementLeft
            + ", AttacksLeft-" + AttacksLeft
            + ", Money-" + GetMoney();

        public override int GetHashCode()
        {
            return 123142738 + Name.GetHashCode();
        }
    }

}