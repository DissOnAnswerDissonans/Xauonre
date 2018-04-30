using MiniXauonre.Core;
using MiniXauonre.Core.Heroes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xauonre.Core;

namespace MiniXauonre.Controller
{
    class Game
    {
        public List<Player> Players { get; set; }
        public Map Maze { get; set; }

        public bool GameLegality { get; set; }
        public Dictionary<Player, Point> SpawnPoints { get; set; }
        public Game(List<Tuple<string, Point>> playerNames)
        {
            GameLegality = true;
            Maze = new Map();
            Players = new List<Player>();
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
            ChooseHeroes(2, HeroMaker.GetAllHeroes());


            //Choose Talents

            //Main Game Process
            AddAllHeroesOnMap();
            GameProcess();
        }


        public void PrintWorld()
        {
            Console.WriteLine();

            for (var i = 0; i < Maze.Length; i++)
            {
                for (var j = 0; j < Maze.Width; j++)
                {
                    var units = Maze.GetIn(new Point(j, i));
                    if (units.Count == 0)
                        Console.Write(".");
                    else
                        Console.Write(units.First().Name[0]);
                }
                Console.WriteLine();
            }


            foreach (var hero in Maze.UnitPositions.Keys)
                hero.FastPrintStats();
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

        public void HeroTurn(Hero hero, Player player)
        {
            while (GameLegality) 
            {
                CheckWorld();   
                PrintWorld();
                var skills = hero.Skills.Select(s => s.Name).ToList();
                var possibleCommands = new List<Command>
                        {
                            new Command(CommandType.UseAbility, new List<List<string>> { skills }),
                            new Command(CommandType.Cancel, new List<List<string>>{ })
                        };
                var answer = player.GetCommand(possibleCommands);
                if (answer.Type == CommandType.UseAbility)
                {
                    hero.Skills[answer.Data[0]].Work(Maze, player, hero);
                }
                else
                    break;
            }
        }

        public void GameProcess()
        {
            while (true)
            {
                foreach (var p in Players)
                {
                    var hero = p.GetNextHero();
                    hero.NextTurn(Maze, p);
                    HeroTurn(hero, p);
                }
            }
        }

        public void AddAllHeroesOnMap()
        {
            foreach (var p in Players)
                foreach (var h in p.Heroes)
                {
                    Maze.UnitPositions.Add(h, SpawnPoints[p] + new Point());
                    h.Init();
                }
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








    }
}
