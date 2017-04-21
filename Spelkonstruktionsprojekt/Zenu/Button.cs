using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ZEngine.Components;
using ZEngine.Managers;
using static Spelkonstruktionsprojekt.ZEngine.GameTest.TestChaseGame;

namespace Spelkonstruktionsprojekt.Zenu
{
    public class Button
    {
        public Button(GameState currentGameState, GameState nextGameState)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Enter)) currentGameState = nextGameState;
        }
    }
}