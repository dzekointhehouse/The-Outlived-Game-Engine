using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ZEngine.Wrappers;
using static Game.Services.VirtualGamePad.MenuKeys;
using static Game.Services.VirtualGamePad.MenuKeyStates;

namespace Game.Menu.States
{
    /// <summary>
    /// Pause game state, when the user want's to
    /// pause the game.
    /// </summary>
    class PausedMenu : IMenu
    {
        public MenuNavigator MenuNavigator { get; }
        public VirtualGamePad VirtualGamePad { get; }
        private readonly GameManager gameManager;
        private Viewport viewport;
        private SpriteBatch sb = GameDependencies.Instance.SpriteBatch;

        public PausedMenu(GameManager gameManager, MenuNavigator menuNavigator, VirtualGamePad virtualGamePad)
        {
            MenuNavigator = menuNavigator;
            VirtualGamePad = virtualGamePad;
            this.gameManager = gameManager;
            viewport = this.gameManager.Engine.Dependencies.GraphicsDeviceManager.GraphicsDevice.Viewport;
        }

        // drawing the menu background.
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            sb.Begin();
            sb.Draw(gameManager.MenuContent.PauseBackground, viewport.Bounds, Color.White);
            sb.End();
        }
        // A pause button that goes to the pause game state,
        // but if the current game state is the pause state
        // then we go back to the previous state.
        public void Update(GameTime gameTime)
        {
            if (VirtualGamePad.Is(Pause, Pressed))
            {
                MenuNavigator.GoBack();
            }
            else if(VirtualGamePad.Is(Cancel, Pressed))
            {
                MenuNavigator.GoTo(GameManager.GameState.MainMenu);
            }
        }

        public void Reset()
        {
        }
    }
}
