using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Helpers;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using ZEngine.Components;
using ZEngine.Managers;

namespace Game.Entities
{
    class PickupFactory
    {
        public void CreatePickups()
        {
            CreateFlyweightAmmopickupEntity();
            CreateFlyweightHealthpickupEntity();
        }
        private static void CreateFlyweightHealthpickupEntity()
        {
            var entity = EntityManager.GetEntityManager().NewEntity();
            var HealthpickupSprite = ComponentManager.Instance.ComponentFactory.NewComponent<SpriteComponent>();
            HealthpickupSprite.SpriteName = "healthpickup";
            var healthSpriteComponent = ComponentManager.Instance.ComponentFactory.NewComponent<FlyweightPickupComponent>();
            var soundComponent = ComponentManager.Instance.ComponentFactory.NewComponent<SoundComponent>();
            soundComponent.SoundEffectName = "pickup";
            ComponentManager.Instance.AddComponentToEntity(soundComponent, entity);
            ComponentManager.Instance.AddComponentToEntity(HealthpickupSprite, entity);
            ComponentManager.Instance.AddComponentToEntity(healthSpriteComponent, entity);
        }

        private static void CreateFlyweightAmmopickupEntity()
        {
            var entity = EntityManager.GetEntityManager().NewEntity();
            var ammopickupSprite = ComponentManager.Instance.ComponentFactory.NewComponent<SpriteComponent>();
            ammopickupSprite.SpriteName = "knife";
            var ammoPickUpSprite = ComponentManager.Instance.ComponentFactory.NewComponent<FlyweightPickupComponent>();
            var soundComponent = ComponentManager.Instance.ComponentFactory.NewComponent<SoundComponent>();
            soundComponent.SoundEffectName = "pickup";
            ComponentManager.Instance.AddComponentToEntity(soundComponent, entity);
            ComponentManager.Instance.AddComponentToEntity(ammopickupSprite, entity);
            ComponentManager.Instance.AddComponentToEntity(ammoPickUpSprite, entity);
        }
    }
}
