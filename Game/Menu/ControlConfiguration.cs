using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ZEngine.Wrappers;

namespace Game.Menu
{
    /// <summary>
    /// This class is intented to be used when the same
    /// control configuration is intented to be used in different places,
    /// now we don't need to make redundant code for every menu state.
    /// </summary>
    class ControlConfiguration
    {
        private readonly int min = 0, max = 0;
        private readonly GameManager gameManager;

        // Specify the intervals oft arrow controlls and the game manager.
        public ControlConfiguration(int min, int max, GameManager gameManager)
        {
            // interval for arrows
            this.min = min;
            this.max = max;
            this.gameManager = gameManager;
        }

        public ControlConfiguration(GameManager gameManager)
        {
            this.gameManager = gameManager;
        }

        // This can be used as a general button configuration for
        // arrows. The menu using this method needs to have the current position
        // which is inserted as parameter, and then a check is done to see if
        // the player hass pressed up or down arrow then the position
        // is moved accordingly, or kept the same.
        public int GetMenuOptionPosition(int currentPosition)
        {
            // get the newest state
            KeyboardState newState = Keyboard.GetState();

            
            if (GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed
                || newState.IsKeyDown(Keys.Up) && gameManager.OldState.IsKeyUp(Keys.Up))
            {
                gameManager.OldState = newState;
                if (currentPosition != min)
                    return currentPosition = currentPosition - 1;


            }
            if (GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed
                || newState.IsKeyDown(Keys.Down) && gameManager.OldState.IsKeyUp(Keys.Down))
            {
                gameManager.OldState = newState;
                if (currentPosition != max)
                    return currentPosition = currentPosition + 1;
            }
            return currentPosition;
        }

        public void ContinueButton(GameManager.GameState state)
        {
            // get the newest state
            KeyboardState newState = Keyboard.GetState();

            // With this button we want to continue to the next phase of the game initialization
            if (GamePad.GetState(PlayerIndex.One).Buttons.X == ButtonState.Pressed
                || newState.IsKeyDown(Keys.Enter) && gameManager.OldState.IsKeyUp(Keys.Enter))
            {
                gameManager.PreviousGameState = gameManager.CurrentGameState;
                gameManager.CurrentGameState = state;
            }
            gameManager.OldState = newState;
        }

        public void GoBackButton()
        {
            // get the newest state
            KeyboardState newState = Keyboard.GetState();

            // With this button we want to continue to the next phase of the game initialization
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || newState.IsKeyDown(Keys.Back) && gameManager.OldState.IsKeyUp(Keys.Back))
            {          
                gameManager.CurrentGameState = gameManager.PreviousGameState;
            }
            gameManager.OldState = newState;
        }

    }
}
