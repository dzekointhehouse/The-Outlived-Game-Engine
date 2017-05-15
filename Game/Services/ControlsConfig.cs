using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Game.Services
{
    /// <summary>
    /// This class is intented to be used when the same
    /// control configuration is intented to be used in different places,
    /// now we don't need to make redundant code for every menu state.
    /// </summary>
    class ControlsConfig
    {
        // MinLimit and MaxLimit are the state interval limits.
        public int MinLimit { get; set; } = 0;
        public int MaxLimit { get; set; } = 0;

        // we need a reference to the gm to change states.
        private readonly GameManager gameManager;

        // Specify the intervals oft arrow controlls and the game manager.
        public ControlsConfig(int minLimit, int maxLimit, GameManager gameManager)
        {
            // interval for arrows
            this.MinLimit = minLimit;
            this.MaxLimit = maxLimit;
            this.gameManager = gameManager;
        }

        public ControlsConfig(GameManager gameManager)
        {
            this.gameManager = gameManager;
        }

        // This can be used as a general button configuration for
        // arrows. The menu using this method needs to have the current position
        // which is inserted as parameter, and then a check is done to see if
        // the player hass pressed up or down arrow then the position
        // is moved accordingly, or kept the same.
        public int MoveOptionPositionVertically(int currentPosition, PlayerIndex player = 0)
        {
            // get the newest state
            KeyboardState newState = Keyboard.GetState();
            GamePadState gamePadState = GamePad.GetState(player);

            if (gamePadState.DPad.Up == ButtonState.Pressed && gameManager.OldGamepadState.IsButtonUp(Buttons.DPadUp)
                || newState.IsKeyDown(Keys.Up) && gameManager.OldKeyboardState.IsKeyUp(Keys.Up))
            {
                gameManager.OldKeyboardState = newState;
                gameManager.OldGamepadState = gamePadState;

                if (currentPosition != MinLimit)
                    return currentPosition - 1;


            }
            if (gamePadState.DPad.Down == ButtonState.Pressed && gameManager.OldGamepadState.IsButtonUp(Buttons.DPadDown)
                || newState.IsKeyDown(Keys.Down) && gameManager.OldKeyboardState.IsKeyUp(Keys.Down))
            {
                gameManager.OldKeyboardState = newState;
                gameManager.OldGamepadState = gamePadState;

                if (currentPosition != MaxLimit)
                    return currentPosition + 1;
            }
            return currentPosition;
        }

        public int MoveOptionPositionHorizontally(int currentPosition, PlayerIndex player = 0)
        {
            // Get the newest state
            KeyboardState newState = Keyboard.GetState();
            GamePadState gamePadState = GamePad.GetState(player);

            if (gamePadState.DPad.Left == ButtonState.Pressed && gameManager.OldGamepadState.IsButtonUp(Buttons.DPadLeft)
                || newState.IsKeyDown(Keys.Left) && gameManager.OldKeyboardState.IsKeyUp(Keys.Left))
            {
                gameManager.OldKeyboardState = newState;
                gameManager.OldGamepadState = gamePadState;

                if (currentPosition != MinLimit)
                    return currentPosition - 1;
            }
            if (gamePadState.DPad.Right == ButtonState.Pressed && gameManager.OldGamepadState.IsButtonUp(Buttons.DPadRight)
                || newState.IsKeyDown(Keys.Right) && gameManager.OldKeyboardState.IsKeyUp(Keys.Right))
            {
                gameManager.OldKeyboardState = newState;
                gameManager.OldGamepadState = gamePadState;

                if (currentPosition != MaxLimit)
                    return currentPosition + 1;
            }


            return currentPosition;
        }

        public bool ContinueButton(GameManager.GameState state, PlayerIndex player = 0)
        {
            // Get the newest state
            KeyboardState keyboardState = Keyboard.GetState();
            GamePadState gamePadState = GamePad.GetState(player);
            bool clicked = false;

            // With this button we want to continue to the next phase of the game initialization
            if (gamePadState.Buttons.A == ButtonState.Pressed && gameManager.OldGamepadState.IsButtonUp(Buttons.A)
                || keyboardState.IsKeyDown(Keys.Enter) && gameManager.OldKeyboardState.IsKeyUp(Keys.Enter))
            {
                gameManager.PreviousGameState = gameManager.CurrentGameState;
                gameManager.CurrentGameState = state;
                clicked = true;
            } 
            gameManager.OldGamepadState = gamePadState;
            gameManager.OldKeyboardState = keyboardState;
            return clicked;
        }

        public void GoBackButton(PlayerIndex player = 0)
        {
            // get the newest state
            KeyboardState newState = Keyboard.GetState();
            GamePadState gamePadState = GamePad.GetState(player);


            // With this button we want to continue to the next phase of the game initialization
            if (GamePad.GetState(player).Buttons.B == ButtonState.Pressed && gameManager.OldGamepadState.IsButtonUp(Buttons.B)
                || newState.IsKeyDown(Keys.Back) && gameManager.OldKeyboardState.IsKeyUp(Keys.Back))
            {          
                gameManager.CurrentGameState = gameManager.PreviousGameState;
            }
            gameManager.OldKeyboardState = newState;
            gameManager.OldGamepadState = gamePadState;

        }

        public void PauseButton(PlayerIndex player = 0)
        {
            // get the newest state
            KeyboardState newState = Keyboard.GetState();
            GamePadState gamePadState = GamePad.GetState(player);


            // With this button we want to continue to the next phase of the game initialization
            if (gamePadState.Buttons.Start == ButtonState.Pressed && gameManager.OldGamepadState.IsButtonUp(Buttons.Start)
                || newState.IsKeyDown(Keys.Escape) && gameManager.OldKeyboardState.IsKeyUp(Keys.Escape))
            {
                if (gameManager.CurrentGameState != GameManager.GameState.Paused)
                {
                    gameManager.PreviousGameState = gameManager.CurrentGameState;
                    gameManager.CurrentGameState = GameManager.GameState.Paused;

                }
                else
                {
                    gameManager.CurrentGameState = gameManager.PreviousGameState;
                }
            }
            gameManager.OldKeyboardState = newState;
            gameManager.OldGamepadState = gamePadState;

        }

    }
}
