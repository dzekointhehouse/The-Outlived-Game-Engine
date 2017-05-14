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
        private readonly ControlsConfig controls;
        private OptionsState currentPosition = OptionsState.FirstCharacter;

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
            game = this.gameManager.Engine.Dependencies.Game;
            // Adding the options interval and gamemanager.
            controls = new ControlsConfig(0, 3, gameManager);
        }

        // Draws the character names and the button at the option that
        // is the current option that we are positioned at.
        private void MainMenuDisplay()
        {
            SpriteBatch sb = GameDependencies.Instance.SpriteBatch;

            var viewport = game.GraphicsDevice.Viewport;

            switch (currentPosition)
            {
                case OptionsState.FirstCharacter:
                    sb.Draw(gameManager.GameContent.HighlightFirst, viewport.Bounds, Color.White);
                    break;
                case OptionsState.SecondCharacter:
                    sb.Draw(gameManager.GameContent.HighlightSecond, viewport.Bounds, Color.White);
                    break;
                case OptionsState.ThirdCharacter:
                    sb.Draw(gameManager.GameContent.HighlightThird, viewport.Bounds, Color.White);
                    break;
                case OptionsState.FourthCharacter:
                    sb.Draw(gameManager.GameContent.HighlightFourth, viewport.Bounds, Color.White);
                    break;

            }
        }

        // Here is all the drawing called for this
        // class, so if some drawing isn't in here
        // then it won't be drawn.
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            MenuHelper.DrawBackground(spriteBatch, gameManager.GameContent);
            MainMenuDisplay();
            spriteBatch.End();
        }

        // The update method for this class
        // that takes care of all the updates, that
        // are to be done.
        public void Update(GameTime gameTime)
        {
            currentPosition = (OptionsState)controls.MoveOptionPositionHorizontally((int)currentPosition);
            currentPosition = (OptionsState)controls.MoveOptionPositionVertically((int)currentPosition);

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
