using System;
using System.Collections.Generic;
using System.Linq;
using Spelkonstruktionsprojekt.ZEngine.Systems.InputHandler;
using ZEngine.Systems;

namespace ZEngine.Managers
{
    public class SystemManager
    {
        public static SystemManager Instance => LazyInitializer.Value;
        private static readonly Lazy<SystemManager> LazyInitializer = new Lazy<SystemManager>(() => new SystemManager());

        private readonly Dictionary<string, ISystem> _systems = new Dictionary<string, ISystem>()
        {
            { "Render", new RenderSystem() },
            { "LoadContent", new LoadContentSystem() },
            { "HandleInput", new InputHandler() }
        };

        public ISystem CreateSystem(string systemName)
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
            
        public T CreateSystem<T>() where T : new()
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