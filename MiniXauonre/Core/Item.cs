using MiniXauonre.Core.Heroes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core
{
    class Item
    {
        public Perk Effect { get; protected set; }
        public string Name { get; protected set; }
        public Func<Hero, string> Explanation { get; protected set; }
        public double Cost { get; protected set; }
        public List<Item> Parts { get; protected set; }
        public int Tier { get; protected set; }


        public double HP { get; protected set; }
        public double E { get; protected set; }
        public double AD { get; protected set; }
        public double AP { get; protected set; }
        public double R { get; protected set; }
        public double A { get; protected set; }
        public double HR { get; protected set; }
        public double ER { get; protected set; }
        public double MS { get; protected set; }
        public double AS { get; protected set; }
        public double CDR { get; protected set; }


        public Dictionary<StatType, double> Stats { get; private set; }


        public Item()
        {
            Name = "None";
            Explanation = (h) => "";
            Effect = new Perk() { };
            Cost = 0;
            Tier = 0;
            Parts = new List<Item>();
            Stats = new Dictionary<StatType, double>();

            HP  = 0;
            E   = 0;
            AD  = 0;
            AP  = 0;
            R   = 0;
            A   = 0;
            HR  = 0;
            ER  = 0;
            MS  = 0;
            AS  = 0;
            CDR = 0;

            if (HP  != 0) Stats.Add(StatType.MaxHP, HP );
            if (E   != 0) Stats.Add(StatType.MaxEnergy, E  );
            if (AD  != 0) Stats.Add(StatType.AttackDamage, AD );
            if (AP  != 0) Stats.Add(StatType.AbilityPower, AP );
            if (R   != 0) Stats.Add(StatType.Resist, R  );           
            if (A   != 0) Stats.Add(StatType.Armor, A  );
            if (HR  != 0) Stats.Add(StatType.Regen, HR );
            if (ER  != 0) Stats.Add(StatType.EnergyRegen, ER );
            if (MS  != 0) Stats.Add(StatType.MovementSpeed, MS );
            if (AS  != 0) Stats.Add(StatType.AttackSpeed, AS );
            if (CDR != 0) Stats.Add(StatType.CooldownReduction, CDR);
        }


        private void AddStats(Hero h)
        {
            h.AddMaxHp(HP);
            h.AddMaxEnergy(E);
            h.AddAttackDamage(AD);
            h.AddAbilityPower(AP);
            h.AddArmor(A);
            h.AddResist(R);
            h.AddRegen(HR);
            h.AddEnergyRegen(ER);
            h.AddMovementSpeed(MS);
            h.AddAttackSpeed(AS);
            h.AddCDReduction(CDR);
        }

        private void RemoveStats(Hero h)
        {
            h.AddMaxHp(-HP);
            h.AddMaxEnergy(-E);
            h.AddAttackDamage(-AD);
            h.AddAbilityPower(-AP);
            h.AddArmor(-A);
            h.AddResist(-R);
            h.AddRegen(-HR);
            h.AddEnergyRegen(-ER);
            h.AddMovementSpeed(-MS);
            h.AddAttackSpeed(-AS);
            h.AddCDReduction(-CDR);
        }


        public void Bought(Hero h)
        {
            h.AddMoney(-GetFinalCost(h));
            foreach(var part in Parts)
            {
                if (h.Items.Contains(part))
                    part.Remove(h);
            }
            Equip(h);
        }

        public double GetFinalCost(Hero h)
        {
            var resultCost = Cost;
            var gottenParts = h.Items.Where(i => Parts.Contains(i));
            var tempParts = new List<Item>(Parts);
            foreach(var i in gottenParts)
            {
                if (tempParts.Contains(i))
                {
                    tempParts.Remove(i);
                    resultCost -= i.Cost;
                }
            }
            return resultCost;
        }

        protected void Equip(Hero h)
        {
            if (!h.Perks.Contains(Effect))
                h.Perks.Add(Effect);
            h.Items.Add(this);
            AddStats(h);
        }

        public void Remove(Hero h)
        {
            if(h.Items.Contains(this))
            {
                h.Items.Remove(this);
                RemoveStats(h);
                if (!h.Items.Contains(this))
                    h.Perks.Remove(Effect);
            }
        }

        public Dictionary<string, double> GetExplanation()
        {
            var stats = new Dictionary<string, double>();
            if (HP != 0) stats.Add("HP", HP);
            if (E != 0) stats.Add("E", E);
            if (AD != 0) stats.Add("AD", AD);
            if (AP != 0) stats.Add("AP", AP);
            if (R != 0) stats.Add("R", R);
            if (A != 0) stats.Add("A", A);
            if (HR != 0) stats.Add("HR", HR);
            if (ER != 0) stats.Add("ER", ER);
            if (MS != 0) stats.Add("MS", MS);
            if (AS != 0) stats.Add("AS", AS);
            if (CDR != 0) stats.Add("CDR", CDR);
            return stats;
        }


        public override bool Equals(object obj) => Name == (obj as Item).Name;

        public override int GetHashCode()
        {
            return 539060726 + EqualityComparer<string>.Default.GetHashCode(Name);
        }
    }
}
