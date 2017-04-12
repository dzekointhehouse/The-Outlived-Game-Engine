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

        
        public void checkforCollision(GameDependencies rd)
        {

            var collisionEntities = ComponentManager.GetEntitiesWithComponent<CollisionComponent>();
            foreach(int key in collisionEntities.Keys)
            {
                var componentList = ComponentManager.GetComponentsWithEntity(key);
                if (ComponentManager.EntityHasComponent<PositionComponent>(key)) { }
                PositionComponent position = (PositionComponent) componentList[typeof(PositionComponent)];
                CollisionComponent collisionComponent = (CollisionComponent) componentList[typeof(CollisionComponent)];
                collisionComponent.spriteBoundingRectangle = new Rectangle((int)position.Position.X, (int)position.Position.Y, 50,50);

                foreach(int key2 in collisionEntities.Keys)
                {
                    var componentList2 = ComponentManager.GetComponentsWithEntity(key2);
                    PositionComponent position2 = (PositionComponent)componentList[typeof(PositionComponent)];
                    CollisionComponent collisionComponent2 = (CollisionComponent)componentList[typeof(CollisionComponent)];
                    collisionComponent2.spriteBoundingRectangle = new Rectangle((int)position.Position.X, (int)position.Position.Y, 50, 50);

                    if(collisionComponent.spriteBoundingRectangle.Intersects(collisionComponent2.spriteBoundingRectangle) && !collisionComponent2.Equals(collisionComponent))
                    {
                        rd.GraphicsDeviceManager.GraphicsDevice.Clear(Color.Red);
                    }

                        
                }



                

            }

            //ComponentManager.GetComponentsWithEntity();
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
