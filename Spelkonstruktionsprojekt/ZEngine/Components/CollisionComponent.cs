using System;
using System.Collections.Generic;

namespace ZEngine.Components
{
    public class CollisionComponent : IComponent
    {
        public bool IsCage;

        //each int is an entityId
        public HashSet<uint> Collisions { get; set; } = new HashSet<uint>();
        //each int is a zone
        public HashSet<uint> Zones { get; set; }= new HashSet<uint>();
        //each tuple is an entity and the distance to that entity
        public HashSet<Tuple<uint, double>> CloseEncounters { get; set; } = new HashSet<Tuple<uint, double>>();
        
        public CollisionComponent()
        {
            Reset();
        }

        public IComponent Reset()
        {
            IsCage = false;
            Collisions.Clear();
            Zones.Clear();
            CloseEncounters.Clear();
            return this;
        }
    }
}
