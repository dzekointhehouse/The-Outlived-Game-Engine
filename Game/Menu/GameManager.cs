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
        private float minScale = 1.0f, maxScale = 2.0f, scale = 1.0f;
        private bool moveHigher = true;
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
        private IMenu survivalGame;
        private IMenu pausedMenu;

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
            survivalGame = new SurvivalGame(this);
            pausedMenu = new PausedMenu(this);

        }
        // Game states
        protected internal enum GameState
        {
            Intro,
            MainMenu,
            GameModesMenu,
            CharacterMenu,
            PlaySurvivalGame,
            Quit,
            Credits,
            Paused
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
                    gameIntro.Draw(gameTime, sb);
                    break;

                case GameState.MainMenu:
                    mainMenu.Draw(gameTime, sb);
                    break;

                case GameState.PlaySurvivalGame:
                    survivalGame.Draw(gameTime, sb);
                    break;

                case GameState.Quit:
                    engine.Dependencies.Game.Exit();
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
                    DrawBackground();
                    mainMenu.Update(gameTime);
                    break;

                case GameState.PlaySurvivalGame:
                    survivalGame.Update(gameTime);
                    break;

                case GameState.Quit:
                    engine.Dependencies.Game.Exit();
                    break;

                case GameState.GameModesMenu:
                    DrawBackground();
                    gameModesMenu.Update(gameTime);
                    break;

                case GameState.CharacterMenu:
                    DrawBackground();
                    characterMenu.Update(gameTime);
                    break;

                case GameState.Credits:
                    DrawBackground();
                    credits.Update(gameTime);
                    break;

                case GameState.Paused:
                    pausedMenu.Update(gameTime);
                    break;
            }
        }

        private void DrawBackground()
        {
            if (scale <= maxScale && scale >= minScale)
            {
                if (scale <= minScale + 0.1)
                {
                    moveHigher = true;
                }
                if (scale >= maxScale - 0.1)
                {
                    moveHigher = false;
                }

                if (moveHigher)
                {
                    scale = scale + 0.0001f;
                }
                else
                {
                    scale = scale - 0.0001f;
                }
            }


            sb.Begin();
            //sb.Draw(gameManager.GameContent.Background, viewport.Bounds, Color.White);
            sb.Draw(
                texture: GameContent.Background,
                position: Vector2.Zero,
                color: Color.White,
                scale: new Vector2(scale)
            );
            sb.End();
        }
    }
}
