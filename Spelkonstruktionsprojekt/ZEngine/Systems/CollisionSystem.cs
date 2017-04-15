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
using ZEngine.Components.MoveComponent;

namespace Spelkonstruktionsprojekt.ZEngine.Systems
{
    class CollisionSystem : ISystem
    {
        private readonly ComponentManager ComponentManager = ComponentManager.Instance;

        public void Collisions()
        {
            var collisionEntities = ComponentManager.GetEntitiesWithComponent<CollisionComponent>();
            foreach (var movingEntity in collisionEntities)
            {
                foreach (var collidableObject in collisionEntities)
                {
                    if (movingEntity.Key == collidableObject.Key) continue;
                    if (
                        ComponentManager.EntityHasComponent<RenderComponent>(collidableObject.Key) &&
                        ComponentManager.EntityHasComponent<RenderComponent>(movingEntity.Key) &&
                        ComponentManager.EntityHasComponent<MoveComponent>(movingEntity.Key))
                    {
                        if (WillCollide(movingEntity.Key, collidableObject.Key))
                        {
                            System.Diagnostics.Debug.WriteLine("COLLIDED");
                            var collisionComponent = movingEntity.Value;
                            var secondCollisionComponent = collidableObject.Value;
                            if (!collisionComponent.collisions.Contains(collidableObject.Key))
                            {
                                collisionComponent.collisions.Add(collidableObject.Key);
                            }
                            if (!secondCollisionComponent.collisions.Contains(movingEntity.Key))
                            {
                                secondCollisionComponent.collisions.Add(movingEntity.Key);
                            }
                        }
                    }

                }
            }
        }

        public bool WillCollide(int movingEntity, int objectEntity)
        {
            var movingRenderable = ComponentManager.GetEntityComponent<RenderComponent>(movingEntity);
            var movingMovable = ComponentManager.GetEntityComponent<MoveComponent>(movingEntity);
            var movingPosition = (Vector2)movingRenderable.PositionComponent.Position;
            var movingVelocity = (Vector2)movingMovable.Velocity;
            float movingDirection = (float) movingMovable.Direction;
            var newPosition = movingPosition + movingVelocity;

            var objectRenderable = ComponentManager.GetEntityComponent<RenderComponent>(objectEntity);
            var objectPosition = objectRenderable.PositionComponent.Position;
            var objectWidth = objectRenderable.DimensionsComponent.Width;
            var objectHeight = objectRenderable.DimensionsComponent.Height;
            var ray = new Ray(new Vector3(movingPosition, 0), new Vector3(movingDirection));
            var objectBox = new BoundingBox(
                new Vector3((float) objectPosition.X, (float) objectPosition.Y, 0),
                new Vector3((float) (objectPosition.X + objectWidth), (float) (objectPosition.Y + objectHeight), 0));
            float? distance = ray.Intersects(objectBox);
            System.Diagnostics.Debug.WriteLine("\nDistance " + distance + ", actual distance " + Vector2.Distance(newPosition, objectPosition));
            System.Diagnostics.Debug.WriteLine("ObjectPosition " + objectPosition + ", MovingPosition " + newPosition);
            if (distance != null)
            {
                if (distance < Vector2.Distance(newPosition, objectPosition))
                {
                    System.Diagnostics.Debug.WriteLine("COLLISION TRUE");

                    return true;
                }
            }
            return false;
        }
        
        public void DetectCollisions()
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
                                System.Diagnostics.Debug.WriteLine("IS NOT!!!!!! NOT CLAMPED OR WHATEVER");
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
