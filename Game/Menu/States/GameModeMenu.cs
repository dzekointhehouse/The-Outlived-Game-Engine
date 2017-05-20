using Game.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZEngine.Wrappers;

namespace Game.Menu.States
{
    public class GameModeMenu : IMenu
    {
        private SidewaysBackground fogBackground;
        private readonly Microsoft.Xna.Framework.Game game;
        private readonly GameManager gameManager;
        private readonly ControlsConfig controls;
        private GameModes currentPosition = GameModes.Survival;

        public enum GameModes
        {
            Extinction,
            Survival,
            Blockworld,
            Exit
        }

        public GameModeMenu(GameManager gameManager)
        {
            this.gameManager = gameManager;
            game = this.gameManager.Engine.Dependencies.Game;
            this.controls = new ControlsConfig(0, 2, gameManager);
            fogBackground = new SidewaysBackground(gameManager.MenuContent.BackgroundFog, new Vector2(20, 20), 1f);

        }

        private void MainMenuDisplay()
        {
            SpriteBatch sb = GameDependencies.Instance.SpriteBatch;

            var viewport = game.GraphicsDevice.Viewport;

            switch (currentPosition)
            {
                case GameModes.Survival:
                    sb.Draw(gameManager.MenuContent.GameModeHiglightSurvival, viewport.Bounds, Color.White);
                    sb.Draw(gameManager.MenuContent.ButtonContinue, new Vector2(250, viewport.Height * 0.45f), Color.White);
                    break;
                case GameModes.Extinction:
                    sb.Draw(gameManager.MenuContent.GameModeHiglightExtinction, viewport.Bounds, Color.White);
                    sb.Draw(gameManager.MenuContent.ButtonContinue, new Vector2(250, viewport.Height * 0.20f), Color.White);
                    break;
                case GameModes.Blockworld:
                    sb.Draw(gameManager.MenuContent.GameModeHiglightBlockworld, viewport.Bounds, Color.White);
                    sb.Draw(gameManager.MenuContent.ButtonContinue, new Vector2(250, viewport.Height * 0.70f), Color.White);
                    break;
            }

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            ScalingBackground.DrawBackgroundWithScaling(spriteBatch, gameManager.MenuContent, 0.0001f);
            fogBackground.Draw(spriteBatch);
            MainMenuDisplay();
            spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            fogBackground.Update(gameTime, new Vector2(1, 0), gameManager.Viewport);
            controls.GoBackButton();
            currentPosition = (GameModes)controls.MoveOptionPositionVertically((int)currentPosition);

            switch (currentPosition)
            {
                case GameModes.Survival:
                    if (controls.ContinueButton(GameManager.GameState.MultiplayerMenu))
                    {
                        gameManager.MenuContent.ClickSound.Play();
                        gameManager.gameConfig.GameMode = GameModes.Survival;
                        }
                    break;
                case GameModes.Extinction:
                    if (controls.ContinueButton(GameManager.GameState.MultiplayerMenu))
                    {
                        gameManager.MenuContent.ClickSound.Play();
                        gameManager.gameConfig.GameMode = GameModes.Extinction;
                    }
                    break;
                case GameModes.Exit:
                    if(controls.ContinueButton(GameManager.GameState.MultiplayerMenu))
                        gameManager.MenuContent.ClickSound.Play();

                    break;
                case GameModes.Blockworld:
                    if (controls.ContinueButton(GameManager.GameState.MultiplayerMenu))
                    {
                        gameManager.MenuContent.ClickSound.Play();
                        gameManager.gameConfig.GameMode = GameModes.Blockworld;
                    }
                    break;
            }
        }
    }
}
