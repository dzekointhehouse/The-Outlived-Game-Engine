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
    /// <summary>
    /// This system takes care of the calculations that
    /// are related to the camera, eg. scaling, position.
    /// The camera will adjust its position according to 
    /// the entities that possess the player and position
    /// component.
    /// </summary>
    class CameraSceneSystem : ISystem
    {
        private readonly ComponentManager ComponentManager = ComponentManager.Instance;

        public void Update(GameTime gameTime)
        {
            UpdateCamera(gameTime);
            //UpdateFixedRenderables();
        }

        private void UpdateFixedRenderables()
        {
            var cameras = ComponentManager.GetEntitiesWithComponent(typeof(CameraViewComponent));
            if (cameras.Count < 1) return;
            var camera = cameras.First();

            IEnumerable<KeyValuePair<uint, IComponent>> fixedRenderables =
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

        // Updates the cameras position and will handle the scale
        // to be used for each camera, depending on the player entity
        // distances. Current Algorithm takes the farthest plater to the
        // cameras center point.
        private void UpdateCamera(GameTime gameTime)
        {
            double delta = gameTime.ElapsedGameTime.TotalSeconds;
            var followEntities = ComponentManager.GetEntitiesWithComponent(typeof(CameraFollowComponent));
            var cameras = ComponentManager.GetEntitiesWithComponent(typeof(CameraViewComponent));

            // Average position of all the players to be used
            // when we are to calculate the cameras position.
            Vector2 averagePosition = new Vector2(0, 0);
            float maxDistance = 0;



            // Remember: Change the camera scale depending on the distance of the farthest
            // players. And if the distance gets smaller we should increase scale.

            foreach (var cameraEntity in cameras)
            {
                CameraViewComponent camera = cameraEntity.Value as CameraViewComponent;



                foreach (var entity in followEntities)
                {
                    var follow = entity.Value as CameraFollowComponent;

                    // Skip the entity if it doesn't belong to this camera.
                    if(follow.CameraId != camera.CameraId)
                        continue;

                    PositionComponent pos = ComponentManager.GetEntityComponentOrDefault<PositionComponent>(entity.Key);
                    averagePosition += pos.Position;

                    // Getting the maximum distance for scaling, to be used
                    // for the much wanted zooming effect later on.
                    float distance = Vector2.Distance(camera.Center, pos.Position);

                    maxDistance = Math.Max(distance, maxDistance);
                }

                averagePosition /= followEntities.Count;
                // set the cameras center
                camera.Center = new Vector2(averagePosition.X, averagePosition.Y);
               // camera.Center = new Vector2(averagePosition.X - (camera.ViewportDimension.X * 0.5f), averagePosition.Y - (camera.ViewportDimension.Y * 0.5f));
                // Setting the zoom to  the camera.
                if (camera.Scale <= camera.MaxScale && camera.Scale >= camera.MinScale)
                {
                    // We get an OK decimal by dividing camera dimension over itself and the max with.
                    // If we surpass the limitthen we reset the scale.
                    camera.Scale = (camera.View.Width / (maxDistance + camera.View.Width));
                    if (camera.Scale < camera.MinScale)
                        camera.Scale = camera.MinScale;
                    else if (camera.Scale > camera.MaxScale)
                        camera.Scale = camera.MaxScale;

                   // Debug.WriteLine(camera.Scale);
                }

                //    Matrix.CreateScale(new Vector3(camera.Scale, camera.Scale, camera.Scale));
                // Using a matrix makes it easier for us to move the camera
                // independently of all the sprites, which means that we easily can
                // rotate, scale, etc. without much effort, plus its recommended.
                // What we do when multiplying matrices is that we combine them
                // so the result will be a matrix that does the combination of it's 
                // products. Now when we use this transform in the begindraw, it will
                // affect all the stuff that is drawn after it.
                // We create a translation matrix so we are able to move our points easily 
                // from one place to another, and we want to translate the point according to
                // the players average position which is the center of the screen. 
                // X,Y and Z, ofcourse Z will be 0. We won't be having any rotation.
                // Our zoom effect will be doing its jobb here also , when we scale and adjust the points
                // accordingly.

                camera.Transform = Matrix.Identity *
                                   Matrix.CreateTranslation(new Vector3(-camera.Center.X, -camera.Center.Y, 0)) *
                                   Matrix.CreateScale(new Vector3(camera.Scale, camera.Scale, 0))*
                                   Matrix.CreateTranslation(new Vector3(camera.ViewportDimension.X*0.5f, camera.ViewportDimension.Y * 0.5f, 0));
            }
        }
    }
}