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
        private Viewport viewport;
        private MenuNavigator MenuNavigator { get; }
        public VirtualGamePad VirtualGamePad { get; }
        private GenericButtonNavigator<GameModes> MenuPosition;
        private readonly GameManager gm;

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
            this.gm = gameManager;
            this.viewport = gameManager.viewport;
            fogBackground = new SidewaysBackground(gameManager.MenuContent.BackgroundFog, new Vector2(20, 20), 1f);
        }

        private void MainMenuDisplay(SpriteBatch sb)
        {
            

            switch (MenuPosition.CurrentPosition)
            {
                case Survival:
                    sb.Draw(gm.MenuContent.GameModeHiglightSurvival, viewport.Bounds, Color.White);
                    break;
                case Extinction:

                    sb.Draw(gm.MenuContent.GameModeHiglightExtinction, viewport.Bounds, Color.White);
                    break;
                case Blockworld:
                    sb.Draw(gm.MenuContent.GameModeHiglightBlockworld, viewport.Bounds, Color.White);
                    break;
            }

        }

        public void Draw(GameTime gameTime, SpriteBatch sb)
        {
            sb.Begin();
            gm.effects.DrawExpandingEffect(sb, gm.MenuContent.Background);
            fogBackground.Draw(sb);
            MainMenuDisplay(sb);
            sb.End();
        }

        public void Update(GameTime gameTime)
        {
            fogBackground.Update(gameTime, new Vector2(1, 0), gm.viewport);
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
                    gm.MenuContent.ClickSound.Play();
                    gm.gameConfig.GameMode = Survival;
                    break;
                case Extinction:
                    gm.MenuContent.ClickSound.Play();
                    gm.gameConfig.GameMode = Extinction;
                    break;
                case Exit:
                    gm.MenuContent.ClickSound.Play();
                    break;
                case Blockworld:
                    gm.MenuContent.ClickSound.Play();
                    gm.gameConfig.GameMode = Blockworld;
                    break;
            }

            MenuNavigator.GoTo(GameManager.GameState.MultiplayerMenu);
        }

        public void Reset()
        {
            gm.gameConfig.Reset();
        }
    }
}
