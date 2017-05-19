using System;
using System.Collections.Generic;
using ZEngine.Components;

namespace Spelkonstruktionsprojekt.ZEngine.Helpers
{
    public class ComponentFactory
    {
        private readonly Dictionary<Type, Stack<IComponent>> _scrapyard = new Dictionary<Type, Stack<IComponent>>();

        public void Recycle<T>(IComponent component)
        {
            if (component.GetType() == typeof(T))
            {
                _scrapyard[typeof(T)].Push(component);
            }
        }

        public T NewComponent<T>() where T : IComponent, new()
        {
            Stack<IComponent> components;
            var status = _scrapyard.TryGetValue(typeof(T), out components);
            if (status && components.Count >= 1)
            {
                return (T) components.Pop();
            }

            var newComponent = new T();
            if (!status)
            {
                var newStack = new Stack<IComponent>();
                newStack.Push(newComponent);
                _scrapyard[typeof(T)] = newStack;
            }
            else
            {
                _scrapyard[typeof(T)].Push(newComponent);
            }
            return newComponent;
        }
    }
}