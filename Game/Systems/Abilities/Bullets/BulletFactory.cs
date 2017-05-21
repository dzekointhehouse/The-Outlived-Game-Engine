using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Helpers;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using Spelkonstruktionsprojekt.ZEngine.Systems.InputHandler;
using ZEngine.Components;
using ZEngine.Managers;

namespace Spelkonstruktionsprojekt.ZEngine.Systems.Bullets
{
    public class BulletFactory
    {
        private static ComponentManager ComponentManager = ComponentManager.Instance;
        private static ComponentFactory ComponentFactory = ComponentManager.ComponentFactory;
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

        public void FireBullet(uint bulletEntityId, float direction)
        {
            var bulletRenderComponent = ComponentFactory.NewComponent<RenderComponent>();

            var bulletMoveComponent = ComponentFactory.NewComponent<MoveComponent>();
            bulletMoveComponent.AccelerationSpeed = 0;
            bulletMoveComponent.Speed = 2000;
            bulletMoveComponent.MaxVelocitySpeed = 2000;
            bulletMoveComponent.Direction = (float) direction;
            var bulletCollisionComponent = ComponentFactory.NewComponent<CollisionComponent>();

            ComponentManager.AddComponentToEntity(bulletMoveComponent, bulletEntityId);
            ComponentManager.AddComponentToEntity(bulletRenderComponent, bulletEntityId);
            ComponentManager.AddComponentToEntity(bulletCollisionComponent, bulletEntityId);
        }

        public uint CreateBullet(
            uint shooterId,
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


            var bulletshooterPosition = ComponentFactory.NewComponent<PositionComponent>();
            bulletshooterPosition.Position = finalPosition;
            bulletshooterPosition.ZIndex = shooterPosition.ZIndex;
            var bulletDimensionsComponent = ComponentFactory.NewComponent<DimensionsComponent>();
            bulletDimensionsComponent.Height = 10;
            bulletDimensionsComponent.Width = 10;
            var bulletComponent = ComponentFactory.NewComponent<BulletComponent>();
            bulletComponent.Damage = damage;
            bulletComponent.ShooterEntityId = shooterId;
            ComponentManager.AddComponentToEntity(bulletshooterPosition, bulletEntityId);
            ComponentManager.AddComponentToEntity(bulletDimensionsComponent, bulletEntityId);
            ComponentManager.AddComponentToEntity(bulletComponent, bulletEntityId);

            var spriteComponent = ComponentFactory.NewComponentFromLoadedSprite(PistolBulletSprite.Sprite,
                PistolBulletSprite.SpriteName);
            spriteComponent.Position = PistolBulletSprite.Position;
            spriteComponent.Alpha = PistolBulletSprite.Alpha;
            spriteComponent.TileHeight = PistolBulletSprite.TileHeight;
            spriteComponent.TileWidth = PistolBulletSprite.TileWidth;
            ComponentManager.AddComponentToEntity(spriteComponent, bulletEntityId);

            return bulletEntityId;
        }

        //Returns -1 as id if fail to create new bullet
        public uint CreatePistolBullet(uint shooterEntityId, double currentTime, int bulletDamage)
        {
            var shooterPosition =
                ComponentManager.Instance.GetEntityComponentOrDefault<PositionComponent>(shooterEntityId);
            var shooterDimensionsComponent =
                ComponentManager.Instance.GetEntityComponentOrDefault<DimensionsComponent>(shooterEntityId);
            if (shooterDimensionsComponent == null) return default(uint);
            var moveComponent =
                ComponentManager.Instance.GetEntityComponentOrDefault<MoveComponent>(shooterEntityId);
            // We create an new position instance for the bullet that starts from the player but should
            // not be the same as the players, as we found out when we did our test, otherwise the player
            // will follow the same way ass the bullet.

            uint bulletEntityId = CreateBullet(
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

        public uint[] CreateShotgunBullet(uint shooterEntityId, double currentTime, int bulletDamage)
        {
            var shooterPosition =
                ComponentManager.Instance.GetEntityComponentOrDefault<PositionComponent>(shooterEntityId);
            var shooterDimensionsComponent =
                ComponentManager.Instance.GetEntityComponentOrDefault<DimensionsComponent>(shooterEntityId);
            if (shooterDimensionsComponent == null) return new uint[]{};
            var moveComponent =
                ComponentManager.Instance.GetEntityComponentOrDefault<MoveComponent>(shooterEntityId);


            // We create an new position instance for the bullet that starts from the player but should
            // not be the same as the players, as we found out when we did our test, otherwise the player
            // will follow the same way ass the bullet.

//            var spread = new double[] {Math.PI * 0.1, Math.PI * -0.1, 0};
            var spread = new[] {-0.2, 0, 0.2};
//            var spread = new double[] {0};
            var bulletEntityIds = new uint[spread.Length];
            for (var i = 0; i < spread.Length; i++)
            {

                uint bulletEntityId = CreateBullet(
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