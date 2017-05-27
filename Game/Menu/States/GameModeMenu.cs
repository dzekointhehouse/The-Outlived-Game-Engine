using System;
using System.Linq.Expressions;
using Game.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZEngine.Wrappers;
using static Game.GameManager.GameState;
using static Game.Menu.States.GameModeMenu.GameModes;
using static Game.Services.VirtualGamePad.MenuKeys;
using static Game.Services.VirtualGamePad.MenuKeyStates;

namespace Game.Menu.States
{
    public class GameModeMenu : IMenu
    {
        private SidewaysBackground fogBackground;
        private readonly Microsoft.Xna.Framework.Game game;
        private MenuNavigator MenuNavigator { get; }
        public VirtualGamePad VirtualGamePad { get; }
        private GenericButtonNavigator<GameModes> MenuPosition;
        private readonly GameManager gameManager;

        public enum GameModes
        {
            Extinction,
            Survival,
            Blockworld,
            Exit
        }

        public GameModes[] MenuElements = {
            Extinction,
            Survival,
            Blockworld
        };

        public GameModeMenu(GameManager gameManager, MenuNavigator menuNavigator, VirtualGamePad virtualGamePad)
        {
            MenuNavigator = menuNavigator;
            VirtualGamePad = virtualGamePad;
            MenuPosition = new GenericButtonNavigator<GameModes>(MenuElements);
            this.gameManager = gameManager;
            game = this.gameManager.Engine.Dependencies.Game;
            fogBackground = new SidewaysBackground(gameManager.MenuContent.BackgroundFog, new Vector2(20, 20), 1f);
        }

        private void MainMenuDisplay()
        {
            SpriteBatch sb = GameDependencies.Instance.SpriteBatch;

            var viewport = game.GraphicsDevice.Viewport;

            switch (MenuPosition.CurrentPosition)
            {
                case Survival:
                    sb.Draw(gameManager.MenuContent.GameModeHiglightSurvival, viewport.Bounds, Color.White);
                   // sb.Draw(gameManager.MenuContent.ButtonContinue, new Vector2(250, viewport.Height * 0.45f), Color.White);
                    break;
                case Extinction:
                    sb.Draw(gameManager.MenuContent.GameModeHiglightExtinction, viewport.Bounds, Color.White);
                   // sb.Draw(gameManager.MenuContent.ButtonContinue, new Vector2(250, viewport.Height * 0.30f), Color.White);
                    break;
                case Blockworld:
                    sb.Draw(gameManager.MenuContent.GameModeHiglightBlockworld, viewport.Bounds, Color.White);
                   // sb.Draw(gameManager.MenuContent.ButtonContinue, new Vector2(250, viewport.Height * 0.55f), Color.White);
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
            if (VirtualGamePad.Is(Cancel, Pressed))
            {
                MenuNavigator.GoBack();
            }

            MenuPosition.UpdatePosition(VirtualGamePad);

            if (VirtualGamePad.Is(Accept, Pressed))
            {
                HandleStartGameMode();
            }
        }

        public void HandleStartGameMode()
        {
            switch (MenuPosition.CurrentPosition)
            {
                case Survival:
                    gameManager.MenuContent.ClickSound.Play();
                    gameManager.gameConfig.GameMode = Survival;
                    break;
                case Extinction:
                    gameManager.MenuContent.ClickSound.Play();
                    gameManager.gameConfig.GameMode = Extinction;
                    break;
                case Exit:
                    gameManager.MenuContent.ClickSound.Play();
                    break;
                case Blockworld:
                    gameManager.MenuContent.ClickSound.Play();
                    gameManager.gameConfig.GameMode = Blockworld;
                    break;
            }

            MenuNavigator.GoTo(GameManager.GameState.MultiplayerMenu);
        }

        public void Reset()
        {
            gameManager.gameConfig.Reset();
        }
    }
}
