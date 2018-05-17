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
        public Func<string> Explanation { get; protected set; }
        public double Cost { get; protected set; }
        public List<Item> Parts { get; protected set; }


        public double HP { get; protected set; }
        public double E { get; protected set; }
        public double AD { get; protected set; }
        public double AP { get; protected set; }
        public double R { get; protected set; }
        public double A { get; protected set; }
        public double HR { get; protected set; }
        public double ER { get; protected set; }
        public double MS { get; protected set; }


        public Item()
        {
            Name = "None";
            Explanation = () => "None";
            Effect = new Perk() { };
            Cost = 0;
            Parts = new List<Item>();

            HP = 0;
            E = 0;
            AD = 0;
            AP = 0;
            R = 0;
            A = 0;
            HR = 0;
            ER = 0;
            MS = 0;
        }


        private void AddStats(Hero h)
        {
            h.AddMaxHp(HP);
            h.AddEnergy(E);
            h.AddAttackDamage(AD);
            h.AddAbilityPower(AP);
            h.AddArmor(A);
            h.AddResist(R);
            h.AddRegen(HR);
            h.AddEnergyRegen(ER);
            h.AddMovementSpeed(MS);
        }

        private void RemoveStats(Hero h)
        {
            h.AddMaxHp(-HP);
            h.AddEnergy(-E);
            h.AddAttackDamage(-AD);
            h.AddAbilityPower(-AP);
            h.AddArmor(-A);
            h.AddResist(-R);
            h.AddRegen(-HR);
            h.AddEnergyRegen(-ER);
            h.AddMovementSpeed(-MS);
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

        protected void Remove(Hero h)
        {
            if(h.Items.Contains(this))
            {
                h.Items.Remove(this);
                RemoveStats(h);
                if (!h.Items.Contains(this))
                    h.Perks.Remove(Effect);
            }
        }


        public override bool Equals(object obj) => Name == (obj as Item).Name;

        public override int GetHashCode()
        {
            return 539060726 + EqualityComparer<string>.Default.GetHashCode(Name);
        }
    }
}
