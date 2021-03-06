﻿using MiniXauonre.Core;
using MiniXauonre.Core.Heroes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniXauonre.Graphics;
using System.Windows.Forms;
using Xauonre.Core;

namespace MiniXauonre.Controller
{
    class Player
    {
        public Game Game { get; private set; }
        public string Name { get; private set; }
        public List<Hero> Heroes { get; set; }

        public Hero CurrentHero { get; set; }

        public double AllDamage { get; set; }

        public static double[] Levels =
        {
            0,
            300,
            1000,
            2000,
            3500,
            6000,
            10000,
            16000,
            30000,
            50000,
            100000,
        };

        public int Level { get; set; }

        public int LevelUpMoney { get; set; }

        public Player(Game g, string name)
        {
            Game = g;
            Level = 0;
            AllDamage = 0;
            Name = name;
            Heroes = new List<Hero>();
        }

        public void InitPlayer()
        {
            foreach (var h in Heroes)
            {
                h.SetMoney(Game.Rules.StartMoney);
            }
        }

        public void EndTurn()
        {
            
        }
        
        public void LevelUp()
        {
            Level++;
            foreach(var hero in Heroes)
            {
                hero.AddMoney(Level * Game.Rules.LevelUpMoney);
                hero.LevelUp();
            }
        }

        public void NotifyAboutDamage(Damage damage)
        {
            AllDamage += damage.Sum();
            while (Level < Levels.Length - 1 && Levels[Level + 1] <= AllDamage)
                LevelUp();
        }

        public Hero ChooseTarget(List<Hero> targets) => Game.ChooseTarget(targets);
        public Point ChoosePoint(List<Point> points) => Game.ChoosePoint(points);
        
        public Shop GetShop() => CurrentHero.S;
    }
}
