using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Penumbra;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Systems;
using ZEngine.Managers;
using ZEngine.Wrappers;

namespace Spelkonstruktionsprojekt.ZEngine.GameTest
{
    class TestGame2 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        readonly GameDependencies gameDependencies = new GameDependencies();
        private LightSystem lightSystems;
        private PenumbraComponent penumbraComponent;

        public TestGame2()
        {
            graphics = new GraphicsDeviceManager(this);
            gameDependencies.GraphicsDeviceManager = graphics;
            Content.RootDirectory = "Content";
        }


        protected override void Initialize()
        {

            // TODO: Add your initialization logic here
            lightSystems = SystemManager.Instance.GetSystem<LightSystem>();


            base.Initialize();
        }


        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.

            gameDependencies.SpriteBatch = new SpriteBatch(GraphicsDevice);

            gameDependencies.GameContent = this.Content;
            
            gameDependencies.Game = this;

            CreateObjects();

            penumbraComponent = lightSystems.Initialize(gameDependencies);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {


            lightSystems.DrawLights(penumbraComponent);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            lightSystems.EndDraw(penumbraComponent);

            base.Draw(gameTime);
        }


        private void CreateObjects()
        {
            var obj1 = EntityManager.GetEntityManager().NewEntity();

            var light = new LightComponent()
            {
                Light = new PointLight()
                {
                    Position = new Vector2(50, 50),
                    Scale = new Vector2(500f),
                    ShadowType = ShadowType.Solid // Will not lit hulls themselves

                }
            };

            ComponentManager.Instance.AddComponentToEntity(light,obj1);
        }


    }
}
