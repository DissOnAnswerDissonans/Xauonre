using MiniXauonre.Core.Heroes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core
{
    public delegate Func<double> Getter(Func<double> g);
    public delegate Action<double> Setter(Action<double> s);

    public delegate Func<FuncData, FuncData> Act(Func<FuncData,FuncData> a);

    delegate Skill UseSkill(Skill skill);

    class Perk
    {

        protected Getter baseGetter;
        protected Setter baseSetter;
        //public Func<Func<int>, HeroStats, Func<int>> OverGet;
        //public Func<Func<int>, HeroStats, Func<int>> OverSet;


        //Stats
        public Getter GetMaxHp { get; protected set; }
        public Setter SetMaxHp { get; protected set; }

        public Getter GetHp { get; protected set; }
        public Setter SetHp { get; protected set; }

        public Getter GetArmor { get; protected set; }
        public Setter SetArmor { get; protected set; }

        public Getter GetResist { get; protected set; }
        public Setter SetResist { get; protected set; }

        public Getter GetRegen { get; protected set; }
        public Setter SetRegen { get; protected set; }

        public Getter GetMoney { get; protected set; }
        public Setter SetMoney { get; protected set; }

        public Getter GetAbilityPower { get; protected set; }
        public Setter SetAbilityPower { get; protected set; }

        public Getter GetAttackPower { get; protected set; }
        public Setter SetAttackPower { get; protected set; }

        public Getter GetAttackSpeed { get; protected set; }
        public Setter SetAttackSpeed { get; protected set; }

        public Getter GetAttackRange { get; protected set; }
        public Setter SetAttackRange { get; protected set; }

        public Getter GetMovementSpeed { get; protected set; }
        public Setter SetMovementSpeed { get; protected set; }

        //Fns
        public Act GetDamage { get; protected set; }
        public Act GetHeal { get; protected set; }
        public Act NextTurn { get; protected set; }

        public UseSkill SkillFix { get; protected set; }

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
            GetAttackPower = baseGetter;
            SetAttackPower = baseSetter;
            GetAttackSpeed = baseGetter;
            SetAttackSpeed = baseSetter;
            GetAttackRange = baseGetter;
            SetAttackRange = baseSetter;
            GetMovementSpeed = baseGetter;
            SetMovementSpeed = baseSetter;
            //Fns
            GetDamage = (a) => a;
            GetHeal = (a) => a;
            NextTurn = (a) => a;

            //Skills
            SkillFix = (s) => s;
        }
    }
}
