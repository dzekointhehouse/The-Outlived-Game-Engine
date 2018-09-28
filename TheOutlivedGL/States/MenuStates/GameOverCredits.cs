using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using TheOutlivedGL;
using ZEngine.Wrappers;
using static Game.Services.VirtualGamePad.MenuKeys;
using static Game.Services.VirtualGamePad.MenuKeyStates;

namespace Game.Menu.States
{
    class GameOverCredits : IMenu
    {
        private GameManager gameManager;
        private GraphicsDevice gd = OutlivedGame.Instance().GraphicsDevice;
        public bool WasNotPlayed { get; set; } = true;

        public GameOverCredits(GameManager gameManager)
        {
            this.gameManager = gameManager;
        }

        public void Draw(GameTime gameTime, SpriteBatch sb)
        {
            gd.Clear(Color.Black);

            sb.Begin();
            //ScalingBackground.DrawBackgroundWithScaling(sb, gameManager.OutlivedContent, 0.0001f);
            sb.Draw(AssetManager.Instance.Get<Texture2D>("Images/Menu/background3"), new Rectangle(0, 0, 1900, 1100), Color.White);
            sb.End();

            string totalScore = "Thanks for playing!";

            sb.Begin();
            sb.DrawString(AssetManager.Instance.Get<SpriteFont>("Fonts/ZMenufont"), totalScore, new Vector2(380, 40), Color.Red);
            sb.End();
        }

        public void Update(GameTime gameTime)
        {

            if (gameManager.playerControllers.Controllers.Any(c => c.Is(Cancel, Pressed)))
            {
                gameManager.MenuNavigator.GoTo(OutlivedStates.GameState.MainMenu);
            }
        }

        public void Reset()
        {
        }
    }
}
