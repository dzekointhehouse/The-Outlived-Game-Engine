using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Microsoft.Xna.Framework.Input;

namespace Game.Services
{
    public class VirtualGamePad
    {
        private bool Initialized = false;

        private KeyboardState oldKeyboardState;
        private KeyboardState currentKeyboardState;

        private GamePadState[] oldGamePadState = new GamePadState[4];
        private GamePadState[] currentGamePadState = new GamePadState[4];

        //When true allows keyboard to control input
        public bool IsKeyboardControlled { get; set; } = true;
        public bool IsGamePadControlled { get; set; } = true;

        //Index of current gamepad in control
        public int GamePadPlayerIndex { get; set; } = 0;

        public VirtualGamePad(int gamePadPlayerIndex, bool isKeyboardControlled = false)
        {
            IsKeyboardControlled = isKeyboardControlled;
            GamePadPlayerIndex = gamePadPlayerIndex;
        }

        public void UpdateKeyboardState()
        {
            currentKeyboardState = Keyboard.GetState();
            UpdateGameStates(currentGamePadState);
            if (!Initialized)
            {
                oldKeyboardState = Keyboard.GetState();
                UpdateGameStates(oldGamePadState);
                Initialized = true;
            }
        }

        public void MoveCurrentStatesToOld()
        {
            oldKeyboardState = currentKeyboardState;
            for (var i = 0; i < currentGamePadState.Length; i++)
            {
                oldGamePadState[i] = currentGamePadState[i];
            }
        }

        private void UpdateGameStates(GamePadState[] gamePadState)
        {
            for (var i = 0; i < gamePadState.Length; i++)
            {
                gamePadState[i] = GamePad.GetState(i); 
            }
        }

        public bool Is(MenuKeys key, MenuKeyStates state)
        {
            if (IsKeyboardControlled)
            {
                if (GetKeyboardKeyState(key) == state)
                    return true;
            }
            if (IsGamePadControlled)
            {
                if (GetGamePadButtonState(key) == state)
                    return true;
            }
            return false;
        }

        public MenuKeyStates GetKeyboardKeyState(MenuKeys key)
        {
            if (IsKeyboardPress(key, currentKeyboardState, oldKeyboardState))
            {
                return MenuKeyStates.Pressed;
            }
            if (IsKeyboardRelease(key, currentKeyboardState, oldKeyboardState))
            {
                return MenuKeyStates.Released;
            }
            return MenuKeyStates.Sustain;
        }

        public MenuKeyStates GetGamePadButtonState(MenuKeys key)
        {
            if (IsGamePadPress(key, currentGamePadState[GamePadPlayerIndex], oldGamePadState[GamePadPlayerIndex]))
            {
                return MenuKeyStates.Pressed;
            }
            if (IsGamePadRelease(key, currentGamePadState[GamePadPlayerIndex], oldGamePadState[GamePadPlayerIndex]))
            {
                return MenuKeyStates.Released;
            }
            return MenuKeyStates.Sustain;
        }

        private bool IsGamePadPress(MenuKeys key, GamePadState newState, GamePadState oldState)
        {
            if (!GamePadFromMenuKeys.ContainsKey(key)) return false;
            var button = GamePadFromMenuKeys[key];
            return newState.IsButtonUp(button) && oldState.IsButtonDown(button);
        }

        private bool IsGamePadRelease(MenuKeys key, GamePadState newState, GamePadState oldState)
        {
            if (!GamePadFromMenuKeys.ContainsKey(key)) return false;
            var button = GamePadFromMenuKeys[key];
            return newState.IsButtonUp(button) && oldState.IsButtonDown(button);
        }

        private bool IsKeyboardPress(MenuKeys key, KeyboardState newState, KeyboardState oldState)
        {
            if (!KeysFromMenuKeys.ContainsKey(key)) return false;
            var keyboardKey = KeysFromMenuKeys[key];
            var isKeyDown = newState.IsKeyDown(keyboardKey);
            var isKeyUp = oldState.IsKeyUp(keyboardKey);
            return isKeyDown && isKeyUp;
        }

        private bool IsKeyboardRelease(MenuKeys key, KeyboardState newState, KeyboardState oldState)
        {
            if (!KeysFromMenuKeys.ContainsKey(key)) return false;
            var keyboardKey = KeysFromMenuKeys[key];
            return newState.IsKeyUp(keyboardKey) && oldState.IsKeyDown(keyboardKey);
        }

        public enum MenuKeyStates
        {
            Pressed,
            Released,
            Sustain
        }

        public enum MenuKeys
        {
            Up,
            Down,
            Left,
            Right,
            Accept,
            Cancel,
            Pause
        }

        private static readonly Dictionary<MenuKeys, Keys> KeysFromMenuKeys = new Dictionary<MenuKeys, Keys>
        {
            {MenuKeys.Up, Keys.Up},
            {MenuKeys.Down, Keys.Down},
            {MenuKeys.Left, Keys.Left},
            {MenuKeys.Right, Keys.Right},
            {MenuKeys.Pause, Keys.Escape},
            {MenuKeys.Cancel, Keys.Back},
            {MenuKeys.Accept, Keys.Enter}
        };

        private static readonly Dictionary<MenuKeys, Buttons> GamePadFromMenuKeys = new Dictionary<MenuKeys, Buttons>
        {
            {MenuKeys.Up, Buttons.DPadUp},
            {MenuKeys.Down, Buttons.DPadDown},
            {MenuKeys.Left, Buttons.DPadLeft},
            {MenuKeys.Right, Buttons.DPadRight},
            {MenuKeys.Accept, Buttons.A},
            {MenuKeys.Cancel, Buttons.B},
            {MenuKeys.Pause, Buttons.Start}
        };
    }
}