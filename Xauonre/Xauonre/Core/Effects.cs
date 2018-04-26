using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xauonre.Core
{
    class EmptyEffect : IEffect
    {
        public int timer;
        public int Creator;
        private int id = -1;

        public EmptyEffect(int creator)
        {
            Creator = creator;
            timer = 0;
        }

        public int GetId() => id;

        public void SetId(int id) => this.id = id;

        public Action<Map> Work(HashSet<int> creators)
        {
            if (creators.Contains(Creator))
            {
                if (timer == 0)
                    return Job;
            }
            return Nothing;
        }

        protected void Nothing(Map map) { }
        protected void Job(Map map) { }
    }
}
