using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZEngine.Managers;
using ZEngine.Components;
using ZEngine.Components.CollisionComponent;
using Spelkonstruktionsprojekt.ZEngine.Components;
using ZEngine.Wrappers;

namespace Spelkonstruktionsprojekt.ZEngine.Systems
{
    class CollisionSystem : ISystem
    {
        private readonly ComponentManager ComponentManager = ComponentManager.Instance;



        public void addBoxes()
        {
            var collisionEntities = ComponentManager.GetEntitiesWithComponent<CollisionComponent>();
            foreach (int key in collisionEntities.Keys)
            {
                var componentList = ComponentManager.GetComponentsWithEntity(key);

                RenderComponent position = (RenderComponent)componentList[typeof(RenderComponent)];
                CollisionComponent collisionComponent = (CollisionComponent)componentList[typeof(CollisionComponent)];

                collisionComponent.spriteBoundingRectangle = new Rectangle((int)position.PositionComponent.Position.X, (int)position.PositionComponent.Position.Y, position.DimensionsComponent.Width, position.DimensionsComponent.Height);
            }
        }

        public void checkCol()
        {
            var collisionEntities = ComponentManager.GetEntitiesWithComponent<CollisionComponent>();
            foreach (int key in collisionEntities.Keys)
            {
                var componentList = ComponentManager.GetComponentsWithEntity(key);
                CollisionComponent collisionComponent = (CollisionComponent)componentList[typeof(CollisionComponent)];

                foreach (int key2 in collisionEntities.Keys)
                {
                    var secondComponentList = ComponentManager.GetComponentsWithEntity(key2);
                    CollisionComponent secondCollisionComponent = (CollisionComponent)secondComponentList[typeof(CollisionComponent)];

                    if ((collisionComponent.spriteBoundingRectangle.Intersects(secondCollisionComponent.spriteBoundingRectangle)) && (key != key2))
                    {
                        Console.Write("Collision was detected. Go do something about it.");
                    }

                }
            }

        }



        
        // stops the sprite from going off the screen
        public void Boundering(SpriteComponent sprite, GraphicsDeviceManager graphics)
        {                          
            int x = MathHelper.Clamp(sprite.Position.X, graphics.GraphicsDevice.Viewport.X, graphics.GraphicsDevice.Viewport.Width - sprite.Width);
            int y= MathHelper.Clamp(sprite.Position.Y, graphics.GraphicsDevice.Viewport.Y, graphics.GraphicsDevice.Viewport.Height - sprite.Height);

            sprite.Position = new Point(x, y);
            
        }
    }
}
