using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Game.Controllers
{
    public class XboxControls
    {
        private readonly GameManager gameManager;
        public XboxControls(GameManager gameManager)
        {
            this.gameManager = gameManager;
        }

        public void Continue(GameManager.GameState nextState, PlayerIndex player)
        {
            // get the newest state
            GamePadState newGamePadState = GamePad.GetState(player);


            // With this button we want to continue to the next phase of the game initialization
            if (GamePad.GetState(PlayerIndex.One).Buttons.X == ButtonState.Pressed)
            {
                gameManager.PreviousGameState = gameManager.CurrentGameState;
                gameManager.CurrentGameState = nextState;
            }
            gameManager.OldGamepadState = newGamePadState;
        }

        public void GoBack(PlayerIndex player)
        {
            // get the newest state
            GamePadState newGamePadState = GamePad.GetState(player);

            // With this button we want to continue to the next phase of the game initialization
            if (GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed)
            {
                gameManager.CurrentGameState = gameManager.PreviousGameState;
            }
            gameManager.OldGamepadState = newGamePadState;
        }

        public void PauseButton(PlayerIndex player)
        {
            // get the newest state
            GamePadState newGamePadState = GamePad.GetState(player);

            // With this button we want to continue to the next phase of the game initialization
            if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed)
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
            gameManager.OldGamepadState = newGamePadState;
        }
    }
}
