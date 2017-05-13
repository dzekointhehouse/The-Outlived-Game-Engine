using Spelkonstruktionsprojekt.ZEngine.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using Spelkonstruktionsprojekt.ZEngine.Systems.Collisions;
using ZEngine.Components;
using ZEngine.Managers;

namespace Spelkonstruktionsprojekt.ZEngine.Systems
{
    class HealthSystem : ISystem
    {
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
                for(var i = 0; i < healthComponent.Damage.Count; i++)
                {

                }
                

                healthComponent.CurrentHealth = MathHelper.Min(healthComponent.CurrentHealth, healthComponent.MaxHealth);
                CheckIfDead(entityId, healthComponent, gameTime);
            }
        }

        private void CheckIfDead(int entityId, HealthComponent healthComponent, GameTime gameTime)
        {
            if (healthComponent.CurrentHealth <= 0 && healthComponent.Alive)
            {
                healthComponent.CurrentHealth = 0;
                healthComponent.Alive = false;
                var positionComponent = ComponentManager.Instance.GetEntityComponentOrDefault<PositionComponent>(entityId);
                if (positionComponent != null)
                {
                    positionComponent.ZIndex = 2;
                }
                StateManager.TryAddState(entityId, State.Dead, gameTime.TotalGameTime.TotalMilliseconds);
            }
        }

    }
}