using MiniXauonre.Core.Heroes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core
{
    delegate Func<double> Getter(Func<double> g);
    delegate Action<double> Setter(Action<double> s);

    delegate Func<FuncData, FuncData> Act(Func<FuncData,FuncData> a);

    delegate Skill UseSkill(Skill skill);

    class Perk
    {

        protected Getter baseGetter;
        protected Setter baseSetter;
        //public Func<Func<int>, HeroStats, Func<int>> OverGet;
        //public Func<Func<int>, HeroStats, Func<int>> OverSet;


        //Stats
        public Getter GetMaxHp { get; set; }
        public Setter SetMaxHp { get; set; }

        public Getter GetHp { get; set; }
        public Setter SetHp { get; set; }

        public Getter GetArmor { get; set; }
        public Setter SetArmor { get; set; }

        public Getter GetResist { get; set; }
        public Setter SetResist { get; set; }

        public Getter GetRegen { get; set; }
        public Setter SetRegen { get; set; }

        public Getter GetMoney { get; set; }
        public Setter SetMoney { get; set; }

        public Getter GetAbilityPower { get; set; }
        public Setter SetAbilityPower { get; set; }

        public Getter GetAttackDamage { get; set; }
        public Setter SetAttackDamage { get; set; }

        public Getter GetAttackSpeed { get; set; }
        public Setter SetAttackSpeed { get;  set; }

        public Getter GetAttackRange { get;  set; }
        public Setter SetAttackRange { get;  set; }

        public Getter GetMovementSpeed { get;  set; }
        public Setter SetMovementSpeed { get;  set; }
        
        public Getter GetEnergy { get; set; }
        public Setter SetEnergy { get; set; }
        
        public Getter GetEnergyRegen { get; set; }
        public Setter SetEnergyRegen { get; set; }
        
        public Getter GetMaxEnergy { get; set; }
        public Setter SetMaxEnergy { get; set; }
        
        public Getter GetCDReduction { get; set; }
        public Setter SetCDReduction { get; set; }

        //Fns
        public Act GetDamage { get;  set; }
        public Act GetHeal { get;  set; }
        public Act StartTurn { get;  set; }
        public Act EndTurn { get; set; }
        public Act Init { get; set; }
        public Act LevelUp { get; set; }

        public UseSkill SkillFix { get;  set; }

        public Perk() {
            baseGetter = (g) => g;
            baseSetter = (s) => s;
            //Stats
            GetMaxHp = baseGetter;
            SetMaxHp = baseSetter;
            GetHp = baseGetter;
            SetHp = baseSetter;
            GetArmor = baseGetter;
            SetArmor = baseSetter;
            GetResist = baseGetter;
            SetResist = baseSetter;
            GetRegen = baseGetter;
            SetRegen = baseSetter;
            GetAbilityPower = baseGetter;
            SetAbilityPower = baseSetter;
            GetAttackDamage = baseGetter;
            SetAttackDamage = baseSetter;
            GetAttackSpeed = baseGetter;
            SetAttackSpeed = baseSetter;
            GetAttackRange = baseGetter;
            SetAttackRange = baseSetter;
            GetMovementSpeed = baseGetter;
            SetMovementSpeed = baseSetter;
            GetEnergy = baseGetter;
            SetEnergy = baseSetter;
            GetEnergyRegen = baseGetter;
            SetEnergyRegen = baseSetter;
            GetMaxEnergy = baseGetter;
            SetMaxEnergy = baseSetter;
            GetMoney = baseGetter;
            SetMoney = baseSetter;
            GetCDReduction = baseGetter;
            SetCDReduction = baseSetter;
            //Fns
            GetDamage = (a) => a;
            GetHeal = (a) => a;
            StartTurn = (a) => a;
            EndTurn = (a) => a;
            Init = (a) => a;
            LevelUp = (a) => a;
            //Skills
            SkillFix = (s) => s;
        }
    }
}
