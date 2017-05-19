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

        private float xOffset = 65;
        private float yOffset = 52;
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

        public void FireBullet(int bulletEntityId, float direction)
        {
            var bulletRenderComponent = new RenderComponent();

            var bulletMoveComponent = new MoveComponent()
            {
                AccelerationSpeed = 0,
                Speed = 2000,
                MaxVelocitySpeed = 2000,
                Direction = (float) direction
            };
            var bulletCollisionComponent = new CollisionComponent();

            ComponentManager.AddComponentToEntity(bulletMoveComponent, bulletEntityId);
            ComponentManager.AddComponentToEntity(bulletRenderComponent, bulletEntityId);
            ComponentManager.AddComponentToEntity(bulletCollisionComponent, bulletEntityId);
        }

        public int CreateBullet(
            int shooterId,
            PositionComponent shooterPosition,
            DimensionsComponent shooterDimensions,
            int damage,
            float direction)
        {
            // We create an new position instance for the bullet that starts from the player but should
            // not be the same as the players, as we found out when we did our test, otherwise the player
            // will follow the same way ass the bullet.
            var transformation =
                Matrix.CreateTranslation(new Vector3(
                    (float) (-shooterPosition.Position.X - shooterDimensions.Width * 0.5 + xOffset),
                    (float) (-shooterPosition.Position.Y - shooterDimensions.Height * 0.5 + yOffset), 0)) *
                Matrix.CreateRotationZ(direction) *
                Matrix.CreateTranslation(shooterPosition.Position.X, shooterPosition.Position.Y, 0f);

            var bulletPos = new Vector2(shooterPosition.Position.X + 50, shooterPosition.Position.Y + 24);
            var finalPosition = Vector2.Transform(bulletPos, transformation);
            var bulletEntityId = EntityManager.GetEntityManager().NewEntity();


            var bulletshooterPosition = new PositionComponent()
            {
                Position = finalPosition,
                ZIndex = shooterPosition.ZIndex
            };
            var bulletDimensionsComponent = new DimensionsComponent()
            {
                Height = 10,
                Width = 10
            };
            var bulletComponent = new BulletComponent()
            {
                Damage = damage,
                ShooterEntityId = shooterId
            };
            ComponentManager.AddComponentToEntity(bulletshooterPosition, bulletEntityId);
            ComponentManager.AddComponentToEntity(bulletDimensionsComponent, bulletEntityId);
            ComponentManager.AddComponentToEntity(bulletComponent, bulletEntityId);
            ComponentManager.AddComponentToEntity(PistolBulletSprite, bulletEntityId);

            return bulletEntityId;
        }

        //Returns -1 as id if fail to create new bullet
        public int CreatePistolBullet(int shooterEntityId, double currentTime, int bulletDamage)
        {
            var shooterPosition =
                ComponentManager.Instance.GetEntityComponentOrDefault<PositionComponent>(shooterEntityId);
            var shooterDimensionsComponent =
                ComponentManager.Instance.GetEntityComponentOrDefault<DimensionsComponent>(shooterEntityId);
            if (shooterDimensionsComponent == null) return -1;
            var moveComponent =
                ComponentManager.Instance.GetEntityComponentOrDefault<MoveComponent>(shooterEntityId);
            // We create an new position instance for the bullet that starts from the player but should
            // not be the same as the players, as we found out when we did our test, otherwise the player
            // will follow the same way ass the bullet.

            int bulletEntityId = CreateBullet(
                shooterEntityId,
                shooterPosition,
                shooterDimensionsComponent,
                bulletDamage,
                moveComponent.Direction);

            FireBullet(
                bulletEntityId,
                moveComponent.Direction);

            return bulletEntityId;
        }

        public int[] CreateShotgunBullet(int shooterEntityId, double currentTime, int bulletDamage)
        {
            var shooterPosition =
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
            var spread = new[] {-0.2, 0, 0.2};
//            var spread = new double[] {0};
            var bulletEntityIds = new int[spread.Length];
            for (var i = 0; i < spread.Length; i++)
            {

                int bulletEntityId = CreateBullet(
                    shooterEntityId,
                    shooterPosition,
                    shooterDimensionsComponent,
                    bulletDamage,
                    moveComponent.Direction);

                bulletEntityIds[i] = bulletEntityId;
                FireBullet(
                    bulletEntityId,
                    (float) (moveComponent.Direction + spread[i])
                );
            }
            return bulletEntityIds;
        }
    }
}