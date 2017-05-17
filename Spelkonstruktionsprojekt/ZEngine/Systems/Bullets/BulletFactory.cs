using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using Spelkonstruktionsprojekt.ZEngine.Systems.InputHandler;
using ZEngine.Components;
using ZEngine.Managers;

namespace Spelkonstruktionsprojekt.ZEngine.Systems.Bullets
{
    public class BulletFactory
    {
        private ComponentManager ComponentManager = ComponentManager.Instance;
        private SpriteComponent PistolBulletSprite;

        private float xOffset = 100;
        private float yOffset = 73;
        //Loads available bullet sprites, given that the neccesary entities containing
        // these sprites are created.
        public void LoadBulletSprites()
        {
            var bulletSpriteEntities =
                ComponentManager.Instance.GetEntitiesWithComponent(typeof(BulletFlyweightComponent));
            if (bulletSpriteEntities.Count <= 0) throw new Exception("Global bullet sprite not yet loaded! Cannot create bullet.");
            PistolBulletSprite =
                ComponentManager.Instance
                    .GetEntityComponentOrDefault<SpriteComponent>(bulletSpriteEntities.First().Key);
        }

        private int CreateBullet(
            int shooterEntityId,
            PositionComponent shooterPosition,
            Matrix transformation,
            double direction,
            int damage)
        {

            var bulletPos = new Vector2(shooterPosition.Position.X + 50, shooterPosition.Position.Y + 24);
            var finalPosition = Vector2.Transform(bulletPos, transformation);
            var bulletshooterPosition = new PositionComponent()
            {
                Position = finalPosition,
                ZIndex = shooterPosition.ZIndex
            };

            int bulletEntityId = EntityManager.GetEntityManager().NewEntity();

            var bulletRenderComponent = new RenderComponent();

            var bulletDimensionsComponent = new DimensionsComponent()
            {
                Height = 10,
                Width = 10
            };

            var bulletMoveComponent = new MoveComponent()
            {
                AccelerationSpeed = 0,
                Speed = 1000,
                MaxVelocitySpeed = 1000,
                Direction = (float) direction
            };
            var bulletComponent = new BulletComponent()
            {
                Damage = damage,
                ShooterEntityId = shooterEntityId
            };
            var bulletCollisionComponent = new CollisionComponent();

            ComponentManager.AddComponentToEntity(bulletshooterPosition, bulletEntityId);
            ComponentManager.AddComponentToEntity(bulletComponent, bulletEntityId);
            ComponentManager.AddComponentToEntity(PistolBulletSprite, bulletEntityId);
            ComponentManager.AddComponentToEntity(bulletMoveComponent, bulletEntityId);
            ComponentManager.AddComponentToEntity(bulletRenderComponent, bulletEntityId);
            ComponentManager.AddComponentToEntity(bulletDimensionsComponent, bulletEntityId);
            ComponentManager.AddComponentToEntity(bulletCollisionComponent, bulletEntityId);

            return bulletEntityId;
        }

        //Returns -1 as id if fail to create new bullet
        public int CreatePistolBullet(int shooterEntityId, double currentTime, int bulletDamage)
        {
            var positionComponent =
                ComponentManager.Instance.GetEntityComponentOrDefault<PositionComponent>(shooterEntityId);
            var shooterDimensionsComponent =
                ComponentManager.Instance.GetEntityComponentOrDefault<DimensionsComponent>(shooterEntityId);
            if (shooterDimensionsComponent == null) return -1;
            var moveComponent =
                ComponentManager.Instance.GetEntityComponentOrDefault<MoveComponent>(shooterEntityId);
            // We create an new position instance for the bullet that starts from the player but should
            // not be the same as the players, as we found out when we did our test, otherwise the player
            // will follow the same way ass the bullet.
            var matrixA =
                Matrix.CreateTranslation(new Vector3(
                    (float) (-positionComponent.Position.X - shooterDimensionsComponent.Width * 0.5 + 100),
                    (float) (-positionComponent.Position.Y - shooterDimensionsComponent.Height * 0.5 + 75), 0)) *
                Matrix.CreateRotationZ(moveComponent.Direction) *
                Matrix.CreateTranslation(positionComponent.Position.X, positionComponent.Position.Y, 0f);

            var bulletEntityId = CreateBullet(
                shooterEntityId,
                positionComponent,
                matrixA,
                moveComponent.Direction,
                bulletDamage);

            return bulletEntityId;
        }

        public int[] CreateShotgunBullet(int shooterEntityId, double currentTime, int bulletDamage)
        {
            var positionComponent =
                ComponentManager.Instance.GetEntityComponentOrDefault<PositionComponent>(shooterEntityId);
            var shooterDimensionsComponent =
                ComponentManager.Instance.GetEntityComponentOrDefault<DimensionsComponent>(shooterEntityId);
            if (shooterDimensionsComponent == null) return new int[]{};
            var moveComponent =
                ComponentManager.Instance.GetEntityComponentOrDefault<MoveComponent>(shooterEntityId);


            // We create an new position instance for the bullet that starts from the player but should
            // not be the same as the players, as we found out when we did our test, otherwise the player
            // will follow the same way ass the bullet.

//            var spread = new double[] {Math.PI * 0.1, Math.PI * -0.1, 0};
            var spread = new double[] {-0.2, 0, 0.2};
//            var spread = new double[] {0};
            var bulletEntityIds = new int[spread.Length];
            for (var i = 0; i < spread.Length; i++)
            {
                var matrixA =
                    Matrix.CreateTranslation(-positionComponent.Position.X, -positionComponent.Position.Y, 0f) *
                    Matrix.CreateRotationZ(moveComponent.Direction) *
                    Matrix.CreateTranslation(new Vector3(positionComponent.Position.X, positionComponent.Position.Y, 0));

                bulletEntityIds[i] =
                    CreateBullet(
                        shooterEntityId,
                        positionComponent,
                        matrixA,
                        moveComponent.Direction + spread[i],
                        bulletDamage);
            }
            return bulletEntityIds;
        }
    }
}