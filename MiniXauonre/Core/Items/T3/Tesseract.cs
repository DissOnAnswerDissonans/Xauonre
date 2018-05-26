using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Items
{
    class Tesseract : Item
    {
        public Tesseract()
        {
            Name = "Tesseract";
            Tier = 3;
            
            AP = 343;
            E = 50 + 150;
            HP = 300 + 100;
            ER = 20 + 16;
            CDR = 1;

            Cost = 2020;

            Parts = new List<Item>()
            {
                new Accelerator(),
                new Bablonomicon(),
                new Simplex(),
            };
        }
    }
}