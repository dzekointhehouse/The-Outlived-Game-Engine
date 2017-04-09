﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ZEngine.Components;
using ZEngine.Managers;
using ZEngine.Systems;
using ZEngine.EventBus;
using ZEngine.Wrappers;

namespace Spelkonstruktionsprojekt.ZEngine.GameTest
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class TestGame : Game
    {
        private EventBus EventBus = EventBus.Instance;
        private RenderDependencies RenderDependencies = new RenderDependencies();
        private List<ISystem> systems = new List<ISystem>();

        public TestGame()
        {
            RenderDependencies.GraphicsDeviceManager = new GraphicsDeviceManager(this);
            RenderDependencies.GraphicsDeviceManager.PreferredBackBufferWidth = 900;
            RenderDependencies.GraphicsDeviceManager.PreferredBackBufferHeight = 500;
            Content.RootDirectory = "Content";
            systems.Add(SystemManager.Instance.CreateSystem("Render").Start());
            systems.Add(SystemManager.Instance.CreateSystem("LoadContent").Start());

            var entity = EntityManager.GetEntityManager().NewEntity();
            var renderComponent = new RenderComponent()
            {
                DimensionsComponent = new DimensionsComponent() { Width = 100, Height = 100 },
                PositionComponent = new PositionComponent() { X = 100, Y = 100 }
            };
            ComponentManager.Instance.AddComponentToEntity(renderComponent, entity);

            var spriteComponent = new SpriteComponent()
            {
                SpriteName = "java"
            };
            ComponentManager.Instance.AddComponentToEntity(spriteComponent, entity);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            RenderDependencies.SpriteBatch = new SpriteBatch(GraphicsDevice);
            EventBus.Publish("LoadContent", this.Content);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            EventBus.Publish("Render", RenderDependencies);

            base.Draw(gameTime);
        }
    }
}