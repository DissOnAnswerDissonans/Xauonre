using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Items
{
    class MagicArtifact : Item
    {
        public MagicArtifact()
        {
            Name = "Magic Artifact";
            Explanation = () => "";
            Cost = 700;
            AP = 100;
            HP = 200;
            MS = 1;

            Parts = new List<Item>
            {
                new MagicRelic(),
                new MagicStone(),
            };
        }
    }
}
