using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Penumbra;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Components.PickupComponents;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using ZEngine.Components;
using ZEngine.Managers;

namespace Game.Entities
{
    class GamePickups
    {
        public enum PickupType
        {
            Health,
            Ammo
        }

        private void AddPickup(string spritename, PickupType pickup)
        {
            var entity = EntityManager.GetEntityManager().NewEntity();
            var coll = new CollisionComponent();
            var dim = new DimensionsComponent()
            {
                Height = 40,
                Width = 40
            };
            var render = new RenderComponent()
            {
                IsVisible = true,
            };

            if (pickup == PickupType.Health)
            {
                var pick = new HealthPickupComponent();
                ComponentManager.Instance.AddComponentToEntity(pick, entity);
            }
            else if (pickup == PickupType.Ammo)
            {
                var pick = new HealthPickupComponent();
                ComponentManager.Instance.AddComponentToEntity(pick, entity);
            }

            var pos = new PositionComponent()
            {
                Position = new Vector2(40, 40)
            };
            var sprite = new SpriteComponent()
            {
                SpriteName = "healthpickup"
            };
            var ligh = new LightComponent()
            {
                Light = new PointLight() { },
            };
            ComponentManager.Instance.AddComponentToEntity(ligh, entity);
            ComponentManager.Instance.AddComponentToEntity(coll, entity);
            
            ComponentManager.Instance.AddComponentToEntity(pos, entity);
            ComponentManager.Instance.AddComponentToEntity(dim, entity);
            ComponentManager.Instance.AddComponentToEntity(render, entity);
            ComponentManager.Instance.AddComponentToEntity(sprite, entity);

        }
    }
}
