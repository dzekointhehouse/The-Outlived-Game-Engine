using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Spelkonstruktionsprojekt.ZEngine.Components;
using ZEngine.Components;
using ZEngine.EventBus;
using ZEngine.Managers;
using ZEngine.Systems;
using static Spelkonstruktionsprojekt.ZEngine.Components.ActionBindings;
using ZEngine.Wrappers;

namespace Spelkonstruktionsprojekt.ZEngine.Systems.InputHandler
{
    class InputHandler : ISystem
    {
        private readonly EventBus EventBus = EventBus.Instance;
        private readonly ComponentManager ComponentManager = ComponentManager.Instance;

        public void HandleInput(KeyboardState oldKeyboardState, GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();
            var entitiesWithActionBindings = ComponentManager.GetEntitiesWithComponent(typeof(ActionBindings));
            foreach (var entity in entitiesWithActionBindings)
            {
                var actionBindingsComponent = entity.Value as ActionBindings;
                var keyBindings = actionBindingsComponent.Actions;

                foreach (var binding in keyBindings)
                {
                    var key = binding.Key;
                    var eventName = binding.Value;
                    var currentKeyEvent = GetKeyEvent(key, keyboardState, oldKeyboardState);
                    EventBus.Publish(
                        binding.Value,
                        new InputEvent()
                        {
                            EntityId = entity.Key,
                            EventTime = gameTime.TotalGameTime.TotalMilliseconds,
                            KeyEvent = currentKeyEvent,
                            Key = key,
                            EventName = eventName
                        });
                }
            }
        }

        public KeyEvent GetKeyEvent(Keys key, KeyboardState newState, KeyboardState oldState)
        {
            if (IsKeyPress(key, newState, oldState))
            {
                return KeyEvent.KeyPressed;
            }
            if (IsKeyRelease(key, newState, oldState))
            {
                return KeyEvent.KeyReleased;
            }
            if (newState.IsKeyUp(key))
            {
                return KeyEvent.KeyUp;
            }
            if (newState.IsKeyDown(key))
            {
                return KeyEvent.KeyDown;
            }

            return KeyEvent.KeySustain;
        }

        private bool IsKeyPress(Keys key, KeyboardState newState, KeyboardState oldState)
        {
            return newState.IsKeyDown(key) && oldState.IsKeyUp(key);
        }

        private bool IsKeyRelease(Keys key, KeyboardState newState, KeyboardState oldState)
        {
            return newState.IsKeyUp(key) && oldState.IsKeyDown(key);
        }

    }

    public class InputEvent
    {
        public KeyEvent KeyEvent;
        public Keys Key;
        public string EventName;
        public int EntityId;
        public double EventTime;
    }
}
