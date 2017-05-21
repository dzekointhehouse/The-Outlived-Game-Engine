using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Penumbra;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using ZEngine.Components;
using ZEngine.Managers;
using ZEngine.Wrappers;

namespace Spelkonstruktionsprojekt.ZEngine.Systems
{
    // Optimus prime
    public class FlashlightSystem : ISystem
    {
        public static string SystemName = "FlashlightSystem";
        private float GameScale = 1.0f;

        // This method is used to initialize the penumbra instance, and add
        // all the entities that have an associated instance of light component.
        public PenumbraComponent LoadPenumbra(GameDependencies gameDependencies)
        {
            var penumbra = new PenumbraComponent(gameDependencies.Game)
            {
                // Ambient color will determine how dark everything else
                // except for the light will be.
                AmbientColor = new Color(new Vector3(0.5f)) // should be an entity?
            };
            var lights = ComponentManager.Instance.GetEntitiesWithComponent(typeof(LightComponent));

            foreach (var instance in lights)
            {
                var lightComponent = instance.Value as LightComponent;
                penumbra.Lights.Add(lightComponent.Light);
            }
            
            penumbra.Initialize();
            return penumbra;
        }

        // This update call will be used to update the lights position if it is
        // attached to an moving entity.
        public void Update(GameTime gameTime, Vector2 gameDimensions)
        {
            var cameras = ComponentManager.Instance.GetEntitiesWithComponent(typeof(CameraViewComponent));
            // Return if there is not only one camera, because
            // penumbra didn't work so well with several cameras.
            if (cameras.Count() > 1 || !cameras.Any()) return;

            var lightEntities = ComponentManager.Instance.GetEntitiesWithComponent(typeof(LightComponent));
            foreach (var lightEntity in lightEntities)
            {

                var lightComponent = lightEntity.Value as LightComponent;

                // If it has no render component than we should skip this entity.
                if (!ComponentManager.Instance.EntityHasComponent<RenderComponent>(lightEntity.Key))
                    continue;

                var positionComponent = ComponentManager.Instance.GetEntityComponentOrDefault<PositionComponent>(lightEntity.Key);

                lightComponent.Light.Position =
                    new Vector2(
                        //when using Matrix!
                        positionComponent.Position.X,
                        positionComponent.Position.Y
                        );

                if (ComponentManager.Instance.EntityHasComponent<MoveComponent>(lightEntity.Key))
                {
                    var moveComponent = ComponentManager.Instance.GetEntityComponentOrDefault<MoveComponent>(lightEntity.Key);
                    lightComponent.Light.Rotation = (float)moveComponent.Direction;
                }


            }
        }

        // We use begin draw to start drawing the lights that have
        // ben added to the penumbra instance. All the items that
        // are rendered betweend this BeginDraw and EndDraw will be affected.
        public void BeginDraw(PenumbraComponent penumbraComponent)
        {
            var camera = ComponentManager.Instance.GetEntitiesWithComponent(typeof(CameraViewComponent));
            if (camera.Count() > 1 || !camera.Any())
            {
                // disable penumbra if more than one camera.
                penumbraComponent.Enabled = false;
                return;
            }
           
            var cameraViewComponent = camera.First().Value as CameraViewComponent;
            // Transforming the matrix so it's in line with the transformation in rendersystem
            // where rendererable entities that have light will be transformed.
            penumbraComponent.Transform = cameraViewComponent.Transform;
            // Begin draw, where everything after this call will be 
            // affected by the penumbra light.
            penumbraComponent.BeginDraw();
        }

        // Used to finish the penumbra instance drawing process,
        // all the items drawn after this call won't be affected.
        public void EndDraw(PenumbraComponent penumbraComponent, GameTime gameTime)
        {
            // E dont do the draw call if there is not just one camera.
            var cameras = ComponentManager.Instance.GetEntitiesWithComponent(typeof(CameraViewComponent));
            if (cameras.Count() > 1 || !cameras.Any())
            {
                return;
            }
            else
            {
                penumbraComponent.Draw(gameTime);
            }

        }
    }
}
