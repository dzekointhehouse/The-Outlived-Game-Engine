using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Components.RenderComponent;
using Spelkonstruktionsprojekt.ZEngine.Wrappers;
using ZEngine.Components;
using ZEngine.Managers;

namespace ZEngine.Systems
{
    class CameraSceneSystem : ISystem
    {
        private ComponentManager ComponentManager = ComponentManager.Instance;

        public void Update(GameTime gameTime)
        {
            UpdateCameraPosition(gameTime);
            UpdateFixedRenderables();
        }

        private void UpdateFixedRenderables()
        {
            var camera = ComponentManager.GetEntitiesWithComponent<CameraViewComponent>().First();

            var fixedRenderables = 
                ComponentManager.GetEntitiesWithComponent<RenderComponent>()
                    .Where(entity => 
                        entity.Value.Fixed
                        && ComponentManager.Instance.EntityHasComponent<RenderOffsetComponent>(entity.Key)
                    );

            foreach (var fixedEntity in fixedRenderables)
            {
                var offsetComponent = ComponentManager.GetEntityComponent<RenderOffsetComponent>(fixedEntity.Key);
                offsetComponent.Offset.X = camera.Value.View.X;
                offsetComponent.Offset.Y = camera.Value.View.Y;

                Debug.WriteLine("Offset " + offsetComponent.Offset);
            }
        }

        private void UpdateCameraPosition(GameTime gameTime)
        {
            var delta = gameTime.ElapsedGameTime.TotalSeconds;
            var followEntities = ComponentManager.GetEntitiesWithComponent<CameraFollowComponent>();
            var cameras = ComponentManager.GetEntitiesWithComponent<CameraViewComponent>();

            Vector2 averagePosition = new Vector2(0, 0);

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
                float cameraSpeed = (float)(5 * delta);
                var ratioY = (float)camera.View.Width / (float)camera.View.Height;
                var ratioX = (float)camera.View.Height / (float)camera.View.Width;
                Vector2 speed = new Vector2(cameraSpeed * ratioX, cameraSpeed * ratioY);
                var oldPosition = new Vector2(camera.View.X, camera.View.Y);
                var newPosition = oldPosition + direction * speed;
                camera.View = new Rectangle((int)Math.Ceiling(newPosition.X), (int)Math.Ceiling(newPosition.Y), camera.View.Width, camera.View.Height);


                //camera.Origin = new Vector2(camera.View.Width / 2, camera.View.Height / 2);

                //// Using a matrix makes it easier for us to move the camera
                //// independently of all the sprites, which means that we easily can
                //// rotate, scale, etc. without much effort. plus its recommended.

                //// What we do when are multiplying matrices is that we combine them
                //// so the result will be a matrix that does the combination of it's 
                //// products. Now when we use this transform in the begindraw, it will
                //// affect all the stuff that is drawn after it.
                //camera.Transform = Matrix.Identity *

                //    // We create a translation matrix so we are able to move our points easily 
                //    // from one place to another. 
                //    // X,Y and Z, ofcourse Z will be 0.
                //    Matrix.CreateTranslation((float)-cameraRenderComponent.PositionComponent.Position.X, (float)-cameraRenderComponent.PositionComponent.Position.Y, 0) *

                //    // We won't be having any rotation.
                //    Matrix.CreateRotationZ(0) * 
                //    Matrix.CreateTranslation(camera.Origin.X, camera.Origin.Y, 0) *

                //    // Our zoom effect will be doing its jobb here,
                //    // as this matrix will easily help us achieve it.
                //    Matrix.CreateScale(new Vector3(camera.Scale, camera.Scale, camera.Scale));

            }
        }
    }
}
