﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ZEngine.Wrappers;

namespace Game.Menu.States
{
    /// <summary>
    /// Pause game state, when the user want's to
    /// pause the game.
    /// </summary>
    class PausedMenu : IMenu
    {
        private readonly GameManager gameManager;
        private readonly ControlsConfig controls;
        private Viewport viewport;
        private SpriteBatch sb = GameDependencies.Instance.SpriteBatch;

        public PausedMenu(GameManager gameManager)
        {
            this.gameManager = gameManager;
            controls = new ControlsConfig(gameManager);
            viewport = this.gameManager.Engine.Dependencies.GraphicsDeviceManager.GraphicsDevice.Viewport;
        }

        // drawing the menu background.
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            sb.Begin();
            sb.Draw(gameManager.GameContent.PauseBackground, viewport.Bounds, Color.White);
            sb.End();
        }
        // A pause button that goes to the pause game state,
        // but if the current game state is the pause state
        // then we go back to the previous state.
        public void Update(GameTime gameTime)
        {
            controls.PauseButton();
        }
    }
}