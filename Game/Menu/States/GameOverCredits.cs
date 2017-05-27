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
        private GraphicsDevice gd = GameDependencies.Instance.GraphicsDeviceManager.GraphicsDevice;
        public bool WasNotPlayed { get; set; } = true;

        public GameOverCredits(GameManager gameManager, MenuNavigator menuNavigator, VirtualGamePad virtualGamePad)
        {
            MenuNavigator = menuNavigator;
            VirtualGamePad = virtualGamePad;
            this.gameManager = gameManager;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            gd.Clear(Color.Black);

            spriteBatch.Begin();

            //ScalingBackground.DrawBackgroundWithScaling(spriteBatch, gameManager.MenuContent, 0.0001f);
            spriteBatch.Draw(gameManager.MenuContent.Background, new Rectangle(0, 0, 1900, 1100), Color.White);
            spriteBatch.End();


            string totalScore = "Thanks for playing!";

            spriteBatch.Begin();
            spriteBatch.DrawString(gameManager.MenuContent.MenuFont, totalScore, new Vector2(380, 40), Color.Red);
            spriteBatch.End();
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
