using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ZEngine.Wrappers;

namespace Game.Menu
{
    class MainMenu : IMenu
    {
        private Microsoft.Xna.Framework.Game game;
        private GameManager gameManager;
        private ControlConfiguration controls;
        private OptionsState currentPosition = OptionsState.Continue;
        private float minScale = 1.0f, maxScale = 2.0f, scale = 1.0f;
        private bool moveHigher = true;
        Viewport viewport;

        private enum OptionsState
        {
            Continue,
            Credits,
            Exit
        }

        public MainMenu(GameManager gameManager)
        {
            this.gameManager = gameManager;
            game = this.gameManager.engine.Dependencies.Game;
            controls = new ControlConfiguration(0, 2, gameManager);
            viewport = game.GraphicsDevice.Viewport;
        }



        private void MainMenuDisplay()
        {
            SpriteBatch sb = GameDependencies.Instance.SpriteBatch;
            String textEscape = "CREDITS";
            String textContinue = "PLAY IF YOU DARE";
            String textExit = "TAKE THE EASY WAY OUT";


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
                texture: gameManager.GameContent.Background,
                position:Vector2.Zero,
                color: Color.White,
                scale: new Vector2(scale)
            );
            sb.Draw(gameManager.GameContent.MainOptionsBackground, viewport.Bounds, Color.White);

            sb.DrawString(gameManager.GameContent.MenuFont, textContinue, new Vector2(600, viewport.Height * 0.45f), Color.White);
            sb.DrawString(gameManager.GameContent.MenuFont, textEscape, new Vector2(600, viewport.Height * 0.55f), Color.White);
            sb.DrawString(gameManager.GameContent.MenuFont, textExit, new Vector2(600, viewport.Height * 0.65f), Color.White);

            switch (currentPosition)
            {
                case OptionsState.Continue:
                    sb.Draw(gameManager.GameContent.ButtonContinue, new Vector2(250, viewport.Height * 0.40f), Color.White);
                    break;
                case OptionsState.Credits:
                    sb.Draw(gameManager.GameContent.ButtonContinue, new Vector2(250, viewport.Height * 0.50f), Color.White);
                    break;
                case OptionsState.Exit:
                    sb.Draw(gameManager.GameContent.ButtonContinue, new Vector2(250, viewport.Height * 0.60f), Color.White);
                    break;
            }

            sb.End();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            MainMenuDisplay();

        }

        public void Update(GameTime gameTime)
        {
            currentPosition = (OptionsState) controls.GetMenuOptionPosition((int) currentPosition);

            switch (currentPosition)
            {
                case OptionsState.Continue:
                    controls.ContinueButton(GameManager.GameState.GameModesMenu);
                    break;
                case OptionsState.Credits:
                    controls.ContinueButton(GameManager.GameState.Credits);
                    break;
                case OptionsState.Exit:
                    controls.ContinueButton(GameManager.GameState.Quit);
                    break;
            }
        }
    }
}
