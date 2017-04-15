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
using System.Diagnostics;
using System.Runtime.Remoting;
using Spelkonstruktionsprojekt.ZEngine.Components.RenderComponent;
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
            float movingDirection = (float)movingMovable.Direction;
            var newPosition = movingPosition + movingVelocity;

            var objectRenderable = ComponentManager.GetEntityComponent<RenderComponent>(objectEntity);
            var objectPosition = objectRenderable.PositionComponent.Position;
            var objectWidth = RenderComponentHelper.GetDimensions(objectRenderable).Width;
            var objectHeight = RenderComponentHelper.GetDimensions(objectRenderable).Height;

            var direction = Vector2.Normalize(objectPosition - movingPosition);
            //Creates a ray heading out from the movingObjects facing direction
            var ray = new Ray(new Vector3(movingPosition, 0), new Vector3(direction, 0));
            Debug.WriteLine("\n Ray position" + ray.Position + ", direction" + ray.Direction);

            //Creating boundingbox for which to see if the ray has intersected
            var objectBox = new BoundingBox(
                new Vector3((float)objectPosition.X, (float)objectPosition.Y, 0),
                new Vector3((float)(objectPosition.X + objectWidth), (float)(objectPosition.Y + objectHeight), 0));

            var objectBox2 = new BoundingSphere(
                new Vector3((float)(objectPosition.X + objectWidth / 2), (float)(objectPosition.Y + objectHeight / 2), 0),
                objectWidth);

            float? distance = ray.Intersects(objectBox);

            System.Diagnostics.Debug.WriteLine("Distance " + distance + ", actual distance " + Vector2.Distance(newPosition, objectPosition));
            System.Diagnostics.Debug.WriteLine("ObjectPosition " + objectPosition + ", MovingPosition " + newPosition);
            //Distance will be a number if the ray hits the Objects BoundingBox
            if (distance != null)
            {
                //The distance of the new position from the objectPosition will be greater than the ray distance to the object
                // only if there will be a collision on the next frame
                if (distance < (Vector2.Distance(newPosition, objectPosition) + RenderComponentHelper.GetDimensions(movingRenderable).Width))
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

            var movingEntities = collisionEntities.Where(entity => ComponentManager.EntityHasComponent<MoveComponent>(entity.Key));
            foreach (var movingEntity in movingEntities)
            {
                var movingEntityId = movingEntity.Key;
                CollisionComponent movingEntityCollisionComponent = movingEntity.Value;

                foreach (var stillEntity in collisionEntities)
                {
                    var stillEntityId = stillEntity.Key;
                    if (movingEntityId == stillEntityId) continue;

                    CollisionComponent stillEntityCollisionComponent = stillEntity.Value;

                    if (stillEntityCollisionComponent.CageMode)
                    {
                        if (IsCagedBy(movingEntityId, stillEntityId))
                        {
                            var contains = Contains(stillEntityId, movingEntityId);
                            if (!contains)
                            {
                                System.Diagnostics.Debug.WriteLine("COLLISION!!!!!! CLAMPED OR WHATEVER");

                                if (!movingEntityCollisionComponent.collisions.Contains(stillEntityId))
                                {
                                    movingEntityCollisionComponent.collisions.Add(stillEntityId);
                                }
                                if (!stillEntityCollisionComponent.collisions.Contains(movingEntityId))
                                {
                                    stillEntityCollisionComponent.collisions.Add(movingEntityId);
                                }
                            }
                        }
                    }
                    else if (EntitiesIntersects(movingEntityId, stillEntityId))
                    {
                        if (!movingEntityCollisionComponent.collisions.Contains(stillEntityId))
                        {
                            movingEntityCollisionComponent.collisions.Add(stillEntityId);
                        }
                        if (!stillEntityCollisionComponent.collisions.Contains(movingEntityId))
                        {
                            stillEntityCollisionComponent.collisions.Add(movingEntityId);
                        }
                    }
                }
            }
        }

        public bool EntitiesIntersects(int movingEntity, int stillEntity)
        {
            var movingRenderComponent = ComponentManager.GetEntityComponent<RenderComponent>(movingEntity);
            var movingCollisionBox = ComponentManager.GetEntityComponent<CollisionComponent>(stillEntity).spriteBoundingRectangle;
            var stillRenderComponent = ComponentManager.GetEntityComponent<RenderComponent>(stillEntity);
            var stillCollisionBox = ComponentManager.GetEntityComponent<CollisionComponent>(stillEntity).spriteBoundingRectangle;

            var movingEntityOffset = ComponentManager.EntityHasComponent<RenderOffsetComponent>(movingEntity)
                ? ComponentManager.GetEntityComponent<RenderOffsetComponent>(movingEntity).Offset
                : default(Vector2);
            var stillEntityOffset = ComponentManager.EntityHasComponent<RenderOffsetComponent>(stillEntity)
                ? ComponentManager.GetEntityComponent<RenderOffsetComponent>(stillEntity).Offset
                : default(Vector2);

            if (movingRenderComponent.Radius > 0)
            {
                if (stillRenderComponent.Radius > 0)
                {
                    //Both object are SPHERES
                    return
                        GetSpriteBoundingSphere(movingRenderComponent, movingCollisionBox, movingEntityOffset)
                            .Intersects(GetSpriteBoundingSphere(stillRenderComponent, stillCollisionBox, stillEntityOffset));
                }
                //Only ONE is a SPHERE
                return
                    GetBoundingRectangleFromSphere(movingRenderComponent, movingCollisionBox, movingEntityOffset)
                        .Intersects(GetSpriteBoundingRectangle(stillRenderComponent, stillCollisionBox, stillEntityOffset));
            }
            //Both objects are RECTANGLES
            return
                GetSpriteBoundingRectangle(movingRenderComponent, movingCollisionBox, movingEntityOffset)
                    .Intersects(GetSpriteBoundingRectangle(stillRenderComponent, stillCollisionBox, stillEntityOffset));
        }

        public Rectangle GetSpriteBoundingRectangle(RenderComponent renderComponent, Rectangle spriteBoundingBox, Vector2 offset = default(Vector2))
        {
            var x = renderComponent.PositionComponent.Position.X + spriteBoundingBox.X + offset.X;
            var y = renderComponent.PositionComponent.Position.Y + spriteBoundingBox.Y + offset.Y;
            var width = spriteBoundingBox.Width > 0 ? spriteBoundingBox.Width : renderComponent.DimensionsComponent.Width;
            var height = spriteBoundingBox.Height > 0 ? spriteBoundingBox.Height : renderComponent.DimensionsComponent.Height;
            Debug.WriteLine("In collisions RENDER DATA is W:" + width + ", H:" + height + ", X:" + x + ", Y:" + y);
            return new Rectangle((int)x, (int)y, (int)width, (int)height);
        }

        public BoundingSphere GetSpriteBoundingSphere(RenderComponent renderComponent, Rectangle spriteBoundingBox, Vector2 offset = default(Vector2))
        {
            const double tightBoundFactor = 0.8; //makes for a tighter fit.

            var x = renderComponent.PositionComponent.Position.X + spriteBoundingBox.X + offset.X;
            var y = renderComponent.PositionComponent.Position.Y + spriteBoundingBox.Y + offset.Y;
            var radius = spriteBoundingBox.Width > 0 ? spriteBoundingBox.Width : renderComponent.Radius;

            return new BoundingSphere(new Vector3((float)x, (float)y, 0), (float)(radius / 2 * tightBoundFactor));
        }

        public Rectangle GetBoundingRectangleFromSphere(RenderComponent renderComponent, Rectangle spriteBoundingBox, Vector2 offset = default(Vector2))
        {
            var x = renderComponent.PositionComponent.Position.X + spriteBoundingBox.X + offset.X;
            var y = renderComponent.PositionComponent.Position.Y + spriteBoundingBox.Y + offset.Y;
            var width = spriteBoundingBox.Width > 0 ? spriteBoundingBox.Width : renderComponent.Radius;
            var height = spriteBoundingBox.Height > 0 ? spriteBoundingBox.Height : renderComponent.Radius;

            return new Rectangle((int)x, (int)y, (int)width, (int)height);
        }

        public bool Contains(int cageId, int movingEntity)
        {
            var cageRenderComponent = ComponentManager.GetEntityComponent<RenderComponent>(cageId);
            var cageCollisionBox = ComponentManager.GetEntityComponent<CollisionComponent>(cageId).spriteBoundingRectangle;

            var movingEntityComponent = ComponentManager.GetEntityComponent<RenderComponent>(movingEntity);
            var movingCollisionBox = ComponentManager.GetEntityComponent<CollisionComponent>(movingEntity).spriteBoundingRectangle;

            var movingEntityOffset = ComponentManager.EntityHasComponent<RenderOffsetComponent>(movingEntity)
                ? ComponentManager.GetEntityComponent<RenderOffsetComponent>(movingEntity).Offset
                : default(Vector2);
            var cageEntityOffset = ComponentManager.EntityHasComponent<RenderOffsetComponent>(cageId)
                ? ComponentManager.GetEntityComponent<RenderOffsetComponent>(cageId).Offset
                : default(Vector2);

            if (movingEntityComponent.Radius > 0)
            {
                if (cageRenderComponent.Radius > 0)
                {
                    //Both object are SPHERES
                    return
                        GetSpriteBoundingSphere(cageRenderComponent, cageCollisionBox, cageEntityOffset)
                            .Contains(GetSpriteBoundingSphere(movingEntityComponent, movingCollisionBox, movingEntityOffset)) == ContainmentType.Contains;
                }
                //Only ONE is a SPHERE
                return GetSpriteBoundingRectangle(cageRenderComponent, cageCollisionBox, cageEntityOffset)
                            .Contains(GetBoundingRectangleFromSphere(movingEntityComponent, movingCollisionBox, movingEntityOffset));
            }
            //Both objects are RECTANGLES
            return
                GetSpriteBoundingRectangle(cageRenderComponent, cageCollisionBox, cageEntityOffset)
                    .Contains(GetSpriteBoundingRectangle(movingEntityComponent, movingCollisionBox, movingEntityOffset));
        }

        public bool IsCagedBy(int entityId, int potentialCageId)
        {
            if (!ComponentManager.EntityHasComponent<CageComponent>(entityId)) return false;

            var cageComponent = ComponentManager.GetEntityComponent<CageComponent>(entityId);
            return cageComponent.CageId == potentialCageId;
        }

        // stops the sprite from going off the screen
        public void Boundering(Vector2D spritePosition, int width, int height)
        {
            spritePosition.X = MathHelper.Clamp((float)spritePosition.X, (0 + (width / 2)), (900 - (width / 2)));
            spritePosition.Y = MathHelper.Clamp((float)spritePosition.Y, (0 + (height / 2)), (500 - (height / 2)));
        }
    }
}
