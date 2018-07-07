using System;
using System.Diagnostics;
using System.Net;
using Game.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using static Game.Menu.GameManager;
using static Game.Menu.OutlivedStates;
using static Game.Menu.States.MainMenu.OptionsState;

namespace Game.Menu.States
{
    /// <summary>
    /// The main game menu state. This state is used as the
    /// state that the users come to when starting the game
    /// (exl. intro state). It presents options that the players
    /// can choose from.
    /// </summary>
    class MainMenu : IMenu
    {
        private const string textCredits = "CREDITS";
        private const string textContinue = "PLAY IF YOU DARE";
        private const string textAbout = "WHAT HAPPENED?";
        private const string textExit = "TAKE THE EASY WAY OUT";

        public VirtualGamePad VirtualGamePad { get; }
        public MenuNavigator MenuNavigator { get; }
        public GenericButtonNavigator<OptionsState> MenuPosition { get; set; }

        private GameManager gm;
        private SidewaysBackground fogBackground;

        internal enum OptionsState
        {
            Continue,
            About,
            Credits,
            Exit
        }

        public MainMenu(GameManager gameManager, VirtualGamePad virtualGamePad, MenuNavigator menuNavigator)
        {
            VirtualGamePad = virtualGamePad;
            MenuNavigator = menuNavigator;
            gm = gameManager;

            MenuPosition = new GenericButtonNavigator<OptionsState>(new[] { Continue, About, OptionsState.Credits, Exit });
            fogBackground = new SidewaysBackground(AssetManager.Instance.Get<Texture2D>("Images/Menu/movingfog"), new Vector2(20, 20), 1f);
        }

        private void DisplayGameOptions(SpriteBatch sb)
        {
            sb.Draw(AssetManager.Instance.Get<Texture2D>("Images/Menu/mainoptions"), gm.viewport.Bounds, Color.White);
            fogBackground.Draw(sb);
            sb.DrawString(AssetManager.Instance.Get<SpriteFont>("Fonts/ZMenufont"), textContinue, new Vector2(600, gm.viewport.Height * 0.45f), Color.White);
            sb.DrawString(AssetManager.Instance.Get<SpriteFont>("Fonts/ZMenufont"), textAbout, new Vector2(600, gm.viewport.Height * 0.55f), Color.White);
            sb.DrawString(AssetManager.Instance.Get<SpriteFont>("Fonts/ZMenufont"), textCredits, new Vector2(600, gm.viewport.Height * 0.65f), Color.White);
            sb.DrawString(AssetManager.Instance.Get<SpriteFont>("Fonts/ZMenufont"), textExit, new Vector2(600, gm.viewport.Height * 0.75f), Color.White);

            switch (MenuPosition.CurrentPosition)
            {
                case Continue:
                    sb.Draw(AssetManager.Instance.Get<Texture2D>("Images/Keyboard/enter"), new Vector2(250, gm.viewport.Height * 0.42f), Color.White);
                    break;
                case About:
                    sb.Draw(AssetManager.Instance.Get<Texture2D>("Images/Keyboard/enter"), new Vector2(250, gm.viewport.Height * 0.52f), Color.White);
                    break;
                case OptionsState.Credits:
                    sb.Draw(AssetManager.Instance.Get<Texture2D>("Images/Keyboard/enter"), new Vector2(250, gm.viewport.Height * 0.62f), Color.White);
                    break;
                case Exit:
                    sb.Draw(AssetManager.Instance.Get<Texture2D>("Images/Keyboard/enter"), new Vector2(250, gm.viewport.Height * 0.72f), Color.White);
                    break;
            }
        }

        // Draws
        public void Draw(GameTime gameTime, SpriteBatch sb)
        {
            sb.Begin();
            gm.effects.DrawExpandingEffect(sb, AssetManager.Instance.Get<Texture2D>("Images/Menu/background3"));
            DisplayGameOptions(sb);
            sb.End();
        }

        public void Update(GameTime gameTime)
        {
            fogBackground.Update(gameTime, new Vector2(1, 0), gm.viewport);

            if (MediaPlayer.State == MediaState.Stopped)
            {
                MediaPlayer.Volume = 0.8f;
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Play(AssetManager.Instance.Get<Song>("Sound/bg_menumusic"));
            }

            MenuPosition.UpdatePosition(VirtualGamePad);

            if (VirtualGamePad.Is(VirtualGamePad.MenuKeys.Accept, VirtualGamePad.MenuKeyStates.Pressed))
            {
                AssetManager.Instance.Get<SoundEffect>("sound/click2").Play();
                switch (MenuPosition.CurrentPosition)
                {
                    case Continue:
                        MenuNavigator.GoTo(GameState.GameModesMenu);
                        break;
                    case About:
                        MenuNavigator.GoTo(GameState.About);
                        break;
                    case OptionsState.Credits:
                        MenuNavigator.GoTo(GameState.Credits);
                        break;
                    case Exit:
                        MenuNavigator.GoTo(GameState.Quit);
                        break;
                }
            }
        }

        public void Reset()
        {
        }
    }
}
