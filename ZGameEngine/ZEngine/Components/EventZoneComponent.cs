using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ZEngine.Components;

namespace ZEngine.Components
{
    public class EventZoneComponent : IComponent
    {
        public List<string> Events { get; set; }
        public HashSet<uint> Inhabitants { get; set; }
        public List<uint> NewInhabitants { get; set; } = new List<uint>();
        
        public IComponent Reset()
        {
            Events = null;
            Inhabitants.Clear();
            NewInhabitants.Clear();
            return this;
        }
    }
}