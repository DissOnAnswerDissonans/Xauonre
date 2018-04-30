using MiniXauonre.Controller;
using MiniXauonre.Core.Perks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xauonre.Core;

namespace MiniXauonre.Core.Heroes
{
    class Fe11 : HeroWithBaseSkills
    {
        public const double WallMinDist = 11;
        public const double WallMaxDist = 13;
        public const double WallManaPerTurnCost = 30;
        public const double RageArmorBuff = 1;
        public const double RageADBuff = 1;
        public const double RageDuration = 2;

        public Skill Wall { get; set; }
        public Perk Rage { get; set; }

        private int Stacks { get; set; }
        private int Calm { get; set; }
        private bool WallOn { get; set; }
        private List<Point> PlacedWalls { get; set; }

        public Fe11()
        {
            Name = "Fe11";
            SetMaxHp(1300);
            SetAttackPower(30);
            SetResist(5);
            SetMovementSpeed(11);
            SetAttackRange(4);
            SetAttackSpeed(2);
            SetMaxEnergy(120);
            SetEnergyRegen(6);

            Stacks = 0;

            Rage = new Perk
            {
                GetArmor = (g) => () => g() + Stacks,
                SetArmor = (s) => (v) => s(v - Stacks),
                GetAttackPower = (g) => () => g() + Stacks,
                SetAttackPower = (s) => (v) => s(v - Stacks),

                StartTurn = (i) => (d) =>
                {
                    var a = i(d);
                    if (Calm == 0)
                        Stacks = 0;
                    if (Calm > 0)
                        Calm--;
                    return a;
                },

                SkillFix = (sf) => {
                    if (sf.SkillTypes.Contains(SkillType.Attack))
                        return new Skill
                        {
                            Job = (m, p, h) =>
                            {
                                var prev = sf.Job(m, p, h);
                                if (prev)
                                {
                                    Calm = (int)RageDuration;
                                    Stacks++;
                                }
                                return prev;
                            }
                        };
                    return sf;
                },

                //Stuff from Wall skill
                EndTurn = (i) => (d) =>
                {
                    var a = i(d);
                    if (WallOn)
                    {
                        if (GetEnergy() < WallManaPerTurnCost)
                            DestroyWall(d.MapValue);
                        else
                            AddEnergy(-WallManaPerTurnCost);
                    }
                    return a;
                },
            };
            Perks.Add(Rage);


            Wall = new Skill
            {
                Name = "YouShallNotPass",
                Explanation = "Builds a wall in " + WallMinDist + "-" + WallMaxDist +
                    " range around you. On next use destroyes it. Eats " + WallManaPerTurnCost + " energy at the end of turn (if not enought - turnes off)",

                Job = (m, p, h) =>
                {
                    if (WallOn)
                        DestroyWall(m);
                    else
                        BuildWall(m);
                    return WallOn;
                },
            };
            Wall.SkillTypes.Add(SkillType.Special);
            Skills.Add(Wall);
        }


        public void BuildWall(Map m)
        {
            WallOn = true;
            PlacedWalls = m.UnitPositions[this].GetPointsInDistance(WallMinDist, WallMaxDist)
                .Where(po => m.IsInBounds(po) && m.MapTiles[po.X, po.Y].Type != TileType.Solid)
                .ToList();
            foreach (var wall in PlacedWalls)
                m.MapTiles[wall.X, wall.Y].Type = TileType.Solid;
        }

        public void DestroyWall(Map m)
        {
            WallOn = false;
            foreach (var wall in PlacedWalls)
                m.MapTiles[wall.X, wall.Y].Type = TileType.Empty;
            PlacedWalls = new List<Point>();
        }


    }
}
