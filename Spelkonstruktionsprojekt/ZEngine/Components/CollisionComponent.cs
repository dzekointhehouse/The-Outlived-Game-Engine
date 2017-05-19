using System.Collections.Generic;

namespace ZEngine.Components
{
    public class CollisionComponent : IComponent
    {
        public bool IsCage = false;

        //each int is an entityId
        public List<uint> collisions = new List<uint>();
    }
}
