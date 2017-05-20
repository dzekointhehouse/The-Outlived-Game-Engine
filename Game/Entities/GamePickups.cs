using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Penumbra;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Components.PickupComponents;
using Spelkonstruktionsprojekt.ZEngine.Helpers;
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

        public void AddPickup(string spritename, PickupType pickup, Vector2 position)
        {
            var entity = new EntityBuilder()
                .SetRendering(40, 40)
                .SetRectangleCollision()
                .SetPosition(position, 500)
                .SetSprite(spritename)
                .SetLight(new PointLight())
                .SetSound("pickup")
                .Build()
                .GetEntityKey();

            if (pickup == PickupType.Health)
            {
                var pick = ComponentManager.Instance.ComponentFactory.NewComponent<HealthPickupComponent>();
                ComponentManager.Instance.AddComponentToEntity(pick, entity);
            }
            else if (pickup == PickupType.Ammo)
            {
                var pick = ComponentManager.Instance.ComponentFactory.NewComponent<AmmoPickupComponent>();
                ComponentManager.Instance.AddComponentToEntity(pick, entity);
            }
        }
    }
}
