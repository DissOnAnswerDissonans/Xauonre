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
                Availiable = (h) => h.AttacksLeft >= 1,
                Job = (h) =>
                {
                    if (h.AttacksLeft != 0)
                    {
                        var enemiesInRange = GetEnemiesInRange(h, h.GetAttackRange());
                        if (enemiesInRange.Count != 0)
                        {
                            var target = ChooseTarget(enemiesInRange, h.P);
                            if (target == null) return false;
                            h.Targets.Add(target);
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
                Availiable = (h) => h.MovementLeft >= 2,
                Job = (h) =>
                {
                    var stepsLeft = h.MovementLeft;
                    var points = h.M.UnitPositions[h].GetPointsInDistance(1, stepsLeft, (p) => h.M.CellIsFree(p));
                    if (points == null) return false;
                    var point = ChoosePoint(points.Keys.ToList(), h.P);
                    if (point == null) return false;
                    var distance = points[point];
                    h.M.UnitPositions[h] = point;
                    h.MovementLeft -= distance;
                    return true;
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
            var tg = player.ChooseTarget(targets);
            if (targets.Contains(tg)) return tg;
            return null;
        }

        protected Point ChoosePoint(List<Point> points, Player player)
        {
            var tg = player.ChoosePoint(points);
            if (points.Contains(tg)) return tg;
            return null;
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
