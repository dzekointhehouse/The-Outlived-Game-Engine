using Spelkonstruktionsprojekt.ZEngine.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Constants;
using Spelkonstruktionsprojekt.ZEngine.Systems.InputHandler;
using ZEngine.Components;
using ZEngine.EventBus;
using ZEngine.Managers;

namespace Spelkonstruktionsprojekt.ZEngine.Systems
{
    class WeaponSystem : ISystem
    {
        private EventBus EventBus = EventBus.Instance;
        private ComponentManager ComponentManager = ComponentManager.Instance;

        public ISystem Start()
        {
            EventBus.Subscribe<InputEvent>(EventConstants.FireWeapon, HandleFireWeapon);
            return this;
        }

        public ISystem Stop()
        {
            return this;
        }

        public void HandleFireWeapon(InputEvent inputEvent)
        {
            if (inputEvent.KeyEvent == ActionBindings.KeyEvent.KeyPressed)
            {
                var weaponComponent =
                    ComponentManager.Instance.GetEntityComponentOrDefault<WeaponComponent>(inputEvent.EntityId);
                if (weaponComponent == default(WeaponComponent)) return;

                var renderComponent =
                                    ComponentManager.Instance.GetEntityComponentOrDefault<RenderComponent>(inputEvent.EntityId);
                var positionComponent =
                                    ComponentManager.Instance.GetEntityComponentOrDefault<PositionComponent>(inputEvent.EntityId);

                var moveComponent =
                                    ComponentManager.Instance.GetEntityComponentOrDefault<MoveComponent>(inputEvent.EntityId);

                var bulletSpriteEntities = ComponentManager.Instance.GetEntitiesWithComponent<BulletFlyweightComponent>();
                if (bulletSpriteEntities.Count <= 0) return;
                var bulletSpriteComponent =
                    ComponentManager.Instance.GetEntityComponentOrDefault<SpriteComponent>(bulletSpriteEntities.First().Key);
                CreateBullet(inputEvent, bulletSpriteComponent, weaponComponent, moveComponent, renderComponent, positionComponent);
            }
        }

        public void CreateBullet(InputEvent inputEvent, SpriteComponent bulletSpriteComponent, WeaponComponent weaponComponent, MoveComponent moveComponent, RenderComponent renderComponent, PositionComponent positionComponent)
        {
            //var x = positionComponent.Position.X;
            //var y = positionComponent.Position.Y;
            //var z = positionComponent.ZIndex;

            // We create an new position instance for the bullet that starts from the player but should
            // not be the same as the players, as we found out when we did our test, otherwise the player
            // will follow the same way ass the bullet.
            var bulletPositionComponent = new PositionComponent()
            {
                Position = new Vector2(positionComponent.Position.X, positionComponent.Position.Y),
                ZIndex = positionComponent.ZIndex
            };
            

            int bulletEntityId = EntityManager.GetEntityManager().NewEntity();
            //var bulletRenderComponent = new RenderComponentBuilder()
            //   // .Position(x, y, z)
            //    .Dimensions(10, 10)
            //    .Build();
            var bulletRenderComponent = new RenderComponent()
            {
                DimensionsComponent = new DimensionsComponent()
                {
                    Height = 10,
                    Width = 10
                }
            };
            var bulletMoveComponent = new MoveComponent()
            {
                AccelerationSpeed = 0,
                Speed = 1000,
                MaxVelocitySpeed = 1000,
                Direction = moveComponent.Direction
            };
            var bulletComponent = new BulletComponent()
            {
                Damage = weaponComponent.Damage,
                ShooterEntityId = inputEvent.EntityId
            };
            var bulletCollisionComponent = new CollisionComponent();

            ComponentManager.AddComponentToEntity(bulletPositionComponent, bulletEntityId);
            ComponentManager.AddComponentToEntity(bulletComponent, bulletEntityId);
            ComponentManager.AddComponentToEntity(bulletSpriteComponent, bulletEntityId);
            ComponentManager.AddComponentToEntity(bulletMoveComponent, bulletEntityId);
            ComponentManager.AddComponentToEntity(bulletRenderComponent, bulletEntityId);
            ComponentManager.AddComponentToEntity(bulletCollisionComponent, bulletEntityId);

            var animationComponent = new AnimationComponent();
            ComponentManager.AddComponentToEntity(animationComponent, bulletEntityId);

            var animation = new GeneralAnimation()
            {
                StartOfAnimation = inputEvent.EventTime,
                Length = 2000
            };
            var animationAction = NewBulletAnimation(animation, bulletEntityId);
            animation.Animation = animationAction;

            animationComponent.Animations.Add(animation);
        }

        public Action<double> NewBulletAnimation(GeneralAnimation generalAnimation, int entityId)
        {
            return delegate(double currentTimeInMilliseconds)
            {
                if (currentTimeInMilliseconds - generalAnimation.StartOfAnimation > generalAnimation.Length)
                {
                    ComponentManager.DeleteEntity(entityId);
                    generalAnimation.IsDone = true;
                }
            };
        }
    }
}
