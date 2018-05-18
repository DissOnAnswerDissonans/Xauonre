using MiniXauonre.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Heroes
{
    class Micro : HeroWithBaseSkills
    {
        public const double DefenceRadius = 15;
        public const double DefenceADScale = 0.05;
        public Perk Defence { get; set; }
        public Map Map { get; set; }
        public Player Player { get; set; }

        public const double DropCatchRadius = 5;
        public const double DropRaduis = 20;
        public const double DropCD = 10;
        public const double DropCost = 100;
        public Skill Drop { get; set; }

        public const double RestoreHeal = 0.1;
        public const double RestoreRegen = 0.03;
        public const int RestoreRegenDuration = 2;
        public const double RestoreCooldown = 6;
        public const double RestoreCost = 50;
        public Skill Restore { get; set; }
        public Perk RestoreRegenBuff { get; set; }

        public Micro()
        {
            Name = "Micro";
            SetAttackDamage(70);
            SetMaxHp(1500);
            SetMaxEnergy(200);
            SetEnergyRegen(2);
            SetMovementSpeed(13);
            SetRegen(5);

            Defence = new Perk
            {
                GetArmor = (g) => () => 
                    g() + DefenceADScale * GetEnemiesInRange(P, M, DefenceRadius).Count(),
                SetArmor = (s) => (v) => 
                    s(v - DefenceADScale * GetEnemiesInRange(P, M, DefenceRadius).Count),
                GetResist = (g) => () =>
                    g() + DefenceADScale * GetEnemiesInRange(P, M, DefenceRadius).Count(),
                SetResist = (s) => (v) =>
                    s(v - DefenceADScale * GetEnemiesInRange(P, M, DefenceRadius).Count),
            };
            Perks.Add(Defence);

            Drop = new Skill
            {
                Name = "Drop",
                Explanation = () => "Get any hero in " + DropCatchRadius + " units and throw in any free place in "
                    + DropRaduis + " units from you."
                    + " CD " + DropCD + ". Cost " + DropCost + ".",
                CoolDown = DropCD,
                EnergyCost = DropCost,
                Job = (h) =>
                {
                    var targets = GetHeroesInRange(h.P, h.M, DropCatchRadius).Where(t => t != this).ToList();
                    if (targets.Count == 0)
                        return false;
                    Targets.Add(ChooseTarget(targets, h.P));
                    var points = h.M.UnitPositions[h].GetPointsInDistance(0, DropRaduis)
                        .Where(p => h.M.CellIsFree(p)).ToList();
                    if (points.Count == 0)
                        return false;
                    var point = ChoosePoint(points, h.P);
                    h.M.UnitPositions[Targets[0]] = point;
                    return true;
                }
            };
            Drop.SkillTypes.Add(SkillType.Special);
            Skills.Add(Drop);


            RestoreRegenBuff = new Perk
            {
                GetRegen = (g) => () => g() + RestoreRegen * GetMaxHp(),
                SetRegen = (s) => (v) => s(v - RestoreRegen * GetMaxHp()),
            };

            Restore = new Skill
            {
                Name = "Restore",
                Explanation = () => "Heal you for " 
                + RestoreHeal + "% MaxHp ("+ RestoreHeal * GetMaxHp() +") and give " 
                + RestoreRegen + "% MaxHp ("+RestoreRegen * GetMaxHp()+") Regen for " 
                + RestoreRegenDuration + " turns. CD"
                + RestoreCooldown + ". Cost " + RestoreCost + ".",
                CoolDown = RestoreCooldown,
                EnergyCost = RestoreCost,
                Job = (h) =>
                {
                    var effect = new Effect(h, RestoreRegenDuration)
                    {
                        Activate = (eh) =>
                        {
                            eh.Perks.Add(RestoreRegenBuff);
                        },
                        Disactivate = (eh) =>
                        {
                            eh.Perks.Remove(RestoreRegenBuff);
                        }
                    };
                    effect.Activate(h);
                    h.M.Effects.Add(effect);
                    h.GetHeal(RestoreHeal * GetMaxHp());
                    return true;
                }
            };
            Restore.SkillTypes.Add(SkillType.Special);
            Skills.Add(Restore);



        }
    }
}
