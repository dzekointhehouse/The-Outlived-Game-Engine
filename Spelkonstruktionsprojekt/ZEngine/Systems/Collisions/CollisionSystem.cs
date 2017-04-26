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
            var collisionEntities = ComponentManager.GetEntitiesWithComponent(typeof(CollisionComponent));

            var movingEntities = collisionEntities.Where(entity => ComponentManager.EntityHasComponent<MoveComponent>(entity.Key));
            foreach (var movingEntity in movingEntities)
            {
                var movingEntityId = movingEntity.Key;
                var movingEntityCollisionComponent = movingEntity.Value as CollisionComponent;

                foreach (var stillEntity in collisionEntities)
                {
                    var stillEntityId = stillEntity.Key;
                    if (movingEntityId == stillEntityId) continue;

                    var stillEntityCollisionComponent = stillEntity.Value as CollisionComponent;

                    if (stillEntityCollisionComponent.IsCage)
                    {
                        if (IsCagedBy(movingEntityId, stillEntityId))
                        {
                            var contains = EntityContains(stillEntityId, movingEntityId);
                            if (!contains)
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
            var movingRenderComponent = ComponentManager.GetEntityComponentOrDefault<RenderComponent>(movingEntity);
            var movingPositionComponent = ComponentManager.GetEntityComponentOrDefault<PositionComponent>(movingEntity);



            var movingCollisionBox = ComponentManager.GetEntityComponentOrDefault<CollisionComponent>(stillEntity).spriteBoundingRectangle;
            var stillRenderComponent = ComponentManager.GetEntityComponentOrDefault<RenderComponent>(stillEntity);
            var stillPositionComponent = ComponentManager.GetEntityComponentOrDefault<PositionComponent>(stillEntity);
            var stillCollisionBox = ComponentManager.GetEntityComponentOrDefault<CollisionComponent>(stillEntity).spriteBoundingRectangle;

            var movingEntityOffset = ComponentManager.EntityHasComponent<RenderOffsetComponent>(movingEntity)
                ? ComponentManager.GetEntityComponentOrDefault<RenderOffsetComponent>(movingEntity).Offset
                : default(Vector2);
            var stillEntityOffset = ComponentManager.EntityHasComponent<RenderOffsetComponent>(stillEntity)
                ? ComponentManager.GetEntityComponentOrDefault<RenderOffsetComponent>(stillEntity).Offset
                : default(Vector2);

            if (movingRenderComponent.Radius > 0)
            {
                if (stillRenderComponent.Radius > 0)
                {
                    //Both object are SPHERES
                    return
                        GetSpriteBoundingSphere(movingRenderComponent, movingPositionComponent, movingCollisionBox, movingEntityOffset)
                            .Intersects(GetSpriteBoundingSphere(stillRenderComponent, stillPositionComponent, stillCollisionBox, stillEntityOffset));
                }
                //Only ONE is a SPHERE
                return
                    GetBoundingRectangleFromSphere(movingRenderComponent, movingPositionComponent, movingCollisionBox, movingEntityOffset)
                        .Intersects(GetSpriteBoundingRectangle(stillRenderComponent, stillPositionComponent, stillCollisionBox, stillEntityOffset));
            }
            //Both objects are RECTANGLES
            return
                GetSpriteBoundingRectangle(movingRenderComponent, movingPositionComponent, movingCollisionBox, movingEntityOffset)
                    .Intersects(GetSpriteBoundingRectangle(stillRenderComponent, stillPositionComponent, stillCollisionBox, stillEntityOffset));
        }
        public bool EntityContains(int cageId, int movingEntity)
        {
            var cageRenderComponent = ComponentManager.GetEntityComponentOrDefault<RenderComponent>(cageId);
            var cagePositionComponent = ComponentManager.GetEntityComponentOrDefault<PositionComponent>(cageId);
            var cageCollisionBox = ComponentManager.GetEntityComponentOrDefault<CollisionComponent>(cageId).spriteBoundingRectangle;


            var movingEntityComponent = ComponentManager.GetEntityComponentOrDefault<RenderComponent>(movingEntity);
            var movingEntityPositionComponent = ComponentManager.GetEntityComponentOrDefault<PositionComponent>(movingEntity);
            var movingCollisionBox = ComponentManager.GetEntityComponentOrDefault<CollisionComponent>(movingEntity).spriteBoundingRectangle;

            var movingEntityOffset = ComponentManager.EntityHasComponent<RenderOffsetComponent>(movingEntity)
                ? ComponentManager.GetEntityComponentOrDefault<RenderOffsetComponent>(movingEntity).Offset
                : default(Vector2);
            var cageEntityOffset = ComponentManager.EntityHasComponent<RenderOffsetComponent>(cageId)
                ? ComponentManager.GetEntityComponentOrDefault<RenderOffsetComponent>(cageId).Offset
                : default(Vector2);

            if (movingEntityComponent.Radius > 0)
            {
                if (cageRenderComponent.Radius > 0)
                {
                    //Both object are SPHERES
                    return
                        GetSpriteBoundingSphere(cageRenderComponent, cagePositionComponent, cageCollisionBox, cageEntityOffset)
                            .Contains(GetSpriteBoundingSphere(movingEntityComponent, movingEntityPositionComponent, movingCollisionBox, movingEntityOffset)) == ContainmentType.Contains;
                }
                //Only ONE is a SPHERE
                return GetSpriteBoundingRectangle(cageRenderComponent, cagePositionComponent, cageCollisionBox, cageEntityOffset)
                            .Contains(GetBoundingRectangleFromSphere(movingEntityComponent, movingEntityPositionComponent, movingCollisionBox, movingEntityOffset));
            }
            //Both objects are RECTANGLES
            return
                GetSpriteBoundingRectangle(cageRenderComponent, cagePositionComponent, cageCollisionBox, cageEntityOffset)
                    .Contains(GetSpriteBoundingRectangle(movingEntityComponent, movingEntityPositionComponent, movingCollisionBox, movingEntityOffset));
        }

        public Rectangle GetSpriteBoundingRectangle(RenderComponent renderComponent, PositionComponent positionComponent, Rectangle spriteBoundingBox, Vector2 offset = default(Vector2))
        {
            var width = spriteBoundingBox.Width > 0 ? spriteBoundingBox.Width : renderComponent.DimensionsComponent.Width;
            var height = spriteBoundingBox.Height > 0 ? spriteBoundingBox.Height : renderComponent.DimensionsComponent.Height;
            var x = positionComponent.Position.X + spriteBoundingBox.X + offset.X - width / 2;
            var y = positionComponent.Position.Y + spriteBoundingBox.Y + offset.Y - height / 2;
            //Debug.WriteLine("In collisions RENDER DATA is W:" + width + ", H:" + height + ", X:" + x + ", Y:" + y);
            return new Rectangle((int)x, (int)y, (int)width, (int)height);
        }

        public BoundingSphere GetSpriteBoundingSphere(RenderComponent renderComponent, PositionComponent positionComponent, Rectangle spriteBoundingBox, Vector2 offset = default(Vector2))
        {
            const double tightBoundFactor = 1; //makes for a tighter fit.
            //Assumes sprite bounding box width represents a radius
            var radius = spriteBoundingBox.Width > 0 ? spriteBoundingBox.Width : renderComponent.Radius;
            var x = positionComponent.Position.X + spriteBoundingBox.X + offset.X - radius;
            var y = positionComponent.Position.Y + spriteBoundingBox.Y + offset.Y - radius;

            return new BoundingSphere(new Vector3((float)x, (float)y, 0), (float)(radius / 2 * tightBoundFactor));
        }

        public Rectangle GetBoundingRectangleFromSphere(RenderComponent renderComponent, PositionComponent positionComponent, Rectangle spriteBoundingBox, Vector2 offset = default(Vector2))
        {
            var width = spriteBoundingBox.Width > 0 ? spriteBoundingBox.Width : renderComponent.Radius;
            var height = spriteBoundingBox.Height > 0 ? spriteBoundingBox.Height : renderComponent.Radius;
            var x = positionComponent.Position.X + spriteBoundingBox.X + offset.X - width / 2;
            var y = positionComponent.Position.Y + spriteBoundingBox.Y + offset.Y - height / 2;

            return new Rectangle((int)x, (int)y, (int)width, (int)height);
        }

        public bool IsCagedBy(int entityId, int potentialCageId)
        {
            if (!ComponentManager.EntityHasComponent<CageComponent>(entityId))
                return false;

            var cageComponent = ComponentManager.GetEntityComponentOrDefault<CageComponent>(entityId);
            return cageComponent.CageId == potentialCageId;
        }

        // stops the sprite from going off the screen
        public void Boundering(Vector2 spritePosition, int width, int height)
        {
            spritePosition.X = MathHelper.Clamp((float)spritePosition.X, (0 + (width / 2)), (900 - (width / 2)));
            spritePosition.Y = MathHelper.Clamp((float)spritePosition.Y, (0 + (height / 2)), (500 - (height / 2)));
        }
    }
}
