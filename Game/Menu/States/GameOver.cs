using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using Spelkonstruktionsprojekt.ZEngine.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZEngine.Wrappers;
using Game.Services;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using static Game.Services.VirtualGamePad.MenuKeys;
using static Game.Services.VirtualGamePad.MenuKeyStates;

namespace Game.Menu.States
{
    class GameOver : IMenu, ILifecycle
    {
        public MenuNavigator MenuNavigator { get; }
        public VirtualGamePad VirtualGamePad { get; }
        private GameManager gameManager;
        private GraphicsDevice gd = GameDependencies.Instance.GraphicsDeviceManager.GraphicsDevice;
        public bool WasNotPlayed { get; set; } = true;

        public GameOver(GameManager gameManager, MenuNavigator menuNavigator, VirtualGamePad virtualGamePad)
        {
            MenuNavigator = menuNavigator;
            VirtualGamePad = virtualGamePad;
            this.gameManager = gameManager;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            gd.Clear(Color.Black);

            //spriteBatch.Begin();
            //spriteBatch.Draw(gameManager.MenuContent.GameOver, new Rectangle(0, 0, 1800, 1500), Color.White);
            //spriteBatch.End();

            //var GameScoreList = ComponentManager.Instance.GetEntitiesWithComponent(typeof(GameScoreComponent));
            //if (GameScoreList.Count <= 0) return;
            //var GameScore = (GameScoreComponent)GameScoreList.First().Value;

            //string yourScore = "Total score: " + GameScore.TotalGameScore;
            //string exit = "(Press ESCAPE to exit)";

            //spriteBatch.Begin();
            //spriteBatch.DrawString(gameManager.MenuContent.MenuFont, yourScore, new Vector2(50, 100), Color.Red);
            //spriteBatch.DrawString(gameManager.MenuContent.MenuFont, exit, new Vector2(50, 200), Color.Red);
            //spriteBatch.End();

            spriteBatch.Begin();

            //ScalingBackground.DrawBackgroundWithScaling(spriteBatch, gameManager.MenuContent, 0.0001f);
            spriteBatch.Draw(gameManager.MenuContent.GameOver, new Rectangle(0, 0, 1900, 1100), Color.White);
            spriteBatch.End();

            string totalScoreText = "Total score: ";
            string exitText = "(ESC -> main menu)";

            spriteBatch.Begin();
            spriteBatch.DrawString(gameManager.MenuContent.MenuFont, totalScoreText, new Vector2(50, 40), Color.Red);
            spriteBatch.DrawString(gameManager.MenuContent.MenuFont, exitText, new Vector2(30, 90), Color.Red);
            spriteBatch.End();

            var GameScoreList = ComponentManager.Instance.GetEntitiesWithComponent(typeof(GameScoreComponent));
            if (GameScoreList.Count <= 0) return;
            var GameScore = (GameScoreComponent)GameScoreList.First().Value;

            string totalScore = GameScore.TotalGameScore.ToString();

            spriteBatch.Begin();
            spriteBatch.DrawString(gameManager.MenuContent.MenuFont, totalScore, new Vector2(380, 40), Color.Red);
            spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {

            if (WasNotPlayed)
            {
                OutlivedGame.Instance().Get<SoundEffect>("Sound/GameOver").Play();
                WasNotPlayed = false;
            }

            if(VirtualGamePad.Is(Cancel, Pressed))
            {
                MediaPlayer.Stop();
                MenuNavigator.GoTo(GameManager.GameState.MainMenu);
            }
        }

        public void Reset()
        {
        }

        public void BeforeShow()
        {
            
        }

        public void BeforeHide()
        {
            ComponentManager.Instance.Clear();
        }
    }
}
