using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xauonre.Core
{
    interface IEffect
    {
        void SetId(int id);
        int GetId();
        Action<Map> Work(HashSet<int> creators);
    }
}
