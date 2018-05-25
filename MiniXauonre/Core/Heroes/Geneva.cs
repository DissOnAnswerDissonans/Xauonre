using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/*
 * SUPPORT


Geneva
- Heal Beam: Восстанавливает союзному герою (100 + 50%AP)HP в радиусе 15. Geneva продолжает восстанавливать ему 10 + 10%AP HP в конце каждого своего хода,
	если не выполняла других действий и союзник все еще в радиусе способности. Cost 100. CD 10.
- Impulse: Создает в радиусе 20 импульс замедляющий всех противников на 40% в радиусе 7 и наносящий 30%AP магического урона. CD 10. Cost 60.

Hp = 1100
Energy = 300
ER = 5
MS = 8
AD = 30
*/


namespace MiniXauonre.Core.Heroes
{
    class Geneva : HeroWithBaseSkills
    {
        public const double BeamHeal = 100;
        public const double BeamHealApScale = 0.8;
        public const double BeamRadius = 20;
        public const double BeamRegen = 20;
        public const double BeamRegenApScale = 0.4;
        public const double BeamCD = 10;
        public const double BeamCost = 200;
        public Skill Beam { get; protected set; }
        public Perk BeamPerk { get; protected set; }

        public const double ImpulseDamage = 50;
        public const double ImpulseApScale = 0.3;
        public const double ImpulseSlow = 0.3;
        public const double ImpulseSlowTime = 2;
        public const double ImpulseRadius = 5;
        public const double ImpulseRange = 20;
        public const double ImpulseCD = 6;
        public const double ImpulseCost = 60;
        public Skill Impulse { get; protected set; }

        public bool Keeping { get; private set; }
        public Hero BeamTarget { get; private set; }

        public Geneva()
        {
            Name = "Geneva";
            SetMaxHp(1100);
            SetMaxEnergy(300);
            SetMovementSpeed(8);
            SetArmor(15);
            SetEnergyRegen(15);

            Keeping = false;

            Impulse = new Skill()
            {
                Name = "Impulse",
                Explanation = () => "Creates an impulse, centered "
                + ImpulseRange + " units from you, that explodes in "
                + ImpulseRadius + " units radius dealing "
                + ImpulseDamage + " + " + ImpulseApScale * 100 + "%AP ("
                + (ImpulseDamage + ImpulseApScale * GetAbilityPower()) + ") " +
                "magic damage to enemies and slowing them by "
                + ImpulseSlow*100 + "% MS for " + ImpulseSlowTime + " turns. CD "
                + ImpulseCD + ". Cost " + ImpulseCost + ".",
                EnergyCost = ImpulseCost,
                CoolDown = ImpulseCD,
                Job = (h) =>
                {
                    var points = h.M.UnitPositions[h].GetPointsInDistance(0, ImpulseRange).Select(p => p.Key).ToList();
                    if (points.Count == 0)
                        return false;
                    var center = ChoosePoint(points, h.P);
                    if (center == null)
                        return false;
                    var targets = h.M.UnitPositions.Where(u => u.Value.GetStepsTo(center) <= ImpulseRadius)
                        .Select(p => p.Key)
                        .Where(u => h.P != u.P)
                        .ToList();
                    var damage = new Damage(h, h.P, magic: ImpulseDamage + ImpulseApScale * GetAbilityPower());

                    foreach (var t in targets)
                    {
                        h.Targets.Add(t);
                        t.GetDamage(damage);
                    }

                    var slowPerk = new Perk
                    {
                        GetMovementSpeed = (g) => () => g() * (1 - ImpulseSlow),
                        SetMovementSpeed = (g) => (v) => g(v / (1 - ImpulseSlow)),
                    };

                    foreach (var t in targets)
                        t.Perks.Add(slowPerk);
                    var slowEffect = new Effect(h)
                    {
                        Activate = (he) =>
                        {                
                           
                        },
                        Disactivate = (he) =>
                        {
                            foreach (var t in targets)
                                t.Perks.Remove(slowPerk);
                        },
                        Timer = (int)ImpulseSlowTime,
                    };
                    h.M.Effects.Add(slowEffect);
                    slowEffect.Activate(h);
                    (h as Geneva).Keeping = false;
                    return true;
                },
            };
            Impulse.SkillTypes.Add(SkillType.Special);
            Impulse.SkillTypes.Add(SkillType.Mag);
            Skills.Add(Impulse);


            BeamPerk = new Perk
            {
                EndTurn = (f) => (d) =>
                {
                    var me = d.HeroValue as Geneva;
                    if (me.Keeping)
                    {
                        if (me.BeamTarget!=null 
                            && me.M.UnitPositions.ContainsKey(me.BeamTarget)
                            && me.M.UnitPositions[me.BeamTarget].GetStepsTo(me.M.UnitPositions[me]) <= BeamRadius)
                            me.BeamTarget.GetHeal(BeamRegen + BeamRegenApScale * me.GetAbilityPower());
                        else
                            me.Keeping = false;
                    }
                    return f(d);
                },

                SkillFix = (s) =>
                {
                    var prev = s.Job;
                    if (s.Name == "Beam" || s.Name == "Impulse")
                        return s;
                    var newSkill = new Skill
                    {
                        Name = s.Name,
                        Explanation = s.Explanation,
                        CoolDown = s.CoolDown,
                        EnergyCost = s.EnergyCost,
                        SkillTypes = s.SkillTypes,
                        Job = (h) =>
                        {
                            var res = prev(h);
                            if (res)
                                (h as Geneva).Keeping = false;
                            return res;
                        },
                        Timer = s.CoolDown,
                    };
                    return newSkill;
                }
            };
            Perks.Add(BeamPerk);

            Beam = new Skill
            {
                Name = "Beam",
                Explanation = () => "Heales chosen hero in "
                + BeamRadius + " units from you for " 
                + BeamHeal + " + " + BeamHealApScale*100 +"%AP ("
                + (BeamHeal + BeamHealApScale*GetAbilityPower()) + ") HP and "
                + " continues healing him at the end of every your turn fro "+
                + BeamRegen + " + " + BeamRegenApScale*100 + "%AP (" + 
                (BeamRegen + BeamRegenApScale*GetAbilityPower())+ ") HP"+
                " while you dont use any other skills and target stays in range of skill. CD " 
                + BeamCD + ". Cost " + BeamCost+  ".",
                EnergyCost = BeamCost,
                CoolDown = BeamCD,
                Job = (h) =>
                {
                    var allys = GetAlliesInRange(h, BeamRadius);
                    if (allys.Count == 0)
                        return false;
                    var target = ChooseTarget(allys, h.P);
                    if (target == null)
                        return false;
                    h.Targets.Add(target);


                    target.GetHeal(BeamHeal + BeamHealApScale * h.GetAbilityPower());
                    (h as Geneva).Keeping = true;
                    (h as Geneva).BeamTarget = target;
                    return true;
                }
            };
            Beam.SkillTypes.Add(SkillType.Special);
            Skills.Add(Beam);
           
        }
    }
}
