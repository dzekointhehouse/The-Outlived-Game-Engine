using Game.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spelkonstruktionsprojekt.ZEngine.Managers;
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
            viewport = OutlivedGame.Instance().graphics.GraphicsDevice.Viewport;
        }

        // drawing the menu background.
        public void Draw(GameTime gameTime, SpriteBatch sb)
        {
            sb.Begin();
            gm.effects.DrawExpandingEffect(sb, AssetManager.Instance.Get<Texture2D>("Images/Menu/background3"));
            sb.Draw(AssetManager.Instance.Get<Texture2D>("Images/Menu/about"), viewport.Bounds, Color.White);
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
