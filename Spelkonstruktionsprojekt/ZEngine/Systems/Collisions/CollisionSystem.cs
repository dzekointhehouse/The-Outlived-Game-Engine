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
using System.Runtime.Remoting;
using Spelkonstruktionsprojekt.ZEngine.Components.RenderComponent;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using UnityEngine;
using Color = Microsoft.Xna.Framework.Color;
using Debug = System.Diagnostics.Debug;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Vector3 = Microsoft.Xna.Framework.Vector3;
using ShapeRendering2D;
using Spelkonstruktionsprojekt.ZEngine.Helpers;

namespace ZEngine.Systems
{
    class CollisionSystem : ISystem
    {
        private readonly ComponentManager ComponentManager = ComponentManager.Instance;

        public void DetectCollisions()
        {
            var collisionEntities = ComponentManager.GetEntitiesWithComponent(typeof(CollisionComponent));

            var movingEntities = collisionEntities.Where(
                entity => ComponentManager.EntityHasComponent<MoveComponent>(entity.Key));
            foreach (var movingEntity in movingEntities)
            {
                var movingEntityId = movingEntity.Key;
                var movingEntityCollisionComponent = movingEntity.Value as CollisionComponent;

                foreach (var stillEntity in collisionEntities)
                {
                    var stillEntityId = stillEntity.Key;
                    if (movingEntityId == stillEntityId) continue;

                    var stillEntityCollisionComponent = stillEntity.Value as CollisionComponent;

//                    if (stillEntityCollisionComponent.IsCage)
//                    {
//                        var cageComponent = ComponentManager.GetEntityComponentOrDefault<CageComponent>(movingEntityId);
//                        var shouldBeCaged = cageComponent != null && cageComponent.CageId == stillEntityId;
//                        var isCaged = EntitiesCollide(stillEntityId, movingEntityId) == ContainmentType.Contains;
//                        if (shouldBeCaged && isCaged)
//                        {
//                            if (!movingEntityCollisionComponent.collisions.Contains(stillEntityId))
//                            {
//                                movingEntityCollisionComponent.collisions.Add(stillEntityId);
//                            }
//                            if (!stillEntityCollisionComponent.collisions.Contains(movingEntityId))
//                            {
//                                stillEntityCollisionComponent.collisions.Add(movingEntityId);
//                            }
//                        }
//                    }
                    if (EntitiesCollide(movingEntityId, stillEntityId))
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

        private bool EntitiesCollide(int movingEntity, int stillEntity)
        {
            var movingRenderComponent = ComponentManager.GetEntityComponentOrDefault<RenderComponent>(movingEntity);
            if (movingRenderComponent == null) return false;
            var movingPositionComponent = ComponentManager.GetEntityComponentOrDefault<PositionComponent>(movingEntity);
            if (movingPositionComponent == null) return false;

            var stillRenderComponent = ComponentManager.GetEntityComponentOrDefault<RenderComponent>(stillEntity);
            if (stillRenderComponent == null) return false;
            var stillPositionComponent = ComponentManager.GetEntityComponentOrDefault<PositionComponent>(stillEntity);
            if (stillPositionComponent == null) return false;

            //Roughly check distance
            var aproxDistance = Math.Pow(stillPositionComponent.Position.X - movingPositionComponent.Position.X, 2) +
                Math.Pow(stillPositionComponent.Position.Y - movingPositionComponent.Position.Y, 2);
            if (aproxDistance > Math.Pow(movingRenderComponent.DimensionsComponent.Width, 2)) return false;

            var movingMoveComponent = ComponentManager.GetEntityComponentOrDefault<MoveComponent>(movingEntity);
            if (movingMoveComponent == null) return false;
            var movingCollisionBox = ComponentManager.GetEntityComponentOrDefault<CollisionComponent>(movingEntity);
            var movingEntityOffset = ComponentManager.GetEntityComponentOrDefault<RenderOffsetComponent>(movingEntity);
            var movingOffset = Vector2.Zero;
//            movingEntityOffset != null
//                ? new Vector2(movingEntityOffset.Offset.X, movingEntityOffset.Offset.Y)
//                : Vector2.Zero;
            var movingSpriteComponent = ComponentManager.GetEntityComponentOrDefault<SpriteComponent>(movingEntity);
            if (movingSpriteComponent == null) return false;
            var colorA =
                new Color[movingSpriteComponent.TileWidth * movingSpriteComponent.TileHeight];
            var position = movingSpriteComponent.Position;
            var x = position.X;
            var y = position.Y;
            movingSpriteComponent.Sprite.GetData(0,
                new Rectangle(x, y, movingSpriteComponent.TileWidth, movingSpriteComponent.TileHeight), colorA, 0,
                colorA.Length);
            var matrixA =
                Matrix.CreateTranslation(new Vector3(
                    (float) (-movingSpriteComponent.TileWidth * 0.5),
                    (float) (-movingSpriteComponent.TileHeight * 0.5), 0)) *
                Matrix.CreateScale(
                    (float) movingRenderComponent.DimensionsComponent.Width / (float) movingSpriteComponent.TileWidth,
                    (float) movingRenderComponent.DimensionsComponent.Height / (float) movingSpriteComponent.TileHeight,
                    1) *
                Matrix.CreateRotationZ(movingMoveComponent.Direction) *
                Matrix.CreateTranslation(movingPositionComponent.Position.X, movingPositionComponent.Position.Y, 0f);

            var stillMoveComponent = ComponentManager.GetEntityComponentOrDefault<MoveComponent>(stillEntity);
//            if (stillMoveComponent == null) return false;
            var stillCollisionBox = ComponentManager.GetEntityComponentOrDefault<CollisionComponent>(stillEntity);
            var stillEntityAngle = 0;
            var stillEntityOffset = ComponentManager.GetEntityComponentOrDefault<RenderOffsetComponent>(stillEntity);
            var stillOffset = Vector2.Zero;
//            stillEntityOffset != null
//                ? new Vector2(stillEntityOffset.Offset.X, stillEntityOffset.Offset.Y)
//                : Vector2.Zero;
            var stillSpriteComponent = ComponentManager.GetEntityComponentOrDefault<SpriteComponent>(stillEntity);
            if (stillSpriteComponent == null) return false;
            var colorB =
                new Color[stillSpriteComponent.TileWidth * stillSpriteComponent.TileHeight];
            var positionB = stillSpriteComponent.Position;
            var xB = positionB.X;
            var yB = positionB.Y;
            stillSpriteComponent.Sprite.GetData(0,
                new Rectangle(xB, yB, stillSpriteComponent.TileWidth, stillSpriteComponent.TileHeight), colorB, 0,
                colorB.Length);
            var stillScale = (float) stillRenderComponent.DimensionsComponent.Width /
                             (float) stillSpriteComponent.TileWidth;
            var matrixB =
                Matrix.CreateTranslation(new Vector3(
                    (float) (-stillSpriteComponent.TileWidth * 0.5),
                    (float) (-stillSpriteComponent.TileHeight * 0.5), 0)) *
                Matrix.CreateScale(
                    (float) stillRenderComponent.DimensionsComponent.Width / (float) stillSpriteComponent.TileWidth,
                    (float) stillRenderComponent.DimensionsComponent.Height / (float) stillSpriteComponent.TileHeight,
                    1) *
                Matrix.CreateRotationZ(stillEntityAngle) *
                Matrix.CreateTranslation(stillPositionComponent.Position.X, stillPositionComponent.Position.Y, 0f);


            var movingEntityCollisionBounds =
                CalculateCollisionBounds(
                    new Rectangle(
                        (int) 0,
                        (int) 0,
                        movingSpriteComponent.TileWidth,
                        movingSpriteComponent.TileHeight
                    ),
                    0,
                    movingCollisionBox,
                    movingOffset,
                    matrixA
                );

            var stillEntityCollisionBounds =
                CalculateCollisionBounds(
                    new Rectangle(
                        (int) 0,
                        (int) 0,
                        stillSpriteComponent.TileWidth,
                        stillSpriteComponent.TileHeight
                    ),
                    0,
                    stillCollisionBox,
                    stillOffset,
                    matrixB
                );

//            var shapeRenderer = CollisionRendering.renderer;
            if (movingEntityCollisionBounds.Intersects(stillEntityCollisionBounds))
            {
                if (IntersectPixs(matrixA,
                    new Rectangle(0, 0, movingSpriteComponent.TileWidth, movingSpriteComponent.TileHeight), colorA,
                    matrixB, new Rectangle(0, 0, stillSpriteComponent.TileWidth, stillSpriteComponent.TileHeight),
                    colorB))
                {
//                    shapeRenderer.AddBoundingRectangle(movingEntityCollisionBounds, Color.Red, 0.02f);
//                    shapeRenderer.AddBoundingRectangle(stillEntityCollisionBounds, Color.DarkRed, 0.02f);
                    return true;
                }
//                shapeRenderer.AddBoundingRectangle(movingEntityCollisionBounds, Color.Blue, 0.02f);
//                shapeRenderer.AddBoundingRectangle(stillEntityCollisionBounds, Color.DarkBlue, 0.02f);
                return false;
            }
//            shapeRenderer.AddBoundingRectangle(movingEntityCollisionBounds, Color.Green, 0.02f);
//            shapeRenderer.AddBoundingRectangle(stillEntityCollisionBounds, Color.DarkGreen, 0.02f);
            return false;
        }

        public Rectangle CalculateCollisionBounds(Rectangle spriteBounds, float angle,
            CollisionComponent collisionComponent, Vector2 offset, Matrix transformation)
        {
            Vector2 leftTop = new Vector2(spriteBounds.Left, spriteBounds.Top);
            Vector2 rightTop = new Vector2(spriteBounds.Right, spriteBounds.Top);
            Vector2 leftBottom = new Vector2(spriteBounds.Left, spriteBounds.Bottom);
            Vector2 rightBottom = new Vector2(spriteBounds.Right, spriteBounds.Bottom);
            Vector2.Transform(ref leftTop, ref transformation, out leftTop);
            Vector2.Transform(ref rightTop, ref transformation, out rightTop);
            Vector2.Transform(ref leftBottom, ref transformation, out leftBottom);
            Vector2.Transform(ref rightBottom, ref transformation, out rightBottom);

            Vector2 min = Vector2.Min(Vector2.Min(leftTop, rightTop),
                Vector2.Min(leftBottom, rightBottom));
            Vector2 max = Vector2.Max(Vector2.Max(leftTop, rightTop),
                Vector2.Max(leftBottom, rightBottom));

            return new Rectangle((int) min.X,
                (int) min.Y,
                (int) (max.X - min.X),
                (int) (max.Y - min.Y));
        }

        public static bool IntersectPixels(Matrix transformA, int widthA, int heightA, Color[] dataA,
            Matrix transformB, int widthB, int heightB, Color[] dataB)
        {
            var collisionTrue = false;
            var shapeRenderer = CollisionRendering.renderer;

            // Matrix that transforms A's local space to world space and then to B's local space
            Matrix transformAToB = transformA * Matrix.Invert(transformB);
            // When a point moves in A's local space, it moves in B's local space with a
            // fixed direction and distance proportional to the movement in A.
            Vector2 stepX = Vector2.TransformNormal(Vector2.UnitX, transformAToB);
            Vector2 stepY = Vector2.TransformNormal(Vector2.UnitY, transformAToB);
            // Calculate the top left corner of A in B's local space
            Vector2 yPosInB = Vector2.Transform(Vector2.Zero, transformA);
            // For each row of pixels in A
            for (int yA = 0; yA < heightA; yA++)
            {
                Vector2 posInB = yPosInB; // Start at the beginning of the row
                // For each pixel in this row
                for (int xA = 0; xA < widthA; xA++)
                {
                    int xB = (int) Math.Round(posInB.X);
                    int yB = (int) Math.Round(posInB.Y);
                    // If the pixel lies within the bounds of B
                    if (0 <= xB && xB < widthB && 0 <= yB && yB < heightB)
                    {
                        // Get the colors of the overlapping pixels
                        Color colorA = dataA[xA + yA * widthA];
                        Color colorB = dataB[xB + yB * widthB];
                        if (colorA.A != 0 && colorB.A != 0)
                        {
//                            shapeRenderer.AddBoundingRectangle(new Rectangle(
//                                xA, yA, 1, 1
//                            ), Color.Yellow, 0.02f);
                            collisionTrue = true;
                        }
                        else
                        {
                            if (colorA.A != 0)
                            {
                                shapeRenderer.AddBoundingRectangle(new Rectangle(
                                    xA, yA, 1, 1
                                ), Color.Orange, 0.02f);
                            }
                            if (colorB.A != 0)
                            {
                                shapeRenderer.AddBoundingRectangle(new Rectangle(
                                    xB, yB, 1, 1
                                ), Color.OrangeRed, 0.02f);
                            }
                        }
                    }
                    posInB += stepX; // Move to the next pixel in the row
                }
                yPosInB += stepY; // Move to the next row
            }
            return collisionTrue;
//            return false;
        }

        public static bool IntersectPixs(
            Matrix transformA, Rectangle boundsA, Color[] dataA,
            Matrix transformB, Rectangle boundsB, Color[] dataB)
        {
            // Calculate a matrix which transforms from A's local space into
            // world space and then into B's local space
            Matrix transformAToB = transformA * Matrix.Invert(transformB);

            // For each row of pixels in A
            for (int yA = 0; yA < boundsA.Height; yA++)
            {
                // For each pixel in this row
                for (int xA = 0; xA < boundsA.Width; xA++)
                {
                    //Transform
                    Vector2 aPos = new Vector2(xA, yA);
                    Vector2 bPos = Vector2.Transform(aPos, transformAToB);

                    int xB = (int) Math.Round(bPos.X);
                    int yB = (int) Math.Round(bPos.Y);

                    // If the pixel lies within the bounds of B
                    if (boundsB.Contains(xB, yB))
                    {
                        // Get the colors of the overlapping pixels
                        Color colorA = dataA[xA + yA * boundsA.Width];
                        Color colorB = dataB[xB + yB * boundsB.Width];

                        // If both pixels are not completely transparent,
                        if (colorA.A != 0 && colorB.A != 0)
                        {
                            // then an intersection has been found
                            return true;
                        }
                    }
                }
            }
            // No intersection found
            return false;
        }
    }
}