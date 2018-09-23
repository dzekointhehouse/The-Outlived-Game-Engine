using System;
using System.Collections.Generic;
using ZEngine.Components;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;

namespace Spelkonstruktionsprojekt.ZEngine.Helpers
{
    public class ComponentFactory
    {
        private readonly Dictionary<Type, Stack<IComponent>> _scrapyard = new Dictionary<Type, Stack<IComponent>>();

        public void Reset()
        {
            _scrapyard.Clear();
        }

        public void Recycle(IComponent component)
        {
            if (!_scrapyard.ContainsKey(component.GetType())) return;
            _scrapyard[component.GetType()].Push(component);
        }

        public void Recycle(Type componentType, IComponent component)
        {
            if (component.GetType() == componentType)
            {
                if (_scrapyard.ContainsKey(componentType))
                {
                    _scrapyard[componentType].Push(component);
                }
            }
        }

        public T NewComponent<T>() where T : IComponent, new()
        {
            Stack<IComponent> components;
            var status = _scrapyard.TryGetValue(typeof(T), out components);
            if (status && components.Count >= 1)
            {
                return (T) components.Pop().Reset();
            }

            var newComponent = new T();
            if (!status)
            {
                var newStack = new Stack<IComponent>();
                _scrapyard[typeof(T)] = newStack;
            }
            return newComponent;
        }

        public SpriteComponent NewComponentFromLoadedSprite(Texture2D sprite, string spriteName)
        {
            Stack<IComponent> components;
            var status = _scrapyard.TryGetValue(typeof(SpriteComponent), out components);
            SpriteComponent spriteComponent;
            if (status && components.Count > 0)
            {
                spriteComponent = (components.Pop().Reset() as SpriteComponent);
            }
            else
            {
                spriteComponent = new SpriteComponent();
            }

            spriteComponent.Sprite = sprite;
            spriteComponent.SpriteName = spriteName;
            spriteComponent.SpriteIsLoaded = true;

            if (!status)
            {
                var newStack = new Stack<IComponent>();
                _scrapyard[typeof(SpriteComponent)] = newStack;
            }

            return spriteComponent;
        }
    }
}