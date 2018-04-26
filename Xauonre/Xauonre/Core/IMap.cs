using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xauonre.Core
{
    interface IMap
    {
        List<IEntity> GetEntities();
        int AddEntity(IEntity e, Point p);
        void Tick(HashSet<int> creators);
        int AddEffect(IEffect e);
    }
}
