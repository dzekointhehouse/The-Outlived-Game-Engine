using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
        private CharacterType characterType = CharacterType.Bob;
        private Player currentPlayer;
        private int playerIndex = 0;


        // enum so we can keep track on which option
        // we currently are at.
        public enum CharacterType
        {
            Bob,
            Edgar,
            Ward,
            Jimmy
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


            if (gameManager.gameConfig.Players.Count == 0)
            {
                gameManager.CurrentGameState = gameManager.PreviousGameState;
                return;
            }
            // Add the first player. This is done the first time.
            if (currentPlayer == null)
                currentPlayer = gameManager.gameConfig.Players.ElementAt(playerIndex++);

            var viewport = game.GraphicsDevice.Viewport;
            var message = "Player " + currentPlayer.Index.ToString() + " Choose your character!";

            sb.DrawString(gameManager.MenuContent.MenuFont, message,
                new Vector2(viewport.Width * 0.1f, viewport.Height * 0.1f), Color.BlueViolet);

            switch (characterType)
            {
                case CharacterType.Bob:
                    sb.Draw(gameManager.MenuContent.HighlightFirst, viewport.Bounds, Color.White);
                    break;
                case CharacterType.Edgar:
                    sb.Draw(gameManager.MenuContent.HighlightSecond, viewport.Bounds, Color.White);
                    break;
                case CharacterType.Ward:
                    sb.Draw(gameManager.MenuContent.HighlightThird, viewport.Bounds, Color.White);
                    break;
                case CharacterType.Jimmy:
                    sb.Draw(gameManager.MenuContent.HighlightFourth, viewport.Bounds, Color.White);
                    break;
            }
        }

        // Here is all the drawing called for this
        // class, so if some drawing isn't in here
        // then it won't be drawn.
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            ScalingBackground.DrawBackgroundWithScaling(spriteBatch, gameManager.MenuContent, 0.0001f);
            MainMenuDisplay();
            spriteBatch.End();
        }

        // The update method for this class
        // that takes care of all the updates, that
        // are to be done.
        public void Update(GameTime gameTime)
        {
            controls.GoBackButton();
            // Add the first player. This is done the first time.
            if (currentPlayer == null)
                currentPlayer = gameManager.gameConfig.Players.ElementAt(playerIndex++);
            // Change character position
            characterType =
                (CharacterType) controls.MoveOptionPositionHorizontally((int) characterType, currentPlayer.Index);


            // If the player pressed continue button but there are players left..
            if (gameManager.gameConfig.Players.Count > playerIndex)
            {
                if (controls.ContinueButton(GameManager.GameState.CharacterMenu))
                {
                    gameManager.MenuContent.ClickSound.Play();
                    // Set the current character to that player
                    // get the next player and then reset.
                    currentPlayer.SpriteName = GetCharacterSpriteName(characterType);
                    currentPlayer.CharacterType = characterType;
                    currentPlayer = gameManager.gameConfig.Players[playerIndex++];
                    characterType = CharacterType.Bob;
                }
            }

            // If there are no players left to choose character.
            // Continue to next state when done with the players.
            if (controls.ContinueButton(GameManager.GameState.PlaySurvivalGame) &&
                gameManager.gameConfig.Players.Count == playerIndex)
            {
                gameManager.MenuContent.ClickSound.Play();
                // set the last player's character.
                currentPlayer.SpriteName = GetCharacterSpriteName(characterType);
                currentPlayer.CharacterType = characterType;
                // Reset values if this state is re-visited.
                playerIndex = 0;
                currentPlayer = null;
                characterType = CharacterType.Bob;
            }
        }

        private string GetCharacterSpriteName(CharacterType choice)
        {
            switch (choice)
            {
                case CharacterType.Bob:
                    return "Bob";
                case CharacterType.Edgar:
                    return "Edgar";
                case CharacterType.Ward:
                    return "Ward";
                case CharacterType.Jimmy:
                    return "Jimmy";
                default:
                    return "Bob";
            }
        }
    }
}