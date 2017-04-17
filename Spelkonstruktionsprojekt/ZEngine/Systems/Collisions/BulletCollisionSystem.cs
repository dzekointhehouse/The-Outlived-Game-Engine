﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZEngine.Components;
using ZEngine.EventBus;
using ZEngine.Managers;
using ZEngine.Systems;

namespace Spelkonstruktionsprojekt.ZEngine.Systems.Collisions
{
    class BulletCollisionSystem : ISystem
    {
        private EventBus EventBus = EventBus.Instance;
        private ComponentManager ComponentManager = ComponentManager.Instance;

        public void Start()
        {
            EventBus.Subscribe<SpecificCollisionEvent>("BulletCollision", Handle);
        }

        public void Handle(SpecificCollisionEvent collisionEvent)
        {
            Debug.WriteLine("Handle bullet collision");
            var entityMoveComponent = ComponentManager.Instance.GetEntityComponentOrDefault<MoveComponent>(collisionEvent.Entity);
            var entityRenderComponent = ComponentManager.Instance.GetEntityComponentOrDefault<RenderComponent>(collisionEvent.Entity);

            entityRenderComponent.PositionComponent.Position = entityMoveComponent.PreviousPosition;
            entityMoveComponent.Velocity = new Vector2(0, 0);

            var collisonComponent = ComponentManager.GetEntityComponentOrDefault<CollisionComponent>(collisionEvent.Entity);
            collisonComponent.collisions.Remove(collisionEvent.Target);
        }
    }
}