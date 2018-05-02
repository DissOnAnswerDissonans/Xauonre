﻿using System;
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
            SetMaxHp(900);
            SetMaxEnergy(500);
            SetEnergyRegen(3);
            SetAttackPower(40);
            SetMovementSpeed(10);
            SetAttackRange(11);

            PlacedRocks = new List<Tuple<Point, Effect>>();

            Rock = new Skill
            {
                Name = "Rock",
                Explanation = () => "Places a rock in range " + RockRange + " on " + RockSustain
                + " turns, dealing " + RockDamage + " + " + RockDamageAPscale * 100 + "% AP (" +
                (RockDamage + RockDamageAPscale * GetAbilityPower()) + " total) magical damage. " +
                "Cooldown: " + RockCooldown + ", Energy cost: " + RockEnergyCost,
                Job = (h) =>
                {
                    var damage = new Damage(this, h.P, magic: RockDamage + RockDamageAPscale * GetAbilityPower());
                    var pos = h.M.UnitPositions[h].GetPointsInDistance(0, RockRange).Where(pp => h.M.IsInBounds(pp)).ToList();
                    var point = ChoosePoint(pos, h.P);

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
                    var p = point.GetPointsInDistance(0, RockDamageRadius);
                    foreach (var victim in h.M.UnitPositions.Where(t => p.Contains(t.Value)))
                    {
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
                Explanation = () => "...",
                Job = (h) =>
                {
                    var targets = GetEnemiesInRange(h.P, h.M, EarthRange);
                    if (targets.Count == 0)
                        return false;
                    Target = ChooseTarget(targets, h.P); 
                    var rocks = PlacedRocks.Where(p => p.Item1.GetStepsTo(h.M.UnitPositions[h]) <= EarthRangeReq);
                    foreach (var r in rocks.ToList())
                    {
                        h.M.Effects.Remove(r.Item2);
                        r.Item2.Disactivate(h);
                    }
                    var damage = new Damage(h, h.P, magic: EarthDamage + EarthDamageAPscale * GetAbilityPower() +
                        rocks.Count() * (EarthRockDamage + EarthRockDamageAPscale * GetAbilityPower()));
                    if (rocks.Count() > 0)
                    {
                        var point = h.M.UnitPositions[Target];
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
                    }
                    return true;
                },
                CoolDown = EarthCooldown,
                EnergyCost = EarthEnergyCost,
            };
            Rock.SkillTypes.Add(SkillType.Special);
            EarthPower.SkillTypes.Add(SkillType.Special);
            Skills.Add(Rock);
            Skills.Add(EarthPower);
        }
    }
}