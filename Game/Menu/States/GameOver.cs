using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using Spelkonstruktionsprojekt.ZEngine.Components;
using System.Linq;
using System.Threading.Tasks;
using ZEngine.Wrappers;
using Game.Services;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using static Game.Services.VirtualGamePad.MenuKeys;
using static Game.Services.VirtualGamePad.MenuKeyStates;
using ZEngine.Managers;
using Spelkonstruktionsprojekt.ZEngine.Systems;

namespace Game.Menu.States
{
    class GameOver : IMenu, ILifecycle
    {
        public MenuNavigator MenuNavigator { get; }
        public VirtualGamePad VirtualGamePad { get; }
        private GameManager gameManager;
        private GraphicsDevice graphicsDevice = OutlivedGame.Instance().GraphicsDevice;
        public bool WasNotPlayed { get; set; } = true;

        public GameOver(GameManager gameManager, MenuNavigator menuNavigator, VirtualGamePad virtualGamePad)
        {
            MenuNavigator = menuNavigator;
            VirtualGamePad = virtualGamePad;
            this.gameManager = gameManager;
        }

        public void Draw(GameTime gameTime, SpriteBatch sb)
        {
            graphicsDevice.Clear(Color.Black);

            //sb.Begin();
            //sb.Draw(gameManager.MenuContent.GameOver, new Rectangle(0, 0, 1800, 1500), Color.White);
            //sb.End();

            //var GameScoreList = ComponentManager.Instance.GetEntitiesWithComponent(typeof(GameScoreComponent));
            //if (GameScoreList.Count <= 0) return;
            //var GameScore = (GameScoreComponent)GameScoreList.First().Value;

            //string yourScore = "Total score: " + GameScore.TotalGameScore;
            //string exit = "(Press ESCAPE to exit)";

            //sb.Begin();
            //sb.DrawString(gameManager.MenuContent.MenuFont, yourScore, new Vector2(50, 100), Color.Red);
            //sb.DrawString(gameManager.MenuContent.MenuFont, exit, new Vector2(50, 200), Color.Red);
            //sb.End();

            sb.Begin();

            //ScalingBackground.DrawBackgroundWithScaling(sb, gameManager.MenuContent, 0.0001f);
            sb.Draw(gameManager.MenuContent.GameOver, new Rectangle(0, 0, 1900, 1100), Color.White);
            sb.End();

            string totalScoreText = "Total score: ";
            string exitText = "(ESC -> main menu)";

            sb.Begin();
            sb.DrawString(gameManager.MenuContent.MenuFont, totalScoreText, new Vector2(50, 40), Color.Red);
            sb.DrawString(gameManager.MenuContent.MenuFont, exitText, new Vector2(30, 90), Color.Red);
            sb.End();

            var GameScoreList = ComponentManager.Instance.GetEntitiesWithComponent(typeof(GameScoreComponent));
            if (GameScoreList.Count <= 0) return;
            var GameScore = (GameScoreComponent)GameScoreList.First().Value;

            string totalScore = GameScore.TotalGameScore.ToString();

            sb.Begin();
            sb.DrawString(gameManager.MenuContent.MenuFont, totalScore, new Vector2(380, 40), Color.Red);
            sb.End();


            var HighScoreList = ComponentManager.Instance.GetEntitiesWithComponent(typeof(HighScoreComponent));
            if (HighScoreList.Count <= 0) return;
            var HighScore = (HighScoreComponent)HighScoreList.First().Value;
            SystemManager.Instance.Get<HighScoreSystem>().SubmitScore(GameScore.TotalGameScore);
            string[] score = HighScore.score;
            string record = "Record: " + score[0];

            sb.Begin();
            sb.DrawString(gameManager.MenuContent.MenuFont, record, new Vector2(50, 200), Color.Red);
            sb.End();


        }

        public void Update(GameTime gameTime)
        {
            if (WasNotPlayed)
            {
                OutlivedGame.Instance().Get<SoundEffect>("Sound/cinematic").Play();
                OutlivedGame.Instance().Get<SoundEffect>("Sound/GameOver").Play();
                WasNotPlayed = false;
            }

            if (VirtualGamePad.Is(Cancel, Pressed))
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
         //   GameManager.Logger.PrintAverages();
        }
    }
}
