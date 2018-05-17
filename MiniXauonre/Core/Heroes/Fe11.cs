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
        public const double RageArmorBuff = 2;
        public const double RageADBuff = 3;
        public const double RageDuration = 2;

        public Skill Wall { get; set; }
        public Perk Rage { get; set; }

        private int Stacks { get; set; }
        private int Calm { get; set; }
        private bool WallOn { get; set; }
        private List<Point> PlacedWalls { get; set; }
        public Skill BuffedAttack { get; private set; }

        public Fe11()
        {
            Name = "Fe11";
            SetMaxHp(1300);
            SetAttackDamage(30);
            SetResist(5);
            SetMovementSpeed(11);
            SetAttackRange(4);
            SetAttackSpeed(2);
            SetMaxEnergy(120);
            SetEnergyRegen(6);

            Stacks = 0;

            Rage = new Perk
            {
                GetArmor = (g) => () => g() + Stacks * RageArmorBuff,
                SetArmor = (s) => (v) => s(v - Stacks * RageArmorBuff),
                GetAttackDamage = (g) => () => g() + Stacks * RageADBuff,
                SetAttackDamage = (s) => (v) => s(v - Stacks * RageADBuff),

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
                            Job = (h) =>
                            {
                                var prev = sf.Job(h);
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

            Skills.Remove(Attack);
            BuffedAttack = new Skill
            {
                Name = Attack.Name,
                Explanation = () => Attack.Explanation() + "On attack increases you AD by " + RageADBuff + " and armor by " + RageArmorBuff + " for " + RageDuration
                + " turns. Attacking refreshes times.",
                Job = Attack.Job
            };
            BuffedAttack.SkillTypes.Add(SkillType.Attack);
            Skills.Add(BuffedAttack);


            Wall = new Skill
            {
                Name = "YouShallNotPass",
                Explanation = () => WallOn ? "Destroyes the wall builded by this skill." : "Builds a wall in " + WallMinDist + "-" + WallMaxDist +
                    " range around you. On next use destroyes it. Eats " + WallManaPerTurnCost + " energy at the end of turn (if not enough - turnes off).",

                Job = (h) =>
                {
                    if (WallOn)
                        DestroyWall(M);
                    else
                        BuildWall(M);
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
