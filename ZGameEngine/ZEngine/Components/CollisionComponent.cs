using System.Collections.Generic;
using ConcurrentCollections;
using Spelkonstruktionsprojekt.ZEngine.Wrappers;
using Spelkonstruktionsprojekt.ZEngine.Wrappers.CollisionShapes;

namespace ZEngine.Components
{
    public class CollisionComponent : IComponent
    {
        public bool Disabled { get; set; } = false;
        public bool IsCage;
        public CollisionShape BoundingShape { get; set; } = new BoundingRectangle();


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
