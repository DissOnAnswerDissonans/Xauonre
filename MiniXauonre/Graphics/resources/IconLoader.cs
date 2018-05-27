using System.Collections.Generic;
using System.Drawing;
using MiniXauonre.Core;

namespace MiniXauonre.Graphics.resources
{
    public static class IconLoader
    {
        public static Dictionary<string, Bitmap> GetIcons(Size iconSize)
        {
            return new Dictionary<string, Bitmap>
            {
                {"HP", new Bitmap(Res.Health, iconSize)},
                {"E",  new Bitmap(Res.Energy, iconSize)},
                {"AD", new Bitmap(Res.AttackDamage, iconSize)},
                {"AP", new Bitmap(Res.AbilityPower, iconSize)},
                {"A",  new Bitmap(Res.Armor, iconSize)},                              
                {"R",  new Bitmap(Res.Resist, iconSize)},                             
                {"AR", new Bitmap(Res.AttackRange, iconSize)},                        
                {"AS", new Bitmap(Res.AttackSpeed, iconSize)},                        
                {"MS", new Bitmap(Res.MovementSpeed, iconSize)},                      
                {"CDR",new Bitmap(Res.CDReduction, iconSize)},                        
                {"HR", new Bitmap(Res.HealthRegen, iconSize)},                        
                {"ER", new Bitmap(Res.EnergyRegen, iconSize)},                        
            };                                                                        
        }                                                                             
                                                                                      
        public static Bitmap GetIcon(StatType type, Size iconSize)
        {
            switch (type)
            {
                    case StatType.HP:                 return new Bitmap(Res.Health, iconSize);
                    case StatType.Energy:             return new Bitmap(Res.Energy, iconSize);
                    case StatType.MaxHP:              return new Bitmap(Res.Health, iconSize);  
                    case StatType.MaxEnergy:          return new Bitmap(Res.Energy, iconSize);
                    case StatType.AttackDamage:       return new Bitmap(Res.AttackDamage, iconSize); 
                    case StatType.AbilityPower:       return new Bitmap(Res.AbilityPower, iconSize); 
                    case StatType.Armor:              return new Bitmap(Res.Armor, iconSize); 
                    case StatType.Resist:             return new Bitmap(Res.Resist, iconSize); 
                    case StatType.AttackRange:        return new Bitmap(Res.AttackRange, iconSize); 
                    case StatType.AttackSpeed:        return new Bitmap(Res.AttackSpeed, iconSize); 
                    case StatType.MovementSpeed:      return new Bitmap(Res.MovementSpeed, iconSize);
                    case StatType.CooldownReduction:  return new Bitmap(Res.CDReduction, iconSize); 
                    case StatType.Regen:              return new Bitmap(Res.HealthRegen, iconSize); 
                    case StatType.EnergyRegen:        return new Bitmap(Res.EnergyRegen, iconSize); 
                    default: return new Bitmap(Res.noyhing, iconSize);
            }
        }
    }
}