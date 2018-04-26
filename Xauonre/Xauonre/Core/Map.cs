using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xauonre.Core
{
    class Map : IMap
    {
        protected Dictionary<int, IEffect> Effects;
        public Dictionary<Point, Tile> mapTiles;
        public Dictionary<int, IEntity> Entities;
        private int nextId = -1;
        private int nextEId = -1;

        public int AddEffect(IEffect e)
        {
            Effects.Add(GetEId(), e);
            e.SetId(nextEId);
            return nextEId;
        }

        public int AddEntity(IEntity e, Point p)
        {
            Entities.Add(GetId(), e);
            mapTiles[p].idsOfEntities.Add(nextId);
            return nextId;
        }

        private int GetEId() => ++nextEId;
        private int GetId() => ++nextId;

        public List<IEntity> GetEntities() => Entities.Values.ToList();

        public void Tick(HashSet<int> creators)
        {
            foreach (var ef in Effects.Values)
                ef.Work(creators)(this);
        }
    }
}
