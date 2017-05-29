using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ZEngine.Components;

namespace ZEngine.Components
{
    public class EventZoneComponent : IComponent
    {
        public List<string> Events { get; set; }
        public HashSet<uint> Inhabitants { get; set; }
        
        public IComponent Reset()
        {
            Events = null;
            return this;
        }
    }
}