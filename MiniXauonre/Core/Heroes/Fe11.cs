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
        public const double WallBaseManaPerTurnCost = 80;
        public const double WallTurnManaMultiplier = 3;
        public const double RageArmorBuff = 0.8;
        public const double RageADBuff = 3;
        public const double RageDuration = 3;
        public int WallStacks { get; set; }

        public Skill Wall { get; set; }
        public Perk Rage { get; set; }
        public Effect WallControl { get; set; }

        private int Stacks { get; set; }
        private bool WallOn { get; set; }
        private List<Point> PlacedWalls { get; set; }
        public Skill BuffedAttack { get; private set; }

        public Fe11()
        {
            Name = "Fe11";
            Image = Graphics.resources.Res.Fe11;
            SetMaxHp(900);
            SetAttackDamage(30);
            SetResist(5);
            SetMovementSpeed(11);
            SetAttackRange(4);
            SetAttackSpeed(2);
            SetMaxEnergy(120);
            SetEnergyRegen(6);

            Stacks = 0;
            WallStacks = 0;

            Rage = new Perk
            {
                Name = "Rage",
                Explanation = (h) => "AD increased by " + RageADBuff*Stacks + " and armor by " 
                     + RageArmorBuff*Stacks + ".",
                Number = (h) => (h as Fe11).Stacks,
                
                GetArmor = (g) => () => g() + Stacks * RageArmorBuff,
                SetArmor = (s) => (v) => s(v - Stacks * RageArmorBuff),
                GetAttackDamage = (g) => () => g() + Stacks * RageADBuff,
                SetAttackDamage = (s) => (v) => s(v - Stacks * RageADBuff),

                SkillFix = (sf) => {
                    if (sf.SkillTypes.Contains(SkillType.Attack))
                    {
                        var newSkill = new Skill()
                        {
                            SkillTypes = sf.SkillTypes,
                            Job = (h) =>
                            {
                                var prev = sf.Job(h);
                                if (prev)
                                    (h as Fe11).Stacks++;
                                return prev;
                            }
                        };
                        
                        return newSkill;
                    }
                    return sf;
                },

                //Stuff from Wall skill
                EndTurn = (i) => (d) =>
                {
                    var h = (d.HeroValue as Fe11) ;
                    var a = i(d);
                    if (h.WallOn)
                    {
                        var wallCost = (WallBaseManaPerTurnCost * Math.Pow(WallTurnManaMultiplier, h.WallStacks));
                        if (h.GetEnergy() < wallCost)
                        {
                            WallControl.Disactivate(h);
                            h.M.Effects.Remove(WallControl);
                            h.WallStacks--;
                        }
                        else
                        {
                            h.AddEnergy(-wallCost);
                            h.WallStacks++;
                        }
                    }
                    else
                    {
                        if (h.WallStacks > 0)
                            h.WallStacks--;
                    }

                    return a;
                },
            };
            Perks.Add(Rage);

            Skills.Remove(Attack);
            BuffedAttack = new Skill
            {
                Name = Attack.Name,
                Explanation = () => Attack.Explanation() + "On attack increases you AD by " + RageADBuff + " and armor by " + RageArmorBuff + ".",
                Job = Attack.Job
            };
            BuffedAttack.SkillTypes.Add(SkillType.Attack);
            Skills.Add(BuffedAttack);

            WallControl = new Effect(this, int.MaxValue)
            {
                Activate = (h) =>
                {
                    (h as Fe11).BuildWall(h.M); 
                },
                Disactivate = (h) =>
                {
                    (h as Fe11).DestroyWall(h.M);
                },
            };

            Wall = new Skill
            {
                Name = "YouShallNotPass",
                Explanation = () => WallOn ? "Destroyes the wall builded by this skill. (current cost " + WallBaseManaPerTurnCost * Math.Pow(WallTurnManaMultiplier, WallStacks) + ")"  
                    : "Builds a wall in " + WallMinDist + "-" + WallMaxDist +
                    " range around you. On next use destroyes it. At the end of turn eats " + WallBaseManaPerTurnCost * Math.Pow(WallTurnManaMultiplier, WallStacks)
                    + " energy and increase its next cost by " +
                    WallTurnManaMultiplier*100 + "% (when disactivated - decrease). energy at the end of turn (if not enough - turnes off).",

                Job = (h) =>
                {
                    if (WallOn)
                    {
                        WallControl.Disactivate(h);
                        h.M.Effects.Remove(WallControl);
                    }
                    else
                    {
                        h.M.Effects.Add(WallControl);
                        WallControl.Activate(h);
                    }

                    return WallOn;
                },
            };
            Wall.SkillTypes.Add(SkillType.Special);
            Skills.Add(Wall);
            
        }

        public void BuildWall(Map m)
        {
            WallOn = true;
            PlacedWalls = m.UnitPositions[this].GetPointsInDistance(WallMinDist, WallMaxDist).Keys
                .Where(po => m.IsInBounds(po) && m.MapTiles[po.X, po.Y].Type != TileType.Solid)
                .ToList();
            foreach (var wall in PlacedWalls)
                m.MapTiles[wall.X, wall.Y].Type = TileType.Solid;
        }

        public void DestroyWall(Map m)
        {
            if (!WallOn) return;
            foreach (var wall in PlacedWalls)
                m.MapTiles[wall.X, wall.Y].Type = TileType.Empty;
            PlacedWalls = new List<Point>();
            WallOn = false;
        }


    }
}
