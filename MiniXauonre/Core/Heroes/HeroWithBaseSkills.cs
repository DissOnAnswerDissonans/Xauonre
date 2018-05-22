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

        public HeroWithBaseSkills()
        {
            Name = "Hero with Attack";
            Attack = new Skill
            {
                Name = "Attack",
                Explanation = () => "Deales " + GetAttackDamage() + " phys damage to target Enemy in " + GetAttackRange()  + " units from you. Costs 1 weapon attack.",
                Job = (h) =>
                {
                    if (AttacksLeft != 0)
                    {
                        var enemiesInRange = GetEnemiesInRange(h, h.GetAttackRange());
                        if (enemiesInRange.Count != 0)
                        {
                            h.Targets.Add(ChooseTarget(enemiesInRange, h.P));
                            var at = new Damage(h, h.P, phys: h.GetAttackDamage());
                            foreach(var t in Targets)
                                t.GetDamage(at);
                            h.AttacksLeft--;
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
                        .Where(s => s.Value <= MovementLeft && h.M.CellIsFree(s.Key + h.M.UnitPositions[h]))
                        .Select(po => po.Key.ToStep())
                        .ToList();
                    if (possibleSteps.Count != 0)
                    {
                        var step = ChooseDirection(possibleSteps, h.P);
                        var pStep = StepToPoint(step);
                        var dist = new Point(0, 0).GetStepsTo(pStep);
                        h.MovementLeft -= dist;
                        h.M.UnitPositions[h].Add(pStep);
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

        protected List<Hero> GetEnemiesInRange(Hero h, double r) => h.M.UnitPositions
                            .Where(u => !h.P.Heroes.Contains(u.Key))
                            .Where(u => h.M.UnitPositions[h].GetStepsTo(u.Value) <= r)
                            .Select(u => u.Key)
                            .ToList();

        protected List<Hero> GetAlliesInRange(Hero h, double r) => h.M.UnitPositions
                            .Where(u => h.P.Heroes.Contains(u.Key))
                            .Where(u => h.M.UnitPositions[this].GetStepsTo(u.Value) <= r)
                            .Select(u => u.Key)
                            .ToList();

        protected List<Hero> GetHeroesInRange(Hero h, double r) => h.M.UnitPositions
                            .Where(u => h.M.UnitPositions[this].GetStepsTo(u.Value) <= r)
                            .Select(u => u.Key)
                            .ToList();

        
        protected Hero ChooseTarget(List<Hero> targets, Player player)
        {
            return targets[0];
        }


        protected Point ChoosePoint(List<Point> points, Player player)
        {
            return points[0];
        }

        protected Point AskRelativePoint(Point zero, Player player)
        {
            return zero;
        }

        protected StepTypes ChooseDirection(List<StepTypes> steps, Player player)
        {
            return StepTypes.Right;
        }
        
    }
}
