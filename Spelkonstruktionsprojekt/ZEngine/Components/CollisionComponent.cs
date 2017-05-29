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

        public CollisionComponent()
        {
            Reset();
        }

        public IComponent Reset()
        {
            IsCage = false;
            Collisions.Clear();
            Zones.Clear();
            return this;
        }
    }
}
