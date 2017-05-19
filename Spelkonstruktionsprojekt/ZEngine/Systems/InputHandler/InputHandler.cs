using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Components.Input;
using Spelkonstruktionsprojekt.ZEngine.Constants;
using Spelkonstruktionsprojekt.ZEngine.Managers;
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

        private static Dictionary<uint, GamePadState> oldGamePadState = new Dictionary<uint, GamePadState>();

        public void HandleGamePadInput(GameTime gameTime)
        {
            foreach (var entity in ComponentManager.GetEntitiesWithComponent(typeof(GamePadComponent)))
            {
                var gamePadComponent = entity.Value as GamePadComponent;
                if (gamePadComponent == null || gamePadComponent.GamePadPlayerIndex < 0 ||
                    gamePadComponent.GamePadPlayerIndex > 3)
                {
                    Debug.WriteLine("INVALID GAME PAD STATE FOR ENTITY: " + entity.Key);
                    return;
                }

                var gamePadState = GamePad.GetState(gamePadComponent.GamePadPlayerIndex);
                if (!oldGamePadState.ContainsKey(entity.Key))
                {
                    oldGamePadState[entity.Key] = gamePadState;
                }

                var layout = GamePadStandardLayout.Layout;
                foreach (var binding in layout)
                {
                    var button = binding.Key;
                    var eventName = binding.Value;
                    gamePadState.IsButtonDown(button);
                    var currentKeyEvent = GetKeyEvent(button, gamePadState, oldGamePadState[entity.Key]);
                    if (!GamePadStandardLayout.GamePadToKeyboardMap.ContainsKey(button)) return;
                    EventBus.Publish(
                        binding.Value,
                        new InputEvent
                        {
                            EntityId = entity.Key,
                            EventTime = gameTime.TotalGameTime.TotalMilliseconds,
                            KeyEvent = currentKeyEvent,
                            Key = GamePadStandardLayout.GamePadToKeyboardMap[button],
                            EventName = eventName
                        });
                }

                oldGamePadState[entity.Key] = gamePadState;
            }
        }

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
                        new InputEvent
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

        public KeyEvent GetKeyEvent(Buttons button, GamePadState newState, GamePadState oldState)
        {
            if (newState.IsButtonDown(button))
            {
                if (oldState.IsButtonUp(button))
                {
                    return KeyEvent.KeyPressed;
                }
                return KeyEvent.KeyDown;
            }
            if (newState.IsButtonUp(button))
            {
                if (oldState.IsButtonDown(button))
                {
                    return KeyEvent.KeyReleased;
                }
                return KeyEvent.KeyUp;
            }
            return KeyEvent.KeySustain;
        }
    }

    public class InputEvent
    {
        public KeyEvent KeyEvent;
        public Keys Key;
        public string EventName;
        public uint EntityId;
        public double EventTime;
    }

    public class GamePadEvent
    {
        public KeyEvent KeyEvent;
        public int GamePadIndex;
        public Buttons Button;
        public string EventName;
        public int EntityId;
        public double EventTime;
    }
}