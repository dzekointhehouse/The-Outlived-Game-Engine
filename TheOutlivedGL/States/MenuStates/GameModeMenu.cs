using Game.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using System.Linq;
using static Game.Menu.OutlivedStates;
using static Game.Services.VirtualGamePad.MenuKeys;
using static Game.Services.VirtualGamePad.MenuKeyStates;

namespace Game.Menu.States
{
    public class GameModeMenu : IMenu
    {
        private SidewaysBackground fogBackground;
        private Viewport viewport;
        private OptionNavigator<GameState> navigator;
        private readonly GameManager gameManager;

        public GameModeMenu(GameManager gameManager)
        {
            navigator = new OptionNavigator<GameState>(new []{ GameState.SurvivalGame, GameState.PlayDeathMatchGame });
            this.gameManager = gameManager;
            this.viewport = gameManager.viewport;
            fogBackground = new SidewaysBackground(AssetManager.Instance.Get<Texture2D>("Images/Menu/movingfog"), new Vector2(20, 20), 1f);
        }

        private void MainMenuDisplay(SpriteBatch sb)
        {
            switch (navigator.CurrentPosition)
            {
                case GameState.SurvivalGame:
                    sb.Draw(AssetManager.Instance.Get<Texture2D>("Images/Menu/gamemodemenu_hs"), viewport.Bounds, Color.White);
                    break;
                case GameState.PlayExtinctionGame:
                    sb.Draw(AssetManager.Instance.Get<Texture2D>("Images/Menu/gamemodemenu_he"), viewport.Bounds, Color.White);
                    break;
                case GameState.PlayDeathMatchGame:
                    sb.Draw(AssetManager.Instance.Get<Texture2D>("Images/Menu/gamemodemenu_hb"), viewport.Bounds, Color.White);
                    break;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch sb)
        {
            sb.Begin();
            gameManager.effects.DrawExpandingEffect(sb, AssetManager.Instance.Get<Texture2D>("Images/Menu/background3"));
            fogBackground.Draw(sb);
            MainMenuDisplay(sb);
            sb.End();
        }

        public void Update(GameTime gameTime)
        {
            fogBackground.Update(gameTime, new Vector2(1, 0), gameManager.viewport);
            if (gameManager.playerControllers.Controllers.Any(c => c.Is(Cancel, Pressed)))
            {
                gameManager.MenuNavigator.GoBack();
            }

            gameManager.playerControllers.Controllers.ForEach(c => navigator.UpdatePosition(c));

            if (gameManager.playerControllers.Controllers.Any(c => c.Is(Accept, Pressed)))
            {
                HandleStartGameMode();
            }
        }

        public void HandleStartGameMode()
        {
            switch (navigator.CurrentPosition)
            {
                case GameState.SurvivalGame:
                    AssetManager.Instance.Get<SoundEffect>("sound/click2").Play();
                    gameManager.gameConfig.GameMode = GameState.SurvivalGame;
                    break;
                case GameState.PlayExtinctionGame:
                    AssetManager.Instance.Get<SoundEffect>("sound/click2").Play();
                    gameManager.gameConfig.GameMode = GameState.PlayExtinctionGame;
                    break;
                case GameState.Quit:
                    AssetManager.Instance.Get<SoundEffect>("sound/click2").Play();
                    break;
                case GameState.PlayDeathMatchGame:
                    AssetManager.Instance.Get<SoundEffect>("sound/click2").Play();
                    gameManager.gameConfig.GameMode = GameState.PlayDeathMatchGame;
                    break;
            }
            gameManager.MenuNavigator.GoTo(GameState.MultiplayerMenu);
        }

        public void Reset()
        {
            gameManager.gameConfig.Reset();
        }
    }
}
