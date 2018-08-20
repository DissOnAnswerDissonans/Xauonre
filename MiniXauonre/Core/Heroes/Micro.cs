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
        public const double DropCD = 7;
        public const double DropCost = 300;
        public Skill Drop { get; set; }

        public const double RestoreHeal = 0.1;
        public const double RestoreRegen = 0.03;
        public const int RestoreRegenDuration = 2;
        public const double RestoreCooldown = 6;
        public const double RestoreCost = 250;
        public Skill Restore { get; set; }
        public Perk RestoreRegenBuff { get; set; }
        
        

        public Micro()
        {
            Name = "Micro";
            Image = Graphics.resources.Res.Micro;
            SetAttackDamage(75);
            SetMaxHp(1100);
            SetMaxEnergy(350);
            SetEnergyRegen(10);
            SetMovementSpeed(11);
            SetRegen(15);

            Defence = new Perk
            {
                Name = "Defence",
                Number = (h) => (h as Micro).GetEnemiesInRange(h, DefenceRadius).Count,
                Explanation = (h) => "Gets " + (DefenceADScale * 100) + "% AD (" + DefenceADScale * h.GetAttackDamage() 
                   +") armor and resist from every enemy in " + DefenceRadius + "units (" 
                        + (h as Micro).GetEnemiesInRange(h, DefenceRadius).Count + " enemies), total " 
                           + (h.GetAttackDamage() * DefenceADScale * GetEnemiesInRange(this, DefenceRadius).Count),
  
                GetArmor = (g) => () => 
                    g() + GetAttackDamage() * DefenceADScale * GetEnemiesInRange(this, DefenceRadius).Count,
                SetArmor = (s) => (v) => 
                    s(v - GetAttackDamage() * DefenceADScale * GetEnemiesInRange(this, DefenceRadius).Count),
                GetResist = (g) => () =>
                    g() + GetAttackDamage() * DefenceADScale * GetEnemiesInRange(this, DefenceRadius).Count,
                SetResist = (s) => (v) =>
                    s(v - GetAttackDamage() * DefenceADScale * GetEnemiesInRange(this, DefenceRadius).Count),
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
                    var targets = GetHeroesInRange(h, DropCatchRadius).Where(t => t != h).ToList();
                        if (targets.Count == 0) return false;
                    var target = ChooseTarget(targets, h.P);
                        if (target == null) return false;
                    h.Targets.Add(target);
                    var points = h.M.UnitPositions[h].GetPointsInDistance(0, DropRaduis).Keys
                        .Where(p => h.M.CellIsFree(p)).ToList();
                        if (points.Count == 0) return false;
                    var point = ChoosePoint(points, h.P);
                        if (point == null) return false;
                    h.M.UnitPositions[Targets[0]] = point;
                        return true;
                }
            };
            Drop.SkillTypes.Add(SkillType.Special);
            Skills.Add(Drop);


            RestoreRegenBuff = new Perk
            {
                Name = "Restore",
                Number = (h) => Math.Floor(RestoreRegen * h.GetMaxHp()),
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
                    h.GetHeal(RestoreHeal * h.GetMaxHp());
                    return true;
                }
            };
            Restore.SkillTypes.Add(SkillType.Special);
            Skills.Add(Restore);



        }
    }
}
