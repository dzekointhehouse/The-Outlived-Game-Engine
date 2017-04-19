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

        public void TempEndGameIfDead(TempGameEnder tempGameEnder)
        {
            var healthEntities = ComponentManager.Instance.GetEntitiesWithComponent<HealthComponent>();
            foreach (var entity in healthEntities)
            {
                var currentHealth = entity.Value.MaxHealth - entity.Value.Damage.Sum();
//                Debug.WriteLine(currentHealth);
                if (currentHealth <= 0)
                {
                    var playerDeadSpriteEntity = ComponentManager.Instance
                        .GetEntitiesWithComponent<TempPlayerDeadSpriteComponent>()
                        .First()
                        .Key;
                    var playerDeadSpriteComponent = ComponentManager.Instance
                        .GetEntityComponentOrDefault<SpriteComponent>(playerDeadSpriteEntity);
                    if (playerDeadSpriteComponent == null)
                    {
                        Debug.WriteLine("No player dead sprite flyweight available.");
                    }

                    ComponentManager.Instance.RemoveComponentFromEntity(typeof(SpriteComponent), entity.Key);
                    ComponentManager.Instance.AddComponentToEntity(playerDeadSpriteComponent, entity.Key);

                    var lightComponent =
                        ComponentManager.Instance.GetEntityComponentOrDefault<LightComponent>(entity.Key);
                    lightComponent.Light.Scale = Vector2.Zero;

                    ComponentManager.Instance.RemoveComponentFromEntity(typeof(CameraFollowComponent), entity.Key);
                    var renderComponent =
                        ComponentManager.Instance.GetEntityComponentOrDefault<RenderComponent>(entity.Key);
                    renderComponent.DimensionsComponent.Width = (int) (renderComponent.DimensionsComponent.Width * 0.5);
                    renderComponent.DimensionsComponent.Height = (int) (renderComponent.DimensionsComponent.Height * 0.5);

                    ComponentManager.Instance.RemoveComponentFromEntity(typeof(HealthComponent), entity.Key);
                    ComponentManager.Instance.RemoveComponentFromEntity(typeof(PlayerComponent), entity.Key);
                    ComponentManager.Instance.RemoveComponentFromEntity(typeof(SoundComponent), entity.Key);
                    ComponentManager.Instance.RemoveComponentFromEntity(typeof(WeaponComponent), entity.Key);
                    ComponentManager.Instance.RemoveComponentFromEntity(typeof(ActionBindings), entity.Key);
                    ComponentManager.Instance.RemoveComponentFromEntity(typeof(CollisionComponent), entity.Key);
                    ComponentManager.Instance.RemoveComponentFromEntity(typeof(MoveComponent), entity.Key);
                }
            }
        }
    }
}
