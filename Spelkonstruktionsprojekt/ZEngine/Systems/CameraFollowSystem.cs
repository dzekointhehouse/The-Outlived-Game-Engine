using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Wrappers;
using ZEngine.Components;
using ZEngine.Managers;

namespace ZEngine.Systems
{
    class CameraFollowSystem : ISystem
    {
        private ComponentManager ComponentManager = ComponentManager.Instance;

        public void FollowCamera(GameTime gameTime)
        {
            var delta = gameTime.ElapsedGameTime.TotalSeconds;
            var followEntities = ComponentManager.GetEntitiesWithComponent<CameraFollowComponent>();
            var cameras = ComponentManager.GetEntitiesWithComponent<CameraViewComponent>();

            Vector2 averagePosition = new Vector2(0,0);

            foreach (var entity in followEntities)
            {
                if (ComponentManager.EntityHasComponent<RenderComponent>(entity.Key))
                {
                    var comp = ComponentManager.GetEntityComponent<RenderComponent>(entity.Key);
                    averagePosition += comp.PositionComponent.Position;
                }
            }


            averagePosition /= followEntities.Count;

            // Remember: Change the camera scale depending on the distance of the farthest
            // players. And if the distance gets smaller we should increase scale.

            foreach (var cameraEntity in cameras)
            {
                var camera = cameraEntity.Value;
                // camera.Value.Position = new Vector2((float)Xpositions.Average(), (float)Ypositions.Average());
                Point screenCenter = camera.View.Center;
                var cameraRenderComponent = ComponentManager.GetEntityComponent<RenderComponent>(cameraEntity.Key);
                cameraRenderComponent.PositionComponent.Position = averagePosition;
                var centerVector = new Vector2(screenCenter.X, screenCenter.Y);

                var direction = averagePosition - centerVector;
                float cameraSpeed = (float) (5 * delta);
                var ratioY = (float)camera.View.Width / (float)camera.View.Height;
                var ratioX = (float)camera.View.Height / (float)camera.View.Width;
                Vector2 speed = new Vector2(cameraSpeed * ratioX, cameraSpeed * ratioY);
                var oldPosition = new Vector2(camera.View.X, camera.View.Y);
                var newPosition = oldPosition + direction * speed;
                var newPosition2 = oldPosition + direction * speed;
                camera.View = new Rectangle((int) Math.Ceiling(newPosition.X), (int) Math.Ceiling(newPosition.Y), camera.View.Width, camera.View.Height);
            }
        }
    }
}
