using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Penumbra;
using Spelkonstruktionsprojekt.ZEngine.Components;
using ZEngine.Components;
using ZEngine.Managers;
using ZEngine.Wrappers;

namespace Spelkonstruktionsprojekt.ZEngine.Systems
{
    public class FlashlightSystem : ISystem
    {
        public static string SystemName = "FlashlightSystem";

        // This method is used to initialize the penumbra instance, and add
        // all the entities that have an associated instance of light component.
        public PenumbraComponent Initialize(GameDependencies gameDependencies)
        {
            var penumbra = new PenumbraComponent(gameDependencies.Game)
            {
                // Ambient color will determine how dark everything else
                // except for the light will be.
                AmbientColor = new Color(new Vector3(1f))
            };
            var lights = ComponentManager.Instance.GetEntitiesWithComponent<LightComponent>();
            foreach (var instance in lights)
            {
                penumbra.Lights.Add(instance.Value.Light);
            }

            penumbra.Initialize();
            return penumbra;
        }

        public void Update(GameTime gameTime, Vector2 gameDimensions)
        {
            var camera = ComponentManager.Instance.GetEntitiesWithComponent<CameraViewComponent>().First();
            var cameraView = camera.Value.View;
            var lightEntities = ComponentManager.Instance.GetEntitiesWithComponent<LightComponent>();
            foreach (var lightEntity in lightEntities)
            {
                if (ComponentManager.Instance.EntityHasComponent<MoveComponent>(lightEntity.Key))
                {
                    var moveComponent = ComponentManager.Instance.GetEntityComponentOrDefault<MoveComponent>(lightEntity.Key);
                    lightEntity.Value.Light.Rotation = (float)moveComponent.Direction;
                }

                if (ComponentManager.Instance.EntityHasComponent<RenderComponent>(lightEntity.Key))
                {
                    //var renderComponent = ComponentManager.Instance.GetEntityComponentOrDefault<RenderComponent>(lightEntity.Key);
                    var positionComponent = ComponentManager.Instance.GetEntityComponentOrDefault<PositionComponent>(lightEntity.Key);
                    lightEntity.Value.Light.Position =
                        new Vector2(
                            positionComponent.Position.X - cameraView.X,
                            positionComponent.Position.Y - cameraView.Y
                            );
                }
            }
        }

        // We use begin draw to start drawing the lights that have
        // ben added to the penumbra instance. All the items that
        // are rendered betweend this BeginDraw and EndDraw will be affected.
        public void BeginDraw(PenumbraComponent penumbraComponent)
        {
            penumbraComponent.BeginDraw();
        }

        // Used to finish the penumbra instance drawing process,
        // all the items drawn after this call won't be affected.
        public void EndDraw(PenumbraComponent penumbraComponent, GameTime gameTime)
        {
            penumbraComponent.Draw(gameTime);
        }
    }
}
