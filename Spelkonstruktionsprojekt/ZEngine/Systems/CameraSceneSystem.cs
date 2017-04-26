using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Components.RenderComponent;
using ZEngine.Components;
using ZEngine.Managers;

namespace ZEngine.Systems
{
    class CameraSceneSystem : ISystem
    {
        private readonly ComponentManager ComponentManager = ComponentManager.Instance;

        // Update call so the camera follows the followable
        // entities.
        public void Update(GameTime gameTime)
        {
            UpdateCameraPosition(gameTime);
            UpdateFixedRenderables();
        }

        private void UpdateFixedRenderables()
        {
            var camera = ComponentManager.GetEntitiesWithComponent(typeof(CameraViewComponent)).First();

            var fixedRenderables =
                ComponentManager.GetEntitiesWithComponent(typeof(RenderComponent))
                    .Where(entity =>
                    {
                        var renderComponent = entity.Value as RenderComponent;
                        return renderComponent.Fixed
                               && ComponentManager.Instance.EntityHasComponent<RenderOffsetComponent>(entity.Key);
                    });

            foreach (var fixedEntity in fixedRenderables)
            {
                var offsetComponent =
                    ComponentManager.GetEntityComponentOrDefault<RenderOffsetComponent>(fixedEntity.Key);
                var positionComponent =
                    ComponentManager.GetEntityComponentOrDefault<PositionComponent>(fixedEntity.Key);


                var position = positionComponent.Position;
                var cameraViewComponent = camera.Value as CameraViewComponent;
                position.X = cameraViewComponent.View.X + offsetComponent.Offset.X;
                position.Y = cameraViewComponent.View.Y + offsetComponent.Offset.Y;
            }
        }

        private void UpdateCameraPosition(GameTime gameTime)
        {
            var delta = gameTime.ElapsedGameTime.TotalSeconds;
            var followEntities = ComponentManager.GetEntitiesWithComponent(typeof(CameraFollowComponent));
            var cameras = ComponentManager.GetEntitiesWithComponent(typeof(CameraViewComponent));

            Vector2 averagePosition = new Vector2(0, 0);

            foreach (var entity in followEntities)
            {
                var pos = ComponentManager.GetEntityComponentOrDefault<PositionComponent>(entity.Key);
                averagePosition += pos.Position;
            }

            averagePosition /= followEntities.Count;

            // Remember: Change the camera scale depending on the distance of the farthest
            // players. And if the distance gets smaller we should increase scale.

            foreach (var cameraEntity in cameras)
            {
                var camera = cameraEntity.Value as CameraViewComponent;
                Point screenCenter = camera.View.Center;
                var cameraPositionComponent =
                    ComponentManager.GetEntityComponentOrDefault<PositionComponent>(cameraEntity.Key);

                //Setting the position of the red dot (for debugging camera follow of multiple entities)
                cameraPositionComponent.Position = averagePosition;

                var centerVector = new Vector2(screenCenter.X, screenCenter.Y);
                var direction = averagePosition - centerVector;
                float cameraSpeed = (float) (5 * delta);
                var ratioY = (float) camera.View.Width / (float) camera.View.Height;
                var ratioX = (float) camera.View.Height / (float) camera.View.Width;
                Vector2 speed = new Vector2(cameraSpeed * ratioX, cameraSpeed * ratioY);
                var oldPosition = new Vector2(camera.View.X, camera.View.Y);
                var newPosition = oldPosition + direction * speed;
                camera.View = new Rectangle((int) Math.Ceiling(newPosition.X), (int) Math.Ceiling(newPosition.Y),
                    camera.View.Width, camera.View.Height);

                //Debug.WriteLine("CAMERA POSITION " + new Vector2(camera.View.X, camera.View.Y));
            }
        }
    }
}