using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Color = Microsoft.Xna.Framework.Color;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Vector3 = Microsoft.Xna.Framework.Vector3;

namespace Spelkonstruktionsprojekt.ZEngine.Systems
{
    // Optimus prime
    public class FlashlightSystem : ISystem
    {
        public static string SystemName = "FlashlightSystem";

        // This method is used to initialize the penumbra instance, and add
        // all the entities that have an associated instance of light component.
        public PenumbraComponent LoadPenumbra(GameDependencies gameDependencies)
        {
            var penumbra = new PenumbraComponent(gameDependencies.Game)
            {
                // Ambient color will determine how dark everything else
                // except for the light will be.
                AmbientColor = new Color(new Vector3(0.05f)) // should be an entity?
            };

            foreach (var barrelFlash in ComponentManager.Instance.GetEntitiesWithComponent(typeof(BarrelFlashComponent)))
            {
                var barrelFlashComponent = barrelFlash.Value as BarrelFlashComponent;
                penumbra.Lights.Add(barrelFlashComponent.Light);
            }
            
            foreach (var instance in ComponentManager.Instance.GetEntitiesWithComponent(typeof(LightComponent)))
            {
                var lightComponent = instance.Value as LightComponent;
                penumbra.Lights.Add(lightComponent.Light);
            }

            foreach (var hullComponent in ComponentManager.Instance.GetEntitiesWithComponent(typeof(HullComponent)))
            {
                var hull = hullComponent.Value as HullComponent;
                var positionComponent =
                    ComponentManager.Instance.GetEntityComponentOrDefault<PositionComponent>(hullComponent.Key);

                hull.Hull.Position = positionComponent.Position;
                hull.Hull.Enabled = true;
                penumbra.Hulls.Add(hull.Hull);
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

            foreach (var barrelFlash in ComponentManager.Instance
                .GetEntitiesWithComponent(typeof(BarrelFlashComponent)))
            {
                var lightComponent = barrelFlash.Value as BarrelFlashComponent;

                // If it has no render component than we should skip this entity.
                if (!ComponentManager.Instance.EntityHasComponent<RenderComponent>(barrelFlash.Key))
                    continue;

                var positionComponent = ComponentManager.Instance.GetEntityComponentOrDefault<PositionComponent>(barrelFlash.Key);
                var dimensionsComponent = ComponentManager.Instance.GetEntityComponentOrDefault<DimensionsComponent>(barrelFlash.Key);
                if(dimensionsComponent == null) continue;
                var moveComponent =
                    ComponentManager.Instance.GetEntityComponentOrDefault<MoveComponent>(barrelFlash.Key);
                if (moveComponent == null) continue;

                //TODO MOVE BULLET OFFSETS TO A GLOBALLY ACCESSABLE AREA
                var barrelOffsetX = 65;
                var barrelOffsetY = 52;
                var transformation =
                    Matrix.CreateTranslation(new Vector3(
                        (float) (-positionComponent.Position.X - dimensionsComponent.Width * 0.5 + barrelOffsetX),
                        (float) (-positionComponent.Position.Y - dimensionsComponent.Height * 0.5 + barrelOffsetY), 0)) *
                    Matrix.CreateRotationZ(moveComponent.Direction) *
                    Matrix.CreateTranslation(positionComponent.Position.X, positionComponent.Position.Y, 0f);

                lightComponent.Light.Position =
                    Vector2.Transform(
                        new Vector2(positionComponent.Position.X + 50, positionComponent.Position.Y + 24),
                        transformation);
                lightComponent.Light.Rotation = (float) moveComponent.Direction;


            }
            foreach (var lightEntity in ComponentManager.Instance.GetEntitiesWithComponent(typeof(LightComponent)))
            {
                var lightComponent = lightEntity.Value as LightComponent;
//                // If it has no render component than we should skip this entity.
//                if (!ComponentManager.Instance.EntityHasComponent<RenderComponent>(lightEntity.Key))
//                    continue;

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
            //GlobalSpawn
            var GlobalSpawnEntities =
                ComponentManager.Instance.GetEntitiesWithComponent(typeof(GlobalSpawnComponent));
            if (GlobalSpawnEntities.Count <= 0) return;

            var GlobalSpawnComponent =
                ComponentManager.Instance.GetEntityComponentOrDefault<GlobalSpawnComponent>(GlobalSpawnEntities.First().Key);

            


            var camera = ComponentManager.Instance.GetEntitiesWithComponent(typeof(CameraViewComponent));
            if (camera.Count() > 1 || !camera.Any() || GlobalSpawnComponent.EnemiesDead)
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
            //GlobalSpawn
            var GlobalSpawnEntities =
                ComponentManager.Instance.GetEntitiesWithComponent(typeof(GlobalSpawnComponent));
            if (GlobalSpawnEntities.Count <= 0) return;

            var GlobalSpawnComponent =
                ComponentManager.Instance.GetEntityComponentOrDefault<GlobalSpawnComponent>(GlobalSpawnEntities.First().Key);



            // E dont do the draw call if there is not just one camera.
            var cameras = ComponentManager.Instance.GetEntitiesWithComponent(typeof(CameraViewComponent));
            if (cameras.Count() > 1 || !cameras.Any() || GlobalSpawnComponent.EnemiesDead)
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
