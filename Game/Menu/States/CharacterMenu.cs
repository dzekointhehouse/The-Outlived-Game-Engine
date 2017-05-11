using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZEngine.Wrappers;

namespace Game.Menu.States
{
    class CharacterMenu : IMenu
    {

        private readonly Microsoft.Xna.Framework.Game game;
        private readonly GameManager gameManager;
        private readonly ControlConfiguration controls;
        private OptionsState currentPosition = OptionsState.Exit;



        String textOne = "Carlos";
        String textTwo = "Elvir";
        String textThree = "Markus";
        String textFour = "Escape: EXIT THE GAME";

        private enum OptionsState
        {
            Extinction,
            Survival,
            Blockworld,
            Exit
        }

        public CharacterMenu(GameManager gameManager)
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

            sb.DrawString(gameManager.GameContent.MenuFont, textTwo, new Vector2(400, viewport.Height * 0.40f), Color.White);
            sb.DrawString(gameManager.GameContent.MenuFont, textThree, new Vector2(400, viewport.Height * 0.50f), Color.White);
            sb.DrawString(gameManager.GameContent.MenuFont, textOne, new Vector2(400, viewport.Height * 0.60f), Color.White);
            sb.DrawString(gameManager.GameContent.MenuFont, textFour, new Vector2(400, viewport.Height * 0.70f), Color.Gray);

            switch (currentPosition)
            {
                case OptionsState.Extinction:
                    sb.Draw(gameManager.GameContent.ButtonEnter, new Vector2(250, viewport.Height * 0.35f), Color.White);
                    break;
                case OptionsState.Survival:
                    sb.Draw(gameManager.GameContent.ButtonEnter, new Vector2(250, viewport.Height * 0.45f), Color.White);
                    break;
                case OptionsState.Exit:
                    sb.Draw(gameManager.GameContent.ButtonEnter, new Vector2(250, viewport.Height * 0.55f), Color.White);
                    break;
                case OptionsState.Blockworld:
                    sb.Draw(gameManager.GameContent.ButtonEnter, new Vector2(250, viewport.Height * 0.65f), Color.White);
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
                    controls.ContinueButton(GameManager.GameState.MainMenu);
                    break;
                case OptionsState.Survival:
                    controls.ContinueButton(GameManager.GameState.MainMenu);
                    break;
                case OptionsState.Exit:
                    controls.ContinueButton(GameManager.GameState.MainMenu);
                    break;
                case OptionsState.Blockworld:
                    controls.ContinueButton(GameManager.GameState.MainMenu);
                    break;
            }
        }
    }
}
