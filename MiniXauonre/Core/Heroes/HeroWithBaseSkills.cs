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
                Job = (m, p, h) =>
                {
                    if (AttacksLeft != 0)
                    {
                        var hA = h as HeroWithBaseSkills;
                        var enemiesInRange = GetEnemiesInRange(p, m, GetAttackRange());
                        if (enemiesInRange.Count != 0)
                        {
                            Target = ChooseTarget(enemiesInRange, p);
                            var at = new Damage(p, GetAttackPower());
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
                Job = MoveJob,
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

        protected Hero ChooseTarget(List<Hero> targets, Player player)
        {
            var possibleCommands = new List<Command>
                    {
                        new Command(CommandType.Choose, new List<List<string>>{ targets.Select(h => h.Name).ToList() }),
                    };
            var answer = player.GetCommand(possibleCommands);
            return targets[answer.Data[0]];
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




        private bool MoveJob(Map m, Player p, Hero h)
        {
            var hA = h as HeroWithBaseSkills;
            var possibleSteps = PossibleSteps
                .Where(s => s.Value <= MovementLeft && m.CellIsFree(s.Key + m.UnitPositions[h]))
                .Select(po => po.Key.ToStep())
                .ToList();
            if (possibleSteps.Count != 0)
            {
                var step = ChooseDirection(possibleSteps, p);
                var pStep = StepToPoint(step);
                var dist = new Point(0, 0).GetStepsTo(pStep);
                MovementLeft -= dist;
                m.UnitPositions[h].Add(pStep);
                return true;
            }

            /*Console.WriteLine();
            Console.WriteLine((int)MovementLeft);
            Console.WriteLine();

            for(var i = 0; i < 10; i++)
            {
                for(var j = 0; j < 10; j++)
                {
                    var units = m.GetIn(new Point(j, i));
                    if (units.Count == 0)
                        Console.Write(". ");
                    else
                        Console.Write(units.First().Name[0] + " ");
                        
                }
                Console.WriteLine();
            }*/
            return false;
        }


    }
}
