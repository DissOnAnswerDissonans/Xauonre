using System.Collections.Generic;
using System.Drawing;

namespace MiniXauonre.Graphics.resources
{
    public static class IconLoader
    {
        public static Dictionary<string, Bitmap> GetIcons(Size iconSize)
        {
            return new Dictionary<string, Bitmap>
            {
                {"HP", new Bitmap(Res.Health, iconSize)},
                {"E", new Bitmap(Res.Energy, iconSize)},
                {"AD", new Bitmap(Res.AttackDamage, iconSize)},
                {"AP", new Bitmap(Res.AbilityPower, iconSize)},
                {"A", new Bitmap(Res.Armor, iconSize)},
                {"R", new Bitmap(Res.Resist, iconSize)},
                {"AR", new Bitmap(Res.AttackRange, iconSize)},
                {"AS", new Bitmap(Res.AttackSpeed, iconSize)},
                {"MS", new Bitmap(Res.MovementSpeed, iconSize)},
                {"CDR", new Bitmap(Res.CDReduction, iconSize)},
                {"HR", new Bitmap(Res.HealthRegen, iconSize)},
                {"ER", new Bitmap(Res.EnergyRegen, iconSize)},
            };
        }
    }
}