using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;
using Spelkonstruktionsprojekt.ZEngine.Components;
using ZEngine.Components;
using ZEngine.EventBus;
using ZEngine.Managers;
using static Spelkonstruktionsprojekt.ZEngine.Components.ActionBindings;
using ZEngine.Wrappers;

namespace Spelkonstruktionsprojekt.ZEngine.Systems.InputHandler
{
    class InputHandler : ISystem
    {
        private readonly EventBus EventBus = EventBus.Instance;
        private readonly ComponentManager ComponentManager = ComponentManager.Instance;
        private readonly Action<KeyboardState> _systemAction;

        public InputHandler()
        {
            _systemAction = new Action<KeyboardState>(HandleInput);
        }

        public void HandleInput(KeyboardState oldKeyboardState)
        {
            var keyboardState = Keyboard.GetState();
            var entities = ComponentManager.GetEntitiesWithComponent<ActionBindings>();
            foreach (var entity in entities)
            {
                var keyBindings = entity.Value.Actions;
                foreach (var binding in keyBindings)
                {
                    Keys key = binding.Key;
                    var currentKeyEvent = GetKeyEvent(key, keyboardState, oldKeyboardState);
                    if (CurrentKeyEventNotRelevant(currentKeyEvent, binding)) return;

                    Dictionary<KeyEvent, string> eventActions = binding.Value;
                    EventBus.Publish(eventActions[currentKeyEvent], entity.Key);
                }
            }
        }

        private static bool CurrentKeyEventNotRelevant(KeyEvent currentKeyEvent, KeyValuePair<Keys, Dictionary<KeyEvent, string>> entry)
        {
            return currentKeyEvent == KeyEvent.KeySustain || !entry.Value.ContainsKey(currentKeyEvent);
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
}
