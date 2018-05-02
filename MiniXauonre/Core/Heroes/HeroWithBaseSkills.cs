using MiniXauonre.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xauonre.Core;
using static Xauonre.Core.Point;

namespace MiniXauonre.Core.Heroes
{
    class HeroWithBaseSkills : Hero
    {
        protected Skill Attack { get; set; }
        protected Skill Move { get; set; }

        protected Hero Target { get; set; }
        public HeroWithBaseSkills()
        {
            Target = null;


            Name = "Hero with Attack";
            Attack = new Skill
            {
                Name = "Attack",
                Explanation = () => "Deales " + GetAttackPower() + " damage to target Enemy in " + GetAttackRange()  + " units from you. Costs 1 weapon attack.",
                Job = (h) =>
                {
                    if (AttacksLeft != 0)
                    {
                        var enemiesInRange = GetEnemiesInRange(h.P, h.M, GetAttackRange());
                        if (enemiesInRange.Count != 0)
                        {
                            Target = ChooseTarget(enemiesInRange, P);
                            var at = new Damage(P, GetAttackPower());
                            Target.GetDamage(at);
                            AttacksLeft--;
                            //Console.WriteLine(Attack.ToString());
                            return true;
                        }
                        /*foreach (var hero in m.UnitPositions.Keys)
                            Console.WriteLine(hero + "   Hp-" + hero.GetHp());*/
                    }
                    return false;
                }
            };

            Move = new Skill
            {
                Name = "Move",
                Explanation = () => "Moves you into nearby empty cell on the map (linearry costs " + GeometryRules.NormalFactor + " movement, dioganally "
                    + GeometryRules.DiagonalFactor + " movement).",
                Job = (h) =>
                {
                    var hA = h as HeroWithBaseSkills;
                    var possibleSteps = PossibleSteps
                        .Where(s => s.Value <= MovementLeft && M.CellIsFree(s.Key + M.UnitPositions[h]))
                        .Select(po => po.Key.ToStep())
                        .ToList();
                    if (possibleSteps.Count != 0)
                    {
                        var step = ChooseDirection(possibleSteps, P);
                        var pStep = StepToPoint(step);
                        var dist = new Point(0, 0).GetStepsTo(pStep);
                        MovementLeft -= dist;
                        M.UnitPositions[h].Add(pStep);
                        return true;
                    }
                    return false;
                },
            };
            Attack.SkillTypes.Add(SkillType.Attack);
            Move.SkillTypes.Add(SkillType.Move);
            Skills.Add(Attack);
            Skills.Add(Move);
        }

        protected List<Hero> GetEnemiesInRange(Player p, Map m, double r) => m.UnitPositions
                            .Where(u => !p.Heroes.Contains(u.Key))
                            .Where(u => m.UnitPositions[this].GetStepsTo(u.Value) <= r)
                            .Select(u => u.Key)
                            .ToList();

        protected List<Hero> GetHeroesInRange(Player p, Map m, double r) => m.UnitPositions
                            .Where(u => m.UnitPositions[this].GetStepsTo(u.Value) <= r)
                            .Select(u => u.Key)
                            .ToList();

        protected Hero ChooseTarget(List<Hero> targets, Player player)
        {
            var possibleCommands = new List<Command>
                    {
                        new Command(CommandType.Choose, new List<List<string>>{ targets.Select(h => h.Name).ToList() }),
                    };
            var answer = player.GetCommand(possibleCommands);
            return targets[answer.Data[0]];
        }


        protected Point ChoosePoint(List<Point> points, Player player)
        {
            var possibleCommands = new List<Command>
                {
                    new Command(CommandType.Choose, new List<List<string>>{ points.Select(p => p.X + " " + p.Y).ToList()}),
                };
            var answer = player.GetCommand(possibleCommands);
            return points[answer.Data[0]];
        }

        protected Point AskRelativePoint(Point zero, Player player)
        {
            return zero;
        }

        protected StepTypes ChooseDirection(List<StepTypes> steps, Player player)
        {
            var possibleCommands = new List<Command>
                    {
                        new Command(CommandType.Choose, new List<List<string>>{ steps.Select(h => h.ToString()).ToList() }),
                    };
            var answer = player.GetCommand(possibleCommands);
            return steps[answer.Data[0]];
        }
    }
}
