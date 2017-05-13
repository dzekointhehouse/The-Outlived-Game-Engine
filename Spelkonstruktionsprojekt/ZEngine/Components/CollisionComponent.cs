using System.Collections.Generic;

namespace ZEngine.Components
{
    public class CollisionComponent : IComponent
    {
        public bool IsCage = false;

        //each int is an entityId
        public List<int> collisions = new List<int>();
    }
}
