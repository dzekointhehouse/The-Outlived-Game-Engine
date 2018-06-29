using Game.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Game.Services.VirtualGamePad.MenuKeys;
using static Game.Services.VirtualGamePad.MenuKeyStates;

namespace Game.Menu.States
{
    /// <summary>
    /// Pause game state, when the user want's to
    /// pause the game.
    /// </summary>
    class AboutMenu : IMenu
    {
        private readonly GameManager gm;
        private MenuNavigator MenuNavigator { get; }
        public VirtualGamePad VirtualGamePad { get; }
        private Viewport viewport;

        public AboutMenu(GameManager gameManager, MenuNavigator menuNavigator, VirtualGamePad virtualGamePad)
        {
            MenuNavigator = menuNavigator;
            VirtualGamePad = virtualGamePad;
            gm = gameManager;
            viewport = OutlivedGame.Instance().graphicsDeviceManager.GraphicsDevice.Viewport;
        }

        // drawing the menu background.
        public void Draw(GameTime gameTime, SpriteBatch sb)
        {
            sb.Begin();
            gm.effects.DrawExpandingEffect(sb, gm.MenuContent.Background);
            sb.Draw(gm.MenuContent.AboutBackground, viewport.Bounds, Color.White);
            sb.End();
        }
        // A pause button that goes to the pause game state,
        // but if the current game state is the pause state
        // then we go back to the previous state.
        public void Update(GameTime gameTime)
        {
            if (VirtualGamePad.Is(Cancel, Pressed))
            {
                MenuNavigator.GoBack();
            }
        }

        public void Reset()
        {
        }
    }
}
