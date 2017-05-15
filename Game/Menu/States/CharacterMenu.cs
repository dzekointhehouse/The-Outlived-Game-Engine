using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Services;
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
    public class CharacterMenu : IMenu
    {
        // Dependencies
        private readonly Microsoft.Xna.Framework.Game game;
        private readonly GameManager gameManager;
        private readonly ControlsConfig controls;
        private CharacterState currentPosition = CharacterState.FirstCharacter;
        private Player currentPlayer;
        private int playerIndex = 0;


        // enum so we can keep track on which option
        // we currently are at.
        public enum CharacterState
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

            // Add the first player. This is done the first time.
            if (currentPlayer == null)
                currentPlayer = gameManager.gameConfig.Players.ElementAt(playerIndex++);

            var viewport = game.GraphicsDevice.Viewport;
            sb.DrawString(gameManager.GameContent.MenuFont, "Player " + currentPlayer.Index.ToString(), new Vector2(viewport.Width * 0.5f, viewport.Height * 0.15f), Color.White);

            switch (currentPosition)
            {
                case CharacterState.FirstCharacter:
                    sb.Draw(gameManager.GameContent.HighlightFirst, viewport.Bounds, Color.White);
                    break;
                case CharacterState.SecondCharacter:
                    sb.Draw(gameManager.GameContent.HighlightSecond, viewport.Bounds, Color.White);
                    break;
                case CharacterState.ThirdCharacter:
                    sb.Draw(gameManager.GameContent.HighlightThird, viewport.Bounds, Color.White);
                    break;
                case CharacterState.FourthCharacter:
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
            // Add the first player. This is done the first time.
            if (currentPlayer == null)
                currentPlayer = gameManager.gameConfig.Players.ElementAt(playerIndex++);
            // Change character position
            currentPosition = (CharacterState)controls.MoveOptionPositionHorizontally((int)currentPosition, currentPlayer.Index);
            //currentPosition = (CharacterState)controls.MoveOptionPositionVertically((int)currentPosition, currentPlayer.Index);
           


            // If the player pressed continue button but there are players left..
            if (gameManager.gameConfig.Players.Count > playerIndex)
            {
                if (controls.ContinueButton(GameManager.GameState.CharacterMenu))
                {
                    
                
                // Set the current character to that player
                // pop the next player and reset.
                currentPlayer.Character = currentPosition;
                currentPlayer = gameManager.gameConfig.Players[playerIndex++];
                currentPosition = CharacterState.FirstCharacter;
                }
            }

            // If there are no players left to choose character.
            // Continue to next state when done with the players.
            if (controls.ContinueButton(GameManager.GameState.MainMenu) && gameManager.gameConfig.Players.Count == playerIndex)
            {
                // Reset values if this state is re-visited.
                playerIndex = 0;
                currentPlayer = null;
                currentPosition = CharacterState.FirstCharacter;
            }
            // controls.GoBackButton();

        }
    }
}
