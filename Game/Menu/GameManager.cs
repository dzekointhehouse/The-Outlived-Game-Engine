using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Menu;
using Game.Menu.States;
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

        protected internal FullZengineBundle engine;
        private SpriteFont font;
        private Texture2D background;
        SpriteBatch sb = GameDependencies.Instance.SpriteBatch;
        // Here we just say that the first state is the Intro
        protected internal GameState CurrentGameState = GameState.Intro;
        protected internal GameState PreviousGameState;
        protected internal KeyboardState OldState;
        protected internal GameContent GameContent;
        // Game states
        private IMenu mainMenu;
        private IMenu gameModesMenu;
        private IMenu characterMenu;
        private IMenu credits;
        private IMenu gameIntro;

        public GameManager(FullZengineBundle gameBundle)
        {
            engine = gameBundle;
            GameContent = new GameContent(gameBundle.Dependencies.Game);

            // initializing the states, remember:
            // all the states need to exist in the 
            // manager.
            mainMenu = new MainMenu(this);
            gameModesMenu = new GameModeMenu(this);
            characterMenu = new CharacterMenu(this);
            credits = new Credits(this);
            gameIntro = new GameIntro(this);

        }
        // Game states
        public enum GameState
        {
            Intro,
            MainMenu,
            Play,
            GameModesMenu,
            CharacterMenu,
            InGame,
            Quit,
            Credits,

        };


        private void ExitButton()
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.S)) engine.Dependencies.Game.Exit();
        }

        public void Draw(GameTime gameTime)
        {
            switch (CurrentGameState)
            {

                case GameState.Intro:

                    gameIntro.Draw(sb);

                    break;

                case GameState.MainMenu:
                    mainMenu.Draw(sb);
                    break;

                case GameState.InGame:

                    //PlayGame(gameTime);

                    break;

                case GameState.Quit:

                    engine.Dependencies.Game.Exit();

                    break;
                case GameState.GameModesMenu:
                    gameModesMenu.Draw(sb);
                    break;
                case GameState.CharacterMenu:
                    characterMenu.Draw(sb);
                    break;
                case GameState.Credits:
                    credits.Draw(sb);
                    break;
            }
        }

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

                case GameState.InGame:

                   // PlayGame(gameTime);

                    break;

                case GameState.Quit:

                    engine.Dependencies.Game.Exit();

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
            }
        }
    }
}
