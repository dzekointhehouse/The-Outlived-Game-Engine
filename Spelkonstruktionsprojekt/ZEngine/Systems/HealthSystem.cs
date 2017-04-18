using Spelkonstruktionsprojekt.ZEngine.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZEngine.Managers;

namespace Spelkonstruktionsprojekt.ZEngine.Systems
{
    class HealthSystem : ISystem
    {
        public void ApplyDamage()
        {
            var healthEntities = ComponentManager.Instance.GetEntitiesWithComponent<HealthComponent>();
            foreach (int key in healthEntities.Keys)
            {
                var healthComponent = healthEntities[key];
                if (!healthComponent.Alive) { break; }
                while(healthComponent.Damage.Count>0)
                {
                    int damage = healthComponent.Damage.First();
                    healthComponent.CurrentHealth -= damage;
                    healthComponent.Damage.Remove(damage);
                    Debug.WriteLine("HP: " + healthComponent.CurrentHealth);
                }
                if(healthComponent.CurrentHealth <= 0 && healthComponent.Alive)
                {
                    healthComponent.CurrentHealth = 0;
                    healthComponent.Alive = false;
                    Debug.WriteLine("Entity " + key + " has fallen");
                }
            }
        }
    }
}
