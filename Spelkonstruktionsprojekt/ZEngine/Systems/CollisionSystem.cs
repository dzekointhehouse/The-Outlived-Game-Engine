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
            foreach (var firstEntity in collisionEntities)
            {
                var firstEntityKey = firstEntity.Key;
                CollisionComponent collisionComponent = firstEntity.Value;

                foreach (var secondEntity in collisionEntities)
                {
                    var secondEntityKey = secondEntity.Key;
                    if (firstEntityKey != secondEntityKey)
                    {
                        CollisionComponent secondCollisionComponent = secondEntity.Value;

                        //insert stuff
                        var renderComponent = ComponentManager.GetEntityComponent<RenderComponent>(firstEntityKey);
                        var secondRenderComponent = ComponentManager.GetEntityComponent<RenderComponent>(secondEntityKey);
                        var boundingBox = GetSpriteBoundingRectangle(renderComponent, collisionComponent.spriteBoundingRectangle);
                        var secondBoundingBox = GetSpriteBoundingRectangle(secondRenderComponent, secondCollisionComponent.spriteBoundingRectangle);
                        if (collisionComponent.CageMode)
                        {
                            if (IsCagedBy(secondEntityKey, firstEntityKey))
                            {
                                System.Diagnostics.Debug.WriteLine("IS CAGED!!!!!! CLAMPED OR WHATEVER");
                                System.Diagnostics.Debug.WriteLine(boundingBox.X + ", " + boundingBox.Y + ", " + boundingBox.Width + ", " + boundingBox.Height);
                                if (!boundingBox.Contains(secondBoundingBox))
                                {
                                    System.Diagnostics.Debug.WriteLine("COLLISION!!!!!! CLAMPED OR WHATEVER");

                                    if (!collisionComponent.collisions.Contains(secondEntityKey))
                                    {
                                        collisionComponent.collisions.Add(secondEntityKey);
                                    }
                                    if (!secondCollisionComponent.collisions.Contains(firstEntityKey))
                                    {
                                        secondCollisionComponent.collisions.Add(firstEntityKey);
                                    }
                                }
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine("IS NOT!!!!!! CLAMPED OR WHATEVER");
                            }
                        }
                        else
                        {
                            if (boundingBox.Intersects(secondCollisionComponent.spriteBoundingRectangle))
                            {
                                if (!collisionComponent.collisions.Contains(secondEntityKey))
                                {
                                    collisionComponent.collisions.Add(secondEntityKey);
                                }
                                if (!secondCollisionComponent.collisions.Contains(firstEntityKey))
                                {
                                    secondCollisionComponent.collisions.Add(firstEntityKey);
                                }
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

        public Rectangle GetSpriteBoundingRectangle(RenderComponent renderComponent, Rectangle spriteBoundingBox)
        {
            var x = renderComponent.PositionComponent.Position.X;
            var y = renderComponent.PositionComponent.Position.Y;
            var width = renderComponent.DimensionsComponent.Width;
            var height = renderComponent.DimensionsComponent.Height;
            return new Rectangle((int) (x + spriteBoundingBox.X), (int) (y + spriteBoundingBox.Y), (int) (width + spriteBoundingBox.Width), (int) (height + spriteBoundingBox.Height));
        }

        public bool IsCagedBy(int entityId, int potentialCageId)
        {
            if (ComponentManager.EntityHasComponent<CageComponent>(entityId))
            {
                var cageComponent = ComponentManager.GetEntityComponent<CageComponent>(entityId);
                return cageComponent.CageId == potentialCageId;
            }
            return false;
        }
    }
}
