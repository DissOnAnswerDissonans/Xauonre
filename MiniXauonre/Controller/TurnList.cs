using System;
using System.Collections.Generic;
using MiniXauonre.Core.Heroes;

namespace MiniXauonre.Controller
{
    class TurnList : Dictionary<Hero, double>
    {
        public void Init(Game game)
        {
            
        }
        
        public bool HeroWait(Hero hero, double waitTime)
        {
            try
            {
                this[hero] += waitTime;
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private void WaitNextTurn()
        {
            var wait = double.MaxValue;
            foreach (var item in Values)
                wait = Math.Min(wait, item);
            foreach (var item in Keys)
                this[item] -= wait;

        }

        public Hero GetNextHero()
        {
            WaitNextTurn();
            foreach (var item in this)
                if (Math.Abs(item.Value) < 1e-9)
                    return item.Key;
            throw new ArgumentNullException();
        }
    }
}