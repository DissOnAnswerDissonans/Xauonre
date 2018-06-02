using MiniXauonre.Core.Heroes.Specials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xauonre.Core;

namespace MiniXauonre.Core.Heroes
{
    class Gaina : HeroWithBaseSkills
    {
        public Skill Pyroblast { get; protected set; }
        public const double PyroblastAPScale = 2;
        public const double PyroblastCost = 100;
        public const double PyroblastCd = 3;
        public const double PyroblastRadius = 3;
        public const double PyroblasTriggerRadius = 1;
        public List<Pyroblast> Blasts { get; protected set; }
        public Perk Collider { get; protected set; }

        public Gaina()
        {
            Name = "Gaina";
            Image = Graphics.resources.Res.Gaina;
            SetMaxHp(777);
            SetMaxEnergy(200);
            SetEnergyRegen(20);
            SetAbilityPower(50);


            Blasts = new List<Specials.Pyroblast>();
            Collider = new Perk
            {
                StartTurn = (s) => (d) =>
                {
                    var list = new List<Pyroblast>(Blasts);
                    foreach (var pyro in list)
                        pyro.Boom();
                    return s(d);
                },
                SkillFix = (s) =>
                {
                    var newSkill = new Skill()
                    {
                        Name = s.Name,
                        CoolDown = s.CoolDown,
                        SkillTypes = s.SkillTypes,
                        Explanation = s.Explanation,
                    };
                    newSkill.Job = (he) =>
                    {
                        var res = s.Job(he);
                        if (res)
                        {
                            var list = new List<Pyroblast>(Blasts);
                            foreach (var pyro in list)
                                pyro.Boom();
                            return true;
                        }
                        return false;
                    };
                    return newSkill;
                },

                EndTurn = (v) => (d) =>
                {
                    foreach (var pyro in Blasts)
                        pyro.Tick();
                    var list = new List<Pyroblast>(Blasts);
                    foreach (var pyro in list)
                        pyro.Boom();
                    return v(d);
                }
            };

            Perks.Add(new Perk
            {
                StartTurn = (f) => (d) =>
                {
                    foreach (var h in d.HeroValue.M.GetHeroes())
                        if(!h.Perks.Contains(Collider))
                            h.Perks.Add(Collider);
                    return f(d);
                },
            });
            
            Pyroblast = new Skill
            {
                Name = "Pyroblast",
                Explanation = () => "Shoots pyroblast that moves diagonally and bounce off of stones and deales " + PyroblastAPScale*100
                + "%AP ("+GetAbilityPower()*PyroblastAPScale+") spell damage to all enemies in "+PyroblastRadius+" range when fly on them. Cost "
                +PyroblastCost + ". CD " + PyroblastCd,
                EnergyCost = PyroblastCost,
                CoolDown = PyroblastCd,
                Job = (h) =>
                {
                    var directions = new List<Point> { new Point(1, 1), new Point(-1, 1), new Point(-1, -1), new Point(1, -1) };
                    var positions = directions.Select(d => d + h.GetPosition()).Where(p => h.M.CellIsFree(p)).ToList();
                    var point = ChoosePoint(positions, h.P);
                    if (point == null) return false;
                    var pyro = new Pyroblast(h)
                    {
                        Direction = new Point(point.X - h.GetPosition().X, point.Y - h.GetPosition().Y)
                    };
                    pyro.Boom = () =>
                        {
                            var place = h.M.UnitPositions[pyro];
                            var targets = h.M.GetHeroPositions().Where(a => a.Key.P != h.P && place.GetStepsTo(a.Value) <= PyroblasTriggerRadius)
                            .Select(a => a.Key);
                            if (targets.Count() == 0)
                                return false;
                            var damage = new Damage(h, h.P, magic: GetAbilityPower() * PyroblastAPScale);
                            targets = h.M.GetHeroPositions().Where(a => a.Key.P != h.P && place.GetStepsTo(a.Value) <= PyroblastRadius).Select(a => a.Key);
                            foreach (var tg in targets)
                                tg.GetDamage(damage);
                            Blasts.Remove(pyro);
                            h.M.UnitPositions.Remove(pyro);
                            return true;
                        };
                    pyro.Tick = () =>
                    {
                        var curretPoint = h.M.UnitPositions[pyro];
                        var nextPoint = curretPoint + pyro.Direction;
                        if (h.M.IsInBounds(nextPoint) && h.M.MapTiles[nextPoint.X, nextPoint.Y].Type == TileType.Empty)
                        {
                            h.M.UnitPositions[pyro] = nextPoint;
                            return;
                        }

                        var one = curretPoint + new Point(pyro.Direction.X, 0);
                        var another = curretPoint + new Point(0, pyro.Direction.Y);
                        var oneCool = h.M.IsInBounds(one) && h.M.MapTiles[one.X, one.Y].Type == TileType.Empty;
                        var anotherCool = h.M.IsInBounds(another) && h.M.MapTiles[another.X, another.Y].Type == TileType.Empty;
                        if(oneCool == anotherCool)
                        {
                            pyro.Direction = new Point(-pyro.Direction.X, -pyro.Direction.Y);
                            h.M.UnitPositions[pyro] = curretPoint + pyro.Direction;
                            return;
                        }
                        if (oneCool)
                        {
                            var newDirection = new Point(pyro.Direction.X, -pyro.Direction.Y);
                            var newPosition = curretPoint + newDirection;
                            if (h.M.IsInBounds(newPosition) && h.M.MapTiles[newPosition.X, newPosition.Y].Type == TileType.Empty)
                            {
                                pyro.Direction = newDirection;
                                h.M.UnitPositions[pyro] = curretPoint + pyro.Direction;
                                return;
                            }
                        }
                        if (anotherCool)
                        {
                            var newDirection = new Point(-pyro.Direction.X, pyro.Direction.Y);
                            var newPosition = curretPoint + newDirection;
                            if (h.M.IsInBounds(newPosition) && h.M.MapTiles[newPosition.X, newPosition.Y].Type == TileType.Empty)
                            {
                                pyro.Direction = new Point(-pyro.Direction.X, pyro.Direction.Y);
                                h.M.UnitPositions[pyro] = curretPoint + pyro.Direction;
                                return;
                            }
                        }
                        pyro.Direction = new Point(-pyro.Direction.X, -pyro.Direction.Y);
                        h.M.UnitPositions[pyro] = curretPoint + pyro.Direction;
                    };
                    Blasts.Add(pyro);
                    h.M.UnitPositions[pyro] = point;
                    return true;
                }
            };

            Pyroblast.SkillTypes.Add(SkillType.Special);
            Skills.Add(Pyroblast);
        }
    }
}
