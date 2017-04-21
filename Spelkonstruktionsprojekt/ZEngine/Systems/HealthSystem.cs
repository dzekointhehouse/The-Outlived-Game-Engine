using Spelkonstruktionsprojekt.ZEngine.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Systems.Collisions;
using ZEngine.Components;
using ZEngine.Managers;

namespace Spelkonstruktionsprojekt.ZEngine.Systems
{
    class HealthSystem : ISystem
    {
        public void Update()
        {
            var healthEntities = ComponentManager.Instance.GetEntitiesWithComponent<HealthComponent>();
            foreach (int key in healthEntities.Keys)
            {
                var healthComponent = healthEntities[key];
                if (healthComponent.CurrentHealth <= 0 && healthComponent.Alive)
                {
                    healthComponent.CurrentHealth = 0;
                    healthComponent.Alive = false;
                    Debug.WriteLine("Entity " + key + " has fallen");
                }
                while (healthComponent.Damage.Count>0)
                {
                    int damage = healthComponent.Damage.First();
                    healthComponent.CurrentHealth -= damage;
                    healthComponent.Damage.Remove(damage);
                    Debug.WriteLine("HP: " + healthComponent.CurrentHealth);
                }

            }
        }
    }
}
