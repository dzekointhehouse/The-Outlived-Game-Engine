using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using ZEngine.Wrappers;

namespace Game.Menu
{
    class GameModeMenu : IMenu
    {

        private readonly Microsoft.Xna.Framework.Game game;
        private readonly GameManager gameManager;
        private readonly ControlConfiguration controls;
        private OptionsState currentPosition = OptionsState.Extinction;



        String textDeathmatch = "Deathmatch";
        String textBlockworld = "Blockworld";
        String textSurvival = "Survival";
        String textExit = "Escape: EXIT THE GAME";

        private enum OptionsState
        {
            Extinction,
            Survival,
            Blockworld,
            Exit
        }

        public GameModeMenu(GameManager gameManager)
        {
            this.gameManager = gameManager;
            game = this.gameManager.engine.Dependencies.Game;
            this.controls = new ControlConfiguration(0, 2, gameManager);
        }

        private void MainMenuDisplay()
        {
            SpriteBatch sb = GameDependencies.Instance.SpriteBatch;



            var viewport = game.GraphicsDevice.Viewport;
            sb.Begin();

            sb.Draw(gameManager.GameContent.GameModeBackground, viewport.Bounds, Color.White);

            sb.DrawString(gameManager.GameContent.MenuFont, textBlockworld, new Vector2(400, viewport.Height * 0.35f), Color.White);
            sb.DrawString(gameManager.GameContent.MenuFont, textSurvival, new Vector2(400, viewport.Height * 0.55f), Color.White);
            sb.DrawString(gameManager.GameContent.MenuFont, textDeathmatch, new Vector2(400, viewport.Height * 0.75f), Color.White);
            sb.DrawString(gameManager.GameContent.MenuFont, textExit, new Vector2(viewport.Width * 0.5f, viewport.Height * 0.9f), Color.Gray);

            switch (currentPosition)
            {
                case OptionsState.Extinction:
                    sb.Draw(gameManager.GameContent.ButtonContinue, new Vector2(250, viewport.Height * 0.30f), Color.White);
                    break;
                case OptionsState.Survival:
                    sb.Draw(gameManager.GameContent.ButtonContinue, new Vector2(250, viewport.Height * 0.50f), Color.White);
                    break;
                case OptionsState.Exit:
                    sb.Draw(gameManager.GameContent.ButtonContinue, new Vector2(250, viewport.Height * 0.70f), Color.White);
                    break;
                case OptionsState.Blockworld:
                    sb.Draw(gameManager.GameContent.ButtonContinue, new Vector2(250, viewport.Height * 0.70f), Color.White);
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
            currentPosition = (OptionsState)controls.GetMenuOptionPosition((int)currentPosition);

            switch (currentPosition)
            {
                case OptionsState.Extinction:
                    controls.ContinueButton(GameManager.GameState.CharacterMenu);
                    break;
                case OptionsState.Survival:
                    controls.ContinueButton(GameManager.GameState.CharacterMenu);
                    break;
                case OptionsState.Exit:
                    controls.ContinueButton(GameManager.GameState.CharacterMenu);
                    break;
                case OptionsState.Blockworld:
                    controls.ContinueButton(GameManager.GameState.CharacterMenu);
                    break;
            }
        }
    }
}
