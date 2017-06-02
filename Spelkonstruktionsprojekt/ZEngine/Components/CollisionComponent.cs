using System;
using System.Collections.Generic;
using ConcurrentCollections;

namespace ZEngine.Components
{
    public class CollisionComponent : IComponent
    {
        public bool Disabled { get; set; } = false;
        public bool IsCage;

        //each int is an entityId
//        public HashSet<uint> Collisions { get; set; } = new HashSet<uint>();
        public ConcurrentHashSet<uint> Collisions { get; set; } = new ConcurrentHashSet<uint>();
        //each int is a zone
        public HashSet<uint> Zones { get; set; }= new HashSet<uint>();
        public CollisionComponent()
        {
            Reset();
        }

        public IComponent Reset()
        {
            Disabled = false;
            IsCage = false;
            Collisions.Clear();
            Zones.Clear();
            return this;
        }
    }
}
