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
using Spelkonstruktionsprojekt.ZEngine.Wrappers;
using System.Collections;

namespace Spelkonstruktionsprojekt.ZEngine.Systems
{
    class CollisionSystem : ISystem
    {
        private readonly ComponentManager ComponentManager = ComponentManager.Instance;


        public void AddBoxes()
        {
            var collisionEntities = ComponentManager.GetEntitiesWithComponent<CollisionComponent>();
            foreach (int key in collisionEntities.Keys)
            {
                var componentList = ComponentManager.GetComponentsWithEntity(key);

                RenderComponent position = (RenderComponent)componentList[typeof(RenderComponent)];
                CollisionComponent collisionComponent = (CollisionComponent)componentList[typeof(CollisionComponent)];

                collisionComponent.spriteBoundingRectangle = new Rectangle((int)position.PositionComponent.Position.X, (int)position.PositionComponent.Position.Y, position.DimensionsComponent.Width, position.DimensionsComponent.Height);
                Boundering(position.PositionComponent.Position, position.DimensionsComponent.Width, position.DimensionsComponent.Height);
            }
        }

        public void CheckOutsideCollision()
        {
            var collisionEntities = ComponentManager.GetEntitiesWithComponent<CollisionComponent>();
            foreach (int key in collisionEntities.Keys)
            {
                var componentList = ComponentManager.GetComponentsWithEntity(key);
                CollisionComponent collisionComponent = (CollisionComponent)componentList[typeof(CollisionComponent)];

                foreach (int key2 in collisionEntities.Keys)
                {
                    if(key != key2) { 
                        var secondComponentList = ComponentManager.GetComponentsWithEntity(key2);
                        CollisionComponent secondCollisionComponent = (CollisionComponent)secondComponentList[typeof(CollisionComponent)];

                        if (collisionComponent.spriteBoundingRectangle.Intersects(secondCollisionComponent.spriteBoundingRectangle))
                        {
                            if (!collisionComponent.collisions.Contains(key2))
                            {
                                collisionComponent.collisions.Add(key2);
                            }
                            if (!secondCollisionComponent.collisions.Contains(key))
                            {
                                secondCollisionComponent.collisions.Add(key);
                            }
                        }
                    }
                }
            }
        }

        public void CheckInsideCollision()
        {
            var collisionEntities = ComponentManager.GetEntitiesWithComponent<CollisionComponent>();
            foreach (int key in collisionEntities.Keys)
            {
                var componentList = ComponentManager.GetComponentsWithEntity(key);
                CollisionComponent collisionComponent = (CollisionComponent)componentList[typeof(CollisionComponent)];

                foreach (int key2 in collisionEntities.Keys)
                {
                    if (key != key2){
                        var secondComponentList = ComponentManager.GetComponentsWithEntity(key2);
                        CollisionComponent secondCollisionComponent = (CollisionComponent)secondComponentList[typeof(CollisionComponent)];

                        if(collisionComponent.spriteBoundingRectangle.Contains(secondCollisionComponent.spriteBoundingRectangle))
                        {
                            if (!collisionComponent.collisions.Contains(key2))
                            {
                                collisionComponent.collisions.Add(key2);
                            }
                            if (!secondCollisionComponent.collisions.Contains(key))
                            {
                                secondCollisionComponent.collisions.Add(key);
                            }
                        }
                    }
                }
            }
        }

        public void CheckInsideAndOutsideCollision()
        {
            var collisionEntities = ComponentManager.GetEntitiesWithComponent<CollisionComponent>();
            foreach (int key in collisionEntities.Keys)
            {
                var componentList = ComponentManager.GetComponentsWithEntity(key);
                CollisionComponent collisionComponent = (CollisionComponent)componentList[typeof(CollisionComponent)];

                foreach (int key2 in collisionEntities.Keys)
                {
                    if (key != key2)
                    {
                        var secondComponentList = ComponentManager.GetComponentsWithEntity(key2);
                        CollisionComponent secondCollisionComponent = (CollisionComponent)secondComponentList[typeof(CollisionComponent)];

                        //insert stuff
                        if((collisionComponent.spriteBoundingRectangle.Intersects(secondCollisionComponent.spriteBoundingRectangle) || collisionComponent.spriteBoundingRectangle.Contains(secondCollisionComponent.spriteBoundingRectangle)))
                        {
                            if (!collisionComponent.collisions.Contains(key2))
                            {
                                collisionComponent.collisions.Add(key2);
                            }
                            if (!secondCollisionComponent.collisions.Contains(key))
                            {
                                secondCollisionComponent.collisions.Add(key);
                            }
                        }
                    }
                }
            }
        }


    

        // stops the sprite from going off the screen
        public void Boundering(Vector2D spritePosition, int width, int height)
        {
            spritePosition.X = MathHelper.Clamp((float) spritePosition.X, (0 + (width/2)), (900 -(width/2)));
            spritePosition.Y = MathHelper.Clamp((float) spritePosition.Y, (0 + (height/2)), (500-(height/2)));
        }
    }
}
