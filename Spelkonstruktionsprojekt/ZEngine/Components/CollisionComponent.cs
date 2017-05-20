using System.Collections.Generic;

namespace ZEngine.Components
{
    public class CollisionComponent : IComponent
    {
        public bool IsCage;

        //each int is an entityId
        public List<uint> collisions = new List<uint>();

        public CollisionComponent()
        {
            Reset();
        }

        public IComponent Reset()
        {
            IsCage = false;
            collisions.Clear();
            return this;
        }
    }
}
