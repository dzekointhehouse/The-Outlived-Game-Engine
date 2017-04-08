using System;
using System.Collections.Generic;
using System.Linq;
using Systems;
using Spelkonstruktionsprojekt.ZEngine.Systems;

namespace Spelkonstruktionsprojekt.ZEngine
{
    public class SystemManager
    {
        private readonly Dictionary<string, ISystem> _systems = new Dictionary<string, ISystem>()
        {
            { "Render", new RenderSystem() }
        };

        public ISystem GetSystem(string systemName)
        {
            if (!_systems.ContainsKey(systemName))
            {
                throw new Exception("No such system.");
            }
            else
            {
                return NewSystem(systemName);
            }
        }
            
        public T GetSystem<T>() where T : new()
        {
            if (ContainsSystem<T>())
            {
                return new T();
            }
            else
            {
                throw new Exception("No such system exist.");
            }
        }

        private ISystem NewSystem(string name)
        {
            return (ISystem) Activator.CreateInstance(_systems[name].GetType());
        }

        private Boolean ContainsSystem<T>()
        {
            return _systems.Count(entry => entry.Value.GetType() == typeof(T)) == 1;
        }
    }
}