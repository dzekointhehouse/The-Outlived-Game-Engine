using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Components.RenderComponent;
using Spelkonstruktionsprojekt.ZEngine.Managers;
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

            KeyValuePair<int, IComponent> camera = ComponentManager.GetEntitiesWithComponent(typeof(CameraViewComponent)).First();

            IEnumerable<KeyValuePair<int, IComponent>> fixedRenderables =
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


                var cameraViewComponent = camera.Value as CameraViewComponent;
                positionComponent.Position = new Vector2(
                    cameraViewComponent.View.X + offsetComponent.Offset.X,
                    cameraViewComponent.View.Y + offsetComponent.Offset.Y
                );
            }
        }

        private void UpdateCameraPosition(GameTime gameTime)
        {
            double delta = gameTime.ElapsedGameTime.TotalSeconds;
            Dictionary<int, IComponent> followEntities = ComponentManager.GetEntitiesWithComponent(typeof(CameraFollowComponent));
            Dictionary<int, IComponent> cameras = ComponentManager.GetEntitiesWithComponent(typeof(CameraViewComponent));

            Vector2 averagePosition = new Vector2(0, 0);

            foreach (var entity in followEntities)
            {
                PositionComponent pos = ComponentManager.GetEntityComponentOrDefault<PositionComponent>(entity.Key);
                averagePosition += pos.Position;
            }

            averagePosition /= followEntities.Count;

            // Remember: Change the camera scale depending on the distance of the farthest
            // players. And if the distance gets smaller we should increase scale.

            foreach (var cameraEntity in cameras)
            {
                CameraViewComponent camera = cameraEntity.Value as CameraViewComponent;
                Point screenCenter = camera.View.Center;
                PositionComponent cameraPositionComponent =
                    ComponentManager.GetEntityComponentOrDefault<PositionComponent>(cameraEntity.Key);

                //Setting the position of the red dot (for debugging camera follow of multiple entities)
                cameraPositionComponent.Position = averagePosition;

                Vector2 centerVector = new Vector2(screenCenter.X, screenCenter.Y);
                Vector2 direction = averagePosition - centerVector;
                float cameraSpeed = (float) (5 * delta);
                float ratioY = camera.View.Width / (float) camera.View.Height;
                float ratioX = camera.View.Height / (float) camera.View.Width;
                Vector2 speed = new Vector2(cameraSpeed * ratioX, cameraSpeed * ratioY);
                Vector2 oldPosition = new Vector2(camera.View.X, camera.View.Y);
                Vector2 newPosition = oldPosition + direction * speed;
                camera.View = new Rectangle((int) Math.Ceiling(newPosition.X), (int) Math.Ceiling(newPosition.Y),
                    camera.View.Width, camera.View.Height);

                //Debug.WriteLine("CAMERA POSITION " + new Vector2(camera.View.X, camera.View.Y));
            }
        }
    }
}