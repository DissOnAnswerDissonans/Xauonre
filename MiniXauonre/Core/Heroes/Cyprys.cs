using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xauonre.Core;

/**
    Cyprys

- Rock: Устанавливает в выбранном месте камень на 10 ходов и наносит всем врагам 
    в радиусе 3  50 + 50%AP магического урона. CD 2. Cost 20.
- Earth Magic: Берет все камни установленные собой в радиусе 35 и бросает в противника 
    нанося 100 + 50%AP +  (50 + 50%AP) за каждый камень. CD 10. Cost 150.

Hp = 900
Energy = 500
ER = 3
AD = 40
MS = 10
AR = 11
*/

namespace MiniXauonre.Core.Heroes
{
    class Cyprys : HeroWithBaseSkills
    {
        public Skill Rock { get; set; }
        public const double RockRange = 12;
        public const double RockDamage = 50;
        public const double RockDamageAPscale = .5;
        public const double RockDamageRadius = 3;
        public const double RockEnergyCost = 20;
        public const double RockCooldown = 2;
        public const double RockSustain = 10;
        public Skill EarthPower { get; set; }
        public const double EarthRange = 12;
        public const double EarthRangeReq = 35;
        public const double EarthDamage = 100;
        public const double EarthDamageAPscale = .5;
        public const double EarthRockDamage = 50;
        public const double EarthRockDamageAPscale = .5;
        public const double EarthEnergyCost = 150;
        public const double EarthCooldown = 10;

        private List<Tuple<Point, Effect>> PlacedRocks { get; set; }

        public Cyprys()
        {
            Name = "Cyprys";
            Image = Graphics.resources.Res.Cyprus;
            SetMaxHp(900);
            SetMaxEnergy(500);
            SetEnergyRegen(7);
            SetAttackDamage(40);
            SetMovementSpeed(10);
            SetAttackRange(11);

            PlacedRocks = new List<Tuple<Point, Effect>>();

            Rock = new Skill
            {
                Name = "Rock",
                Explanation = () => "Places a rock in range " + RockRange + " on " + RockSustain
                + " turns, dealing " + RockDamage + " + " + RockDamageAPscale * 100 + "%AP (" +
                (RockDamage + RockDamageAPscale * GetAbilityPower()) + ") magical damage. " +
                "Cooldown " + RockCooldown + ". Cost " + RockEnergyCost,
                Job = (h) =>
                {
                    var damage = new Damage(this, h.P, magic: RockDamage + RockDamageAPscale * h.GetAbilityPower());
                    var pos = h.M.UnitPositions[h].GetPointsInDistance(0, RockRange).Keys.Where(pp => h.M.IsInBounds(pp)).ToList();
                    var point = ChoosePoint(pos, h.P);
                    if (point == null) return false;

                    var RockEffect = new Effect(h, (int)RockSustain);
                    RockEffect.Activate = (eh) =>
                    {
                        if (eh.M.MapTiles[point.X, point.Y].Type != TileType.Solid)
                        {
                            PlacedRocks.Add(Tuple.Create(point, RockEffect));
                            eh.M.MapTiles[point.X, point.Y].Type = TileType.Solid;
                        }
                    };

                    RockEffect.Disactivate = (eh) =>
                    {
                        PlacedRocks.Remove(Tuple.Create(point, RockEffect));
                        eh.M.MapTiles[point.X, point.Y].Type = TileType.Empty;
                    };

                    RockEffect.Activate(h);
                    h.M.Effects.Add(RockEffect);
                    var p = point.GetPointsInDistance(0, RockDamageRadius).Keys;
                    foreach (var victim in h.M.UnitPositions.Where(t => p.Contains(t.Value)))
                    {
                        if (!h.P.Heroes.Contains(victim.Key))
                            victim.Key.GetDamage(damage);
                    }
                    return true;
                },
                CoolDown = RockCooldown,
                EnergyCost = RockEnergyCost,
            };

            EarthPower = new Skill
            {
                Name = "Earth Power",
                Explanation = () => "Get all Rocks in " + EarthRangeReq + " units and throw in enemy in "
                + EarthRange + " units dealing " + EarthDamage + " + " + EarthDamageAPscale * 100
                + "%AP  +  for each Rock thrown " + EarthRockDamage + " + " + EarthRockDamageAPscale * 100
                + "%AP (" + (EarthDamage + EarthDamageAPscale * GetAbilityPower() +
                M.UnitPositions[this].GetPointsInDistance(0, EarthRangeReq).Keys
                .Where(a => PlacedRocks.Select(f=>f.Item1).Contains(a)).Count()
                *(EarthRockDamage+EarthRockDamageAPscale*GetAbilityPower()))
                + ")spell damage.\n CD " + EarthCooldown + ". Cost " + EarthEnergyCost,
                Job = (h) =>
                {
                    var targets = GetEnemiesInRange(h, EarthRange);
                    if (targets.Count == 0)
                        return false;
                    var target = ChooseTarget(targets, h.P);
                    if (target == null) return false;
                    h.Targets.Add(target);
                    var rocks = PlacedRocks.Where(p => p.Item1.GetStepsTo(h.M.UnitPositions[h]) <= EarthRangeReq);
                    var rocksNumber = rocks.Count();
                    Effect maxLifeRock = null;
                    foreach (var r in rocks.ToList())
                    {
                        if (maxLifeRock == null || maxLifeRock.Timer < r.Item2.Timer)
                            maxLifeRock = r.Item2;
                        h.M.Effects.Remove(r.Item2);
                        r.Item2.Disactivate(h);
                    }
                    var damage = new Damage(h, h.P, magic: EarthDamage + EarthDamageAPscale * h.GetAbilityPower() +
                        rocks.Count() * (EarthRockDamage + EarthRockDamageAPscale * h.GetAbilityPower()));
                    h.Targets[0].GetDamage(damage);
                    if (rocksNumber > 0)
                    {
                        var point = h.M.UnitPositions[Targets[0]] + new Point(0,0);
                        var RockEffect = new Effect(h, maxLifeRock.Timer);
                        RockEffect.Activate = (eh) =>
                        {
                            if (eh.M.MapTiles[point.X, point.Y].Type != TileType.Solid)
                            {
                                PlacedRocks.Add(Tuple.Create(point, RockEffect));
                                eh.M.MapTiles[point.X, point.Y].Type = TileType.Solid;
                            }
                        };

                        RockEffect.Disactivate = (eh) =>
                        {
                            PlacedRocks.Remove(Tuple.Create(point, RockEffect));
                            eh.M.MapTiles[point.X, point.Y].Type = TileType.Empty;
                        };

                        RockEffect.Activate(h);
                        h.M.Effects.Add(RockEffect);
                    }
                    return true;
                },
                CoolDown = EarthCooldown,
                EnergyCost = EarthEnergyCost,
            };
            EarthPower.SkillTypes.Add(SkillType.Special);
            Skills.Add(EarthPower);
            Skills.Add(Rock);
            Rock.SkillTypes.Add(SkillType.Special);

        }
    }
}
