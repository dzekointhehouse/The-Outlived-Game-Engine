using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZEngine.Wrappers;

namespace Game.Menu.States
{
    /// <summary>
    /// The character menu state is used for drawing the options to
    /// choose from different characters that are to be used in the 
    /// game by the player or players.
    /// </summary>
    class CharacterMenu : IMenu
    {
        // Dependencies
        private readonly Microsoft.Xna.Framework.Game game;
        private readonly GameManager gameManager;
        private readonly ControlConfiguration controls;
        private OptionsState currentPosition = OptionsState.FirstCharacter;

        // Character names
        private string textOne = "Carlios";
        private string textTwo = "Bonzie";
        private string textThree = "Dr.Strange";
        private string textFour = "Wladimir";

        // enum so we can keep track on which option
        // we currently are at.
        private enum OptionsState
        {
            FirstCharacter,
            SecondCharacter,
            ThirdCharacter,
            FourthCharacter
        }

        public CharacterMenu(GameManager gameManager)
        {
            this.gameManager = gameManager;
            game = this.gameManager.engine.Dependencies.Game;
            // Adding the options interval and gamemanager.
            controls = new ControlConfiguration(0, 3, gameManager);
        }

        // Draws the character names and the button at the option that
        // is the current option that we are positioned at.
        private void MainMenuDisplay()
        {
            SpriteBatch sb = GameDependencies.Instance.SpriteBatch;

            var viewport = game.GraphicsDevice.Viewport;
            sb.Begin();

            //sb.Draw(gameManager.GameContent.CharacterBackground, viewport.Bounds, Color.White);

            //sb.DrawString(gameManager.GameContent.MenuFont, textTwo, new Vector2(400, viewport.Height * 0.40f), Color.White);
            //sb.DrawString(gameManager.GameContent.MenuFont, textThree, new Vector2(400, viewport.Height * 0.50f), Color.White);
            //sb.DrawString(gameManager.GameContent.MenuFont, textOne, new Vector2(400, viewport.Height * 0.60f), Color.White);
            //sb.DrawString(gameManager.GameContent.MenuFont, textFour, new Vector2(400, viewport.Height * 0.70f), Color.Gray);

            switch (currentPosition)
            {
                case OptionsState.FirstCharacter:
                    sb.Draw(gameManager.GameContent.HighlightFirst, viewport.Bounds, Color.White);
                    //sb.Draw(gameManager.GameContent.ButtonContinue, new Vector2(250, viewport.Height * 0.35f), Color.White);
                    break;
                case OptionsState.SecondCharacter:
                    sb.Draw(gameManager.GameContent.HighlightSecond, viewport.Bounds, Color.White);
                    //sb.Draw(gameManager.GameContent.ButtonContinue, new Vector2(250, viewport.Height * 0.45f), Color.White);
                    break;
                case OptionsState.ThirdCharacter:
                    sb.Draw(gameManager.GameContent.HighlightThird, viewport.Bounds, Color.White);
                    //sb.Draw(gameManager.GameContent.ButtonContinue, new Vector2(250, viewport.Height * 0.55f), Color.White);
                    break;
                case OptionsState.FourthCharacter:
                    sb.Draw(gameManager.GameContent.HighlightFourth, viewport.Bounds, Color.White);
                    //sb.Draw(gameManager.GameContent.ButtonContinue, new Vector2(250, viewport.Height * 0.65f), Color.White);
                    break;

            }
            sb.End();
        }
        // Here is all the drawing called for this
        // class, so if some drawing isn't in here
        // then it won't be drawn.
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            MainMenuDisplay();

        }

        // The update method for this class
        // that takes care of all the updates, that
        // are to be done.
        public void Update(GameTime gameTime)
        {
            currentPosition = (OptionsState)controls.GetMenuOptionPosition((int)currentPosition);

            switch (currentPosition)
            {
                case OptionsState.FirstCharacter:
                    controls.ContinueButton(GameManager.GameState.MainMenu);
                    break;
                case OptionsState.SecondCharacter:
                    controls.ContinueButton(GameManager.GameState.MainMenu);
                    break;
                case OptionsState.ThirdCharacter:
                    controls.ContinueButton(GameManager.GameState.MainMenu);
                    break;
                case OptionsState.FourthCharacter:
                    controls.ContinueButton(GameManager.GameState.MainMenu);
                    break;
            }
        }
    }
}
