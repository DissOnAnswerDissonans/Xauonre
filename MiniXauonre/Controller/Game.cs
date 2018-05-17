using MiniXauonre.Core;
using MiniXauonre.Core.Heroes;
using MiniXauonre.Core.Shops;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MiniXauonre.Graphics;
using Xauonre.Core;

namespace MiniXauonre.Controller
{
    class Game
    {
        public List<Player> Players { get; set; }
        public Map Maze { get; set; }
        public Shop Shop { get; set; }

        public int HeroesNumber { get; set; }
        public bool GameLegality { get; set; }
        public Dictionary<Player, Point> SpawnPoints { get; set; }
        public Game(List<Tuple<string, Point>> playerNames, int heroesNumber = 1, int length = 20, int width = 50)
        {
            GameLegality = true;
            HeroesNumber = heroesNumber;
            Maze = new Map(length, width);
            Players = new List<Player>();
            Shop = new BasicShop();
            SpawnPoints = new Dictionary<Player, Point>();
            foreach (var name in playerNames)
            {
                var player = new Player(name.Item1);
                Players.Add(player);
                SpawnPoints.Add(player, name.Item2);
            };
        }

        public void StartGame()
        {
            //Choose heroes
            ChooseHeroes(HeroesNumber, HeroMaker.GetAllHeroes());


            //Choose Talents

            //Main Game Process
            AddAllHeroesOnMap();
            Players[0].AllDamage = 100000;
            GameProcess();
        }

        public void ChooseHeroes(int numberHeroes, List<Hero> heroes)
        {
            for (var i = 0; i < numberHeroes; i++)
            {
                foreach (var player in Players)
                {
                    var possibleCommands = new List<Command>
                        {
                            new Command(CommandType.Choose, new List<List<string>>{ heroes.Select(h => h.Name).ToList() }),
                        };
                    var answer = player.GetCommand(possibleCommands);
                    player.Heroes.Add(heroes[answer.Data[0]]);
                    heroes.RemoveAt(answer.Data[0]);
                }
            }
        }
        public void AddAllHeroesOnMap()
        {
            foreach (var p in Players)
                foreach (var h in p.Heroes)
                {
                    Maze.UnitPositions.Add(h, SpawnPoints[p] + new Point());
                    h.Init(p, Maze, Shop);
                }
        }

        public void GameProcess()
        {
            while (GameLegality)
            {
                foreach (var p in Players)
                {
                    var hero = p.GetNextHero();
                    Console.WriteLine();
                    Console.WriteLine(hero.Name);
                    hero.StartTurn(Maze, p);
                    HeroTurn(hero, p);
                    hero.EndTurn(Maze, p);
                    Maze.TickTalents(p, hero);
                    if (!GameLegality)
                        break;
                }
            }
        }

        public void HeroTurn(Hero hero, Player player)
        {
            while (GameLegality)
            {
                CheckWorld();
                PrintWorld();
                var skills = hero.Skills.Select(s => s.Name + " (" + s.Explanation() + ")").ToList();
                var items = hero.S.Items;
                var possibleCommands = new List<Command>
                        {
                            new Command(CommandType.UseAbility, new List<List<string>> { skills }),
                            new Command(CommandType.Cancel, new List<List<string>>{ }),
                            new Command(CommandType.Buy, new List<List<string>>{ items.Select(i => i.Name + "\n" + i.Explanation()).ToList() })
                        };
                var answer = player.GetCommand(possibleCommands);
                if (answer.Type == CommandType.UseAbility)
                {
                    hero.UseSkill(answer.Data[0], Maze, player);
                    CheckWorld();
                }
                else if (answer.Type == CommandType.Buy)
                {
                    var num = answer.Data[0];
                    if(num >= 0 && num < items.Count)
                        hero.BuyItem(items[answer.Data[0]]);
                    CheckWorld();
                }
                else
                {
                    CheckWorld();
                    break;
                }
            }
        }

        public void PrintWorld()
        {
            var form = new ScreenForm(Maze, Players);
            Application.Run(form);
        }

        public void CheckWorld()
        {
            foreach (var p in Players)
            {
                var heroes = new List<Hero>(p.Heroes);
                foreach (var h in heroes)
                    if (h.GetHp() <= 0)
                    {
                        Maze.UnitPositions.Remove(h);
                        p.Heroes.Remove(h);
                    }
            }
            var players = new List<Player>(Players);
            foreach(var p in players)
            {
                if (p.Heroes.Count == 0)
                    Players.Remove(p);
            }
            if (Players.Count <= 1)
            {
                GameLegality = false;
                if (Players.Count == 1)
                    Console.WriteLine("Winner Player - " + Players[0].Name);
                else
                    Console.WriteLine("You all suck");
            }
        }
    }
}
