using Spelkonstruktionsprojekt.ZEngine.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Constants;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using Spelkonstruktionsprojekt.ZEngine.Systems.Collisions;
using ZEngine.Components;
using ZEngine.EventBus;
using ZEngine.Managers;

namespace Spelkonstruktionsprojekt.ZEngine.Systems
{
    public class HealthSystem : ISystem, IUpdateables
    {
        public bool Enabled { get; set; } = true;
        public int UpdateOrder { get; set; }

        public void Update(GameTime gameTime)
        {
            var healthEntities = ComponentManager.Instance.GetEntitiesWithComponent(typeof(HealthComponent));
            foreach (var entity in healthEntities)
            {
                var entityId = entity.Key;
                var healthComponent = entity.Value as HealthComponent;

                while (healthComponent.Damage.Count > 0)
                {
                    var damage = healthComponent.Damage.First();
                    healthComponent.CurrentHealth -= damage;
                    healthComponent.Damage.Remove(damage);
                }

                healthComponent.CurrentHealth = MathHelper.Min(healthComponent.CurrentHealth, healthComponent.MaxHealth);
                CheckIfDead(entityId, healthComponent, gameTime);
            }
        }

        public bool CheckIfAllPlayersAreDead()
        {
            var playerEntities = ComponentManager.Instance.GetEntitiesWithComponent(typeof(PlayerComponent));

            foreach (var soldier in playerEntities)
            {
                var healthComponent = ComponentManager.Instance.GetEntityComponentOrDefault<HealthComponent>(soldier.Key);

                if (healthComponent.IsAlive) return false;
            }
            return true;
        }

        private void CheckIfDead(uint entityId, HealthComponent healthComponent, GameTime gameTime)
        {
            if (healthComponent.CurrentHealth <= 0 && healthComponent.IsAlive)
            {
                healthComponent.CurrentHealth = 0;
                healthComponent.IsAlive = false;
                var positionComponent = ComponentManager.Instance.GetEntityComponentOrDefault<PositionComponent>(entityId);

                var nCameras = ComponentManager.Instance.GetEntitiesWithComponent(typeof(CameraViewComponent)).Count;

                // if the entity is a player and there are multiple cameras, we want to create a special event.
                if (nCameras > 1 && ComponentManager.Instance.EntityHasComponent(typeof(PlayerComponent), entityId))
                {
                    EventBus.Instance.Publish(EventConstants.PlayerDeath, entityId);
                    healthComponent.IsAlive = true;
                    healthComponent.CurrentHealth = healthComponent.MaxHealth;
                }
                else
                {
                    if (positionComponent != null)
                    {
                        positionComponent.ZIndex = 2;
                    }
                    EventBus.Instance.Publish(EventConstants.Death, entityId);
                    StateManager.TryAddState(entityId, State.Dead, gameTime.TotalGameTime.TotalMilliseconds);
                }
            }
        }

    }
}
