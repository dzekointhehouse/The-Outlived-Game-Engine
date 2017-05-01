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
        public void Update()
        {
            var healthEntities = ComponentManager.Instance.GetEntitiesWithComponent(typeof(HealthComponent));
            foreach (var entity in healthEntities)
            {
                var entityId = entity.Key;
                var healthComponent = entity.Value as HealthComponent;
                if (healthComponent.CurrentHealth <= 0 && healthComponent.Alive)
                {
                    healthComponent.CurrentHealth = 0;
                    healthComponent.Alive = false;
                    Debug.WriteLine("Entity " + entityId + " has fallen");
                }
                while (healthComponent.Damage.Count > 0)
                {
                    var damage = healthComponent.Damage.First();
                    healthComponent.CurrentHealth -= damage;
                    healthComponent.Damage.Remove(damage);
                    Debug.WriteLine("HP: " + healthComponent.CurrentHealth);
                }

                //Option 1
       /*         if (healthComponent.CurrentHealth > healthComponent.MaxHealth)
                    healthComponent.CurrentHealth = healthComponent.MaxHealth;
       */
                //Option 2
                healthComponent.CurrentHealth = MathHelper.Min(healthComponent.CurrentHealth, healthComponent.MaxHealth);

            }
        }
    }
}