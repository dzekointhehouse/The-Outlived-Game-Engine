using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Game.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using ZEngine.Components;
using ZEngine.Wrappers;
using static Game.GameManager.GameState;
using static Game.Menu.States.CharacterMenu.CharacterType;
using static Game.Services.VirtualGamePad.MenuKeys;
using static Game.Services.VirtualGamePad.MenuKeyStates;

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

//        private readonly GameManager gameManager;
//        private readonly ControlsConfig controls;
//        private CharacterType characterType = CharacterType.Bob;
//        private Player currentPlayer;
//        private int playerIndex = 0;

        private GameManager GameManager { get; }

        private int NumberOfPlayers;
        private Player CurrentPlayer;
        private IEnumerator<Player> Players;

        public VirtualGamePad VirtualGamePad { get; set; }
        public MenuNavigator MenuNavigator { get; set; }
        public GameConfig GameConfig { get; }

        // enum so we can keep track on which option
        // we currently are at.
        public enum CharacterType
        {
            Bob,
            Edgar,
            Ward,
            Jimmy
        }

        private CharacterType CurrentSelectedCharacter = Bob;
        private int CurrentSelectedCharacterIndex = 0;
        private CharacterType[] Characters = new[]
        {
            Bob, Bob, Edgar, Ward, Jimmy
        };

        public CharacterMenu(GameManager gameManager, VirtualGamePad virtualGamePad)
        {
            GameManager = gameManager;
            VirtualGamePad = gameManager.Controller;
            MenuNavigator = gameManager.MenuNavigator;
            GameConfig = gameManager.gameConfig;
            VirtualGamePad = virtualGamePad;
//            game = this.gameManager.Engine.Dependencies.Game;
            // Adding the options interval and gamemanager.
//            controls = new ControlsConfig(0, 3, gameManager);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            ScalingBackground.DrawBackgroundWithScaling(spriteBatch, GameManager.MenuContent, 0.0001f);

            if (GameConfig.Players.Count == 0)
            {
                MenuNavigator.GoBack();
            }

            var viewport = game.GraphicsDevice.Viewport;
            DrawCharacterNames(spriteBatch, viewport);
            DrawSelectedOptionText(spriteBatch, viewport);
            spriteBatch.End();
        }

        private void DrawCharacterNames(SpriteBatch spriteBatch, Viewport viewport)
        {
            switch (CurrentSelectedCharacter)
            {
                case Bob:
                    spriteBatch.Draw(GameManager.MenuContent.HighlightFirst, viewport.Bounds, Color.White);
                    break;
                case Edgar:
                    spriteBatch.Draw(GameManager.MenuContent.HighlightSecond, viewport.Bounds, Color.White);
                    break;
                case Ward:
                    spriteBatch.Draw(GameManager.MenuContent.HighlightThird, viewport.Bounds, Color.White);
                    break;
                case Jimmy:
                    spriteBatch.Draw(GameManager.MenuContent.HighlightFourth, viewport.Bounds, Color.White);
                    break;
            }
        }

        private void DrawSelectedOptionText(SpriteBatch spriteBatch, Viewport viewport)
        {
            var message = ("Player " + CurrentSelectedCharacterIndex.ToString() + " Choose your character!");

            spriteBatch.DrawString(GameManager.MenuContent.MenuFont, message,
                new Vector2(viewport.Width * 0.1f, viewport.Height * 0.1f), Color.BlueViolet);
        }

        private void NextPlayerOrStartGame()
        {
            var noMorePlayers = !Players.MoveNext();
            if (noMorePlayers)
            {
                StartGame();
            }
            ResetCharacterSelection();
        }

        private void SelectNextCharacter()
        {
            if (CurrentSelectedCharacterIndex + 1 >= Characters.Length)
            {
                CurrentSelectedCharacterIndex = 0;
            }
            CurrentSelectedCharacter = Characters[CurrentSelectedCharacterIndex];
        }
        
        private void SelectPreviousCharacter()
        {
            if (CurrentSelectedCharacterIndex - 1 < 0)
            {
                CurrentSelectedCharacterIndex = Characters.Length - 1;
            }
            CurrentSelectedCharacter = Characters[CurrentSelectedCharacterIndex];
        }

        private void ResetCharacterSelection(){
        
            CurrentSelectedCharacterIndex = 0;
            CurrentSelectedCharacter = Characters[CurrentSelectedCharacterIndex];
        }
        
        private void StartGame()
        {
            ResetCharacterSelection();
            Players.Reset();
            
            if (MediaPlayer.State != MediaState.Stopped)
                MediaPlayer.Stop();
            
            MenuNavigator.GoTo(PlaySurvivalGame);
        }
        
        // The update method for this class
        // that takes care of all the updates, that
        // are to be done.
        public void Update(GameTime gameTime)
        {
            if (Players.Current == null)
            {
                NextPlayerOrStartGame();    
            }
            
//            controls.GoBackButton();
            if (VirtualGamePad.Is(Cancel, Pressed))
            {
                MenuNavigator.GoBack();
            }
            else if (VirtualGamePad.Is(Accept, Pressed))
            {
                GameManager.MenuContent.ClickSound.Play();
                Players.Current.CharacterType = CurrentSelectedCharacter;
                Players.Current.SpriteName = GetCharacterSpriteName(CurrentSelectedCharacter);
                NextPlayerOrStartGame();
            }
            else if (VirtualGamePad.Is(Up, Pressed))
            {
                SelectNextCharacter();
            }
            else if (VirtualGamePad.Is(Down, Pressed))
            {
                SelectPreviousCharacter();
            }
        }

        private string GetCharacterSpriteName(CharacterType choice)
        {
            switch (choice)
            {
                case Bob:
                    return "Bob";
                case Edgar:
                    return "Edgar";
                case Ward:
                    return "Ward";
                case Jimmy:
                    return "Jimmy";
                default:
                    return "Bob";
            }
        }
    }
}