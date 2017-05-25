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
using System.Diagnostics;

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
        

        private int CurrentPlayerIndex;
        private VirtualGamePad Player { get; set; }
        public PlayerVirtualInputCollection VirtualInputs { get; }
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
            Bob, Edgar, Ward, Jimmy
        };

        public CharacterMenu(GameManager gameManager, PlayerVirtualInputCollection virtualInputs)
        {
            GameManager = gameManager;
            VirtualInputs = virtualInputs;
            Player = VirtualInputs.PlayerOne();
            MenuNavigator = gameManager.MenuNavigator;
            GameConfig = gameManager.gameConfig;
            
            game = gameManager.Engine.Dependencies.Game;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            ScalingBackground.DrawBackgroundWithScaling(spriteBatch, GameManager.MenuContent, 0.0001f);
            
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
            var message = ("Player " + CurrentPlayerIndex.ToString() + " Choose your character!");

            spriteBatch.DrawString(GameManager.MenuContent.MenuFont, message,
                new Vector2(viewport.Width * 0.1f, viewport.Height * 0.1f), Color.BlueViolet);
        }

        private void NextPlayerOrStartGame()
        {
            CurrentPlayerIndex++;
            if (CurrentPlayerIndex >= GameConfig.Players.Count)
            {
                StartGame();
            }
            else
            {
                Player = VirtualInputs.VirtualGamePads[CurrentPlayerIndex]; 
            }
            ResetCharacterSelection();
        }

        private void PreviousPlayerOrGoBack()
        {
            CurrentPlayerIndex--;
            if (CurrentPlayerIndex < 0)
            {
                MenuNavigator.GoBack();
            }
            else
            {
                Player = VirtualInputs.VirtualGamePads[CurrentPlayerIndex]; 
            }
            ResetCharacterSelection();
        }

        private void SelectNextCharacter()
        {
            CurrentSelectedCharacterIndex++;
            if (CurrentSelectedCharacterIndex >= Characters.Length)
            {
                CurrentSelectedCharacterIndex = 0;
            }
            CurrentSelectedCharacter = Characters[CurrentSelectedCharacterIndex];
        }
        
        private void SelectPreviousCharacter()
        {
            CurrentSelectedCharacterIndex--;
            if (CurrentSelectedCharacterIndex < 0)
            {
                CurrentSelectedCharacterIndex = Characters.Length - 1;
            }
            CurrentSelectedCharacter = Characters[CurrentSelectedCharacterIndex];
        }

        private void ResetCharacterSelection(){
        
            CurrentSelectedCharacterIndex = 0;
            CurrentSelectedCharacter = Characters[CurrentSelectedCharacterIndex];
        }

        private Player CurrentPlayer()
        {
            return GameConfig.Players[CurrentPlayerIndex];
        }
        
        private void StartGame()
        {
            if (MediaPlayer.State != MediaState.Stopped)
                MediaPlayer.Stop();
            
            MenuNavigator.GoTo(PlaySurvivalGame);
        }
        
        // The update method for this class
        // that takes care of all the updates, that
        // are to be done.
        public void Update(GameTime gameTime)
        {
            if (GameConfig.Players.Count == 0)
            {
                MenuNavigator.GoBack();
            }
            else if (Player.Is(Cancel, Pressed))
            {
                PreviousPlayerOrGoBack();
            }
            else if (Player.Is(Accept, Pressed))
            {
                GameManager.MenuContent.ClickSound.Play();
                CurrentPlayer().CharacterType = CurrentSelectedCharacter;
                CurrentPlayer().SpriteName = GetCharacterSpriteName(CurrentSelectedCharacter);
                NextPlayerOrStartGame();
            }
            else if (Player.Is(Right, Pressed))
            {
                SelectNextCharacter();
            }
            else if (Player.Is(Left, Pressed))
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

        public void Reset()
        {
            ResetCharacterSelection();
            CurrentPlayerIndex = 0;
        }
    }
}