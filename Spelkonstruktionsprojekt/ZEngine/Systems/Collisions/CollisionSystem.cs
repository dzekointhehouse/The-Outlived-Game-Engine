using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZEngine.Managers;
using ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Components;
using ZEngine.Wrappers;
using Spelkonstruktionsprojekt.ZEngine.Wrappers;
using System.Collections;
using System.Diagnostics;
using System.Runtime.Remoting;
using Spelkonstruktionsprojekt.ZEngine.Components.RenderComponent;

namespace ZEngine.Systems
{
    class CollisionSystem : ISystem
    {
        private readonly ComponentManager ComponentManager = ComponentManager.Instance;

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

                    if (stillEntityCollisionComponent.IsCage)
                    {
                        if (IsCagedBy(movingEntityId, stillEntityId))
                        {
                            var contains = EntityContains(stillEntityId, movingEntityId);
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
        public bool EntityContains(int cageId, int movingEntity)
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

        public Rectangle GetSpriteBoundingRectangle(RenderComponent renderComponent, Rectangle spriteBoundingBox, Vector2 offset = default(Vector2))
        {
            var width = spriteBoundingBox.Width > 0 ? spriteBoundingBox.Width : renderComponent.DimensionsComponent.Width;
            var height = spriteBoundingBox.Height > 0 ? spriteBoundingBox.Height : renderComponent.DimensionsComponent.Height;
            var x = renderComponent.PositionComponent.Position.X + spriteBoundingBox.X + offset.X - width / 2;
            var y = renderComponent.PositionComponent.Position.Y + spriteBoundingBox.Y + offset.Y - height / 2;
            Debug.WriteLine("In collisions RENDER DATA is W:" + width + ", H:" + height + ", X:" + x + ", Y:" + y);
            return new Rectangle((int)x, (int)y, (int)width, (int)height);
        }

        public BoundingSphere GetSpriteBoundingSphere(RenderComponent renderComponent, Rectangle spriteBoundingBox, Vector2 offset = default(Vector2))
        {
            const double tightBoundFactor = 1; //makes for a tighter fit.
            //Assumes sprite bounding box width represents a radius
            var radius = spriteBoundingBox.Width > 0 ? spriteBoundingBox.Width : renderComponent.Radius;
            var x = renderComponent.PositionComponent.Position.X + spriteBoundingBox.X + offset.X - radius;
            var y = renderComponent.PositionComponent.Position.Y + spriteBoundingBox.Y + offset.Y - radius;

            return new BoundingSphere(new Vector3((float)x, (float)y, 0), (float)(radius / 2 * tightBoundFactor));
        }

        public Rectangle GetBoundingRectangleFromSphere(RenderComponent renderComponent, Rectangle spriteBoundingBox, Vector2 offset = default(Vector2))
        {
            var width = spriteBoundingBox.Width > 0 ? spriteBoundingBox.Width : renderComponent.Radius;
            var height = spriteBoundingBox.Height > 0 ? spriteBoundingBox.Height : renderComponent.Radius;
            var x = renderComponent.PositionComponent.Position.X + spriteBoundingBox.X + offset.X - width / 2;
            var y = renderComponent.PositionComponent.Position.Y + spriteBoundingBox.Y + offset.Y - height / 2;

            return new Rectangle((int)x, (int)y, (int)width, (int)height);
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
