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

        public Dictionary<Player, Point> SpawnPoints { get; set; }
        public Game(List<Tuple<string, Point>> playerNames)
        {
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
            ChooseHeroes(1, HeroMaker.GetAllHeroes());


            //Choose Talents

            //Main Game Process
            AddAllHeroesOnMap();
            GameProcess();
        }


        public void HeroTurn(Hero hero, Player player)
        {
            while (true) 
            {
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
                    hero.NextTurn();
                    HeroTurn(hero, p);
                }
            }
        }

        public void AddAllHeroesOnMap()
        {
            foreach (var p in Players)
                foreach (var h in p.Heroes)
                    Maze.UnitPositions.Add(h, SpawnPoints[p]);
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
