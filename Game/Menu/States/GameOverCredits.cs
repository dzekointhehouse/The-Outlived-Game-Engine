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
using ZEngine.Wrappers;
using static Game.Services.VirtualGamePad.MenuKeys;
using static Game.Services.VirtualGamePad.MenuKeyStates;

namespace Game.Menu.States
{
    class GameOverCredits : IMenu
    {
        public MenuNavigator MenuNavigator { get; }
        public VirtualGamePad VirtualGamePad { get; }
        private GameManager gameManager;
        private GraphicsDevice gd = OutlivedGame.Instance().GraphicsDevice;
        public bool WasNotPlayed { get; set; } = true;

        public GameOverCredits(GameManager gameManager, MenuNavigator menuNavigator, VirtualGamePad virtualGamePad)
        {
            MenuNavigator = menuNavigator;
            VirtualGamePad = virtualGamePad;
            this.gameManager = gameManager;
        }

        public void Draw(GameTime gameTime, SpriteBatch sb)
        {
            gd.Clear(Color.Black);

            sb.Begin();

            //ScalingBackground.DrawBackgroundWithScaling(sb, gameManager.MenuContent, 0.0001f);
            sb.Draw(gameManager.MenuContent.Background, new Rectangle(0, 0, 1900, 1100), Color.White);
            sb.End();


            string totalScore = "Thanks for playing!";

            sb.Begin();
            sb.DrawString(gameManager.MenuContent.MenuFont, totalScore, new Vector2(380, 40), Color.Red);
            sb.End();
        }

        public void Update(GameTime gameTime)
        {

            if (VirtualGamePad.Is(Cancel, Pressed))
            {
                MenuNavigator.GoTo(GameManager.GameState.MainMenu);
            }
        }

        public void Reset()
        {
        }
    }
}
