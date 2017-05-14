using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZEngine.Wrappers;

namespace Game.Menu.States
{
    class GameModeMenu : IMenu
    {

        private readonly Microsoft.Xna.Framework.Game game;
        private readonly GameManager gameManager;
        private readonly ControlsConfig controls;
        private OptionsState currentPosition = OptionsState.Survival;

        private enum OptionsState
        {
            Survival,
            Extinction,
            Blockworld,
            Exit
        }

        public GameModeMenu(GameManager gameManager)
        {
            this.gameManager = gameManager;
            game = this.gameManager.Engine.Dependencies.Game;
            this.controls = new ControlsConfig(0, 2, gameManager);
        }

        private void MainMenuDisplay()
        {
            SpriteBatch sb = GameDependencies.Instance.SpriteBatch;



            var viewport = game.GraphicsDevice.Viewport;
            sb.Begin();

            switch (currentPosition)
            {
                case OptionsState.Survival:
                    sb.Draw(gameManager.GameContent.GameModeHiglightSurvival, viewport.Bounds, Color.White);
                    sb.Draw(gameManager.GameContent.ButtonContinue, new Vector2(250, viewport.Height * 0.20f), Color.White);
                    break;
                case OptionsState.Extinction:
                    sb.Draw(gameManager.GameContent.GameModeHiglightExtinction, viewport.Bounds, Color.White);
                    sb.Draw(gameManager.GameContent.ButtonContinue, new Vector2(250, viewport.Height * 0.45f), Color.White);
                    break;
                case OptionsState.Blockworld:
                    sb.Draw(gameManager.GameContent.GameModeHiglightBlockworld, viewport.Bounds, Color.White);
                    sb.Draw(gameManager.GameContent.ButtonContinue, new Vector2(250, viewport.Height * 0.70f), Color.White);
                    break;
            }


            sb.End();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            MainMenuDisplay();

        }

        public void Update(GameTime gameTime)
        {
            currentPosition = (OptionsState)controls.MoveOptionPositionVertically((int)currentPosition);

            switch (currentPosition)
            {
                case OptionsState.Survival:
                    controls.ContinueButton(GameManager.GameState.PlaySurvivalGame);
                    break;
                case OptionsState.Extinction:
                    controls.ContinueButton(GameManager.GameState.MultiplayerMenu);
                    break;
                case OptionsState.Exit:
                    controls.ContinueButton(GameManager.GameState.MultiplayerMenu);
                    break;
                case OptionsState.Blockworld:
                    controls.ContinueButton(GameManager.GameState.MultiplayerMenu);
                    break;
            }
        }
    }
}
