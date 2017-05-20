using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Menu;
using Game.Menu.States;
using Game.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Spelkonstruktionsprojekt.ZEngine.Helpers;
using ZEngine.Systems;
using ZEngine.Wrappers;

namespace Game
{
    public class GameManager
    {
        public const float MinScale = 1.0f;
        public const float MaxScale = 2.0f;
        public static float Scale { get; set; } = 1.0f;
        public static bool MoveHigher { get; set; } = true;
        public static bool MoveRight { get; set; } = true;

        public MenuContent MenuContent { get; }
        // Here we just say that the first state is the Intro
        protected internal GameState CurrentGameState = GameState.Intro;
        protected internal GameState PreviousGameState;
        protected internal KeyboardState OldKeyboardState;
        protected internal GamePadState OldGamepadState;
        
        protected internal FullSystemBundle Engine;
        protected internal Viewport Viewport;
        protected internal SpriteBatch sb = GameDependencies.Instance.SpriteBatch;
        // To keep track of the game configurations made
        protected internal GameConfig gameConfig;

        private IMenu mainMenu;
        private IMenu gameModesMenu;
        private IMenu characterMenu;
        private IMenu credits;
        private IMenu gameIntro;
        private IMenu survivalGame;
        private IMenu pausedMenu;
        private IMenu aboutMenu;
        private IMenu multiplayerMenu;
        private IMenu gameOver;

        // Game states
        public enum GameState
        {
            Intro,
            MainMenu,
            GameModesMenu,
            CharacterMenu,
            MultiplayerMenu,
            PlaySurvivalGame,
            Quit,
            Credits,
            Paused,
            About,
            GameOver
        };

        public GameManager(FullSystemBundle gameBundle)
        {
            Engine = gameBundle;
            Viewport = Engine.Dependencies.Game.GraphicsDevice.Viewport;
            MenuContent = new MenuContent(gameBundle.Dependencies.Game);
            gameConfig = new GameConfig();

            // initializing the states, remember:
            // all the states need to exist in the 
            // manager.
            mainMenu = new MainMenu(this);
            gameModesMenu = new GameModeMenu(this);
            characterMenu = new CharacterMenu(this);
            credits = new Credits(this);
            gameIntro = new GameIntro(this);
            survivalGame = new InGame(this);
            pausedMenu = new PausedMenu(this);
            multiplayerMenu = new MultiplayerMenu(this);
            aboutMenu = new AboutMenu(this);
            gameOver = new GameOver(this);
        }

        // Draw method consists of a switch case with all
        // the different states that we have, depending on which
        // state we are we use that state's draw method.
        public void Draw(GameTime gameTime)
        {
            sb.GraphicsDevice.Clear(Color.Black);
            switch (CurrentGameState)
            {

                case GameState.Intro:
                    gameIntro.Draw(gameTime, sb);
                    break;

                case GameState.MainMenu:
                    mainMenu.Draw(gameTime, sb);
                    break;

                case GameState.PlaySurvivalGame:
                    survivalGame.Draw(gameTime, sb);
                    break;

                case GameState.Quit:
                    Engine.Dependencies.Game.Exit();
                    break;

                case GameState.GameModesMenu:
                    gameModesMenu.Draw(gameTime, sb);
                    break;

                case GameState.CharacterMenu:
                    characterMenu.Draw(gameTime, sb);
                    break;

                case GameState.Credits:
                    credits.Draw(gameTime, sb);
                    break;
                case GameState.Paused:
                    pausedMenu.Draw(gameTime, sb);
                    break;
                case GameState.MultiplayerMenu:
                    multiplayerMenu.Draw(gameTime, sb);
                    break;
                case GameState.About:
                    aboutMenu.Draw(gameTime, sb);
                    break;
                case GameState.GameOver:
                    gameOver.Draw(gameTime, sb);
                    break;
            }
        }

        // Same as the draw method, the update method
        // we execute is the one of the current state.
        public void Update(GameTime gameTime)
        {
            
            switch (CurrentGameState)
            {
                case GameState.Intro:
                    gameIntro.Update(gameTime);
                    break;

                case GameState.MainMenu:
                    mainMenu.Update(gameTime);
                    break;

                case GameState.PlaySurvivalGame:
                    survivalGame.Update(gameTime);
                    break;

                case GameState.Quit:
                    Engine.Dependencies.Game.Exit();
                    break;

                case GameState.GameModesMenu:
                    gameModesMenu.Update(gameTime);
                    break;

                case GameState.CharacterMenu:
                    characterMenu.Update(gameTime);
                    break;

                case GameState.Credits:
                    credits.Update(gameTime);
                    break;

                case GameState.Paused:
                    Engine.Dependencies.Game.GraphicsDevice.Viewport = Viewport;
                    pausedMenu.Update(gameTime);
                    break;
                case GameState.MultiplayerMenu:
                    multiplayerMenu.Update(gameTime);
                    break;
                case GameState.About:
                    aboutMenu.Update(gameTime);
                    break;
                case GameState.GameOver:
                    gameOver.Update(gameTime);
                    break;
            }
        }
    }
}
