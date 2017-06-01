using System.Collections.Generic;
using ZEngine.Components;

namespace Spelkonstruktionsprojekt.ZEngine.Components
{
    public class EntityDestructionComponent : IComponent
    {
        public List<uint> EntitiesToDestroy = new List<uint>();
        
        public IComponent Reset()
        {
            EntitiesToDestroy.Clear();
            return this;
        }
    }
}