using System;
using System.Collections.Generic;
using System.Linq;
using Spelkonstruktionsprojekt.ZEngine;
using ZEngine.Components;

namespace ZEngine.Managers
{
    public class ComponenetsManager
    {
        private readonly Dictionary<string, Component> _components = new Dictionary<string, Component>()
        {
            { "Position", new PositionComponent() }
        };

        public ISystem GetComponent(string componentName)
        {
            if (!_components.ContainsKey(componentName))
            {
                throw new Exception("No such component.");
            }
            else
            {
                return NewComponent(componentName);
            }
        }

        public T GetComponent<T>() where T : new()
        {
            if (ContainsComponent<T>())
            {
                return new T();
            }
            else
            {
                throw new Exception("No such component exist.");
            }
        }

        private ISystem NewComponent(string name)
        {
            return (ISystem)Activator.CreateInstance(_components[name].GetType());
        }

        private Boolean ContainsComponent<T>()
        {
            return _components.Count(entry => entry.Value.GetType() == typeof(T)) == 1;
        }
    }
}
