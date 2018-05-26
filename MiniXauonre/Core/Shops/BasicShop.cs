﻿using MiniXauonre.Core.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Shops
{
    class BasicShop : Shop
    {
        public static List<Item> set = new List<Item>
        {
            new MagicStone(),
            new Razor(),
            new RestoreRing(),
            new SpellBook(),
            new Amulet(),
            new Steel(),
            new FlameCoast(),       
            new Battery(),

            new Shield(),
            new MagicRelic(),
            new Bablonomicon(),
            new XPeke(),
            new Accelerator(),
            new Bulker(), 
            new Knife(),
            new Blade(),
            new Carapace(),
            new Leaven(),
            new Accumulator(),
            new Booster(),
            new Boots(),
            
            new MagicBoots(),
            new FlashBoots(),
            new KillerBoots(),
            new UsefulBoots(),
            new MagicCrystal(),
            new MagicWand(),
            new KingSword(),
            new HyperShell(),
            new DeathScythe(),
            new NanoArmor(),
            new Leach(),
            
            new TimeMachine(),
            new Items.Buffer(),
            new GiantArmor(),
            new Minigun(),
            new InfinityEdge(),
        };


        public BasicShop()
        {
            Items = set;
            foreach (var item in Items)
            {
                item.BindStats();
            }
        }
    }
}
