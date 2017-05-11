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
        protected internal GameState CurrentGameState = GameState.MainMenu;
        protected internal KeyboardState OldState;
        protected internal GameContent GameContent;
        // Game states
        private IMenu mainMenu;
        private IMenu gameModesMenu;
        private IMenu characterMenu;

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
            GameOver,

        };


        public void IntroVideo()
        {

            Texture2D videoTexture = null;

            //if (player.State != MediaState.Stopped)
            //    videoTexture = player.GetTexture();

            if (videoTexture != null)
            {
                sb.Begin();
                // sb.Draw(videoTexture, new Rectangle(0, 0, (int)vp.X, (int)vp.Y), Color.White);
                sb.End();

            }
            else CurrentGameState = GameState.MainMenu;
        }

        public void PlayGame(GameTime gameTime)
        {
            // Draw was done here

            //BackToMenu();
            //ContinueButton();
            //ExitButton();
        }

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

                    IntroVideo();

                    break;

                case GameState.MainMenu:
                    mainMenu.Draw(sb);
                    break;

                case GameState.InGame:

                    PlayGame(gameTime);

                    break;

                case GameState.GameOver:

                    engine.Dependencies.Game.Exit();

                    break;
                case GameState.GameModesMenu:
                    gameModesMenu.Draw(sb);
                    break;
                case GameState.CharacterMenu:
                    characterMenu.Draw(sb);
                    break;
            }
        }

        public void Update(GameTime gameTime)
        {
            switch (CurrentGameState)
            {

                case GameState.MainMenu:
                    mainMenu.Update(gameTime);
                    break;

                case GameState.InGame:

                    PlayGame(gameTime);

                    break;

                case GameState.GameOver:

                    engine.Dependencies.Game.Exit();

                    break;
                case GameState.GameModesMenu:
                    gameModesMenu.Update(gameTime);
                    break;
                case GameState.CharacterMenu:
                    characterMenu.Update(gameTime);
                    break;
            }
        }
    }
}
