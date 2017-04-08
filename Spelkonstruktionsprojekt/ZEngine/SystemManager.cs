using System;
using System.Collections.Generic;
using System.Linq;

namespace Systems
{
    public class SystemManager
    {
        private readonly Dictionary<string, Type> _systems = new Dictionary<string, Type>()
        {
            { "Render", typeof(RenderSystem) }
        };

        public Type GetSystem(string systemName)
        {
            if (!_systems.ContainsKey(systemName))
            {
                throw new Exception("No such system.");
            }
            else
            {
                return _systems[systemName];
            }
        }
            
        public T GetSystem<T>() where T : new()
        {
            if (this._systems.ContainsValue(typeof(T)))
            {
                Type type = this._systems.First(entry => entry.Value == typeof(T)).Value;
                return (T) Activator.CreateInstance(type);
            }
            else
            {
                throw new Exception("No such system exist.");
            }
        }
    }
}