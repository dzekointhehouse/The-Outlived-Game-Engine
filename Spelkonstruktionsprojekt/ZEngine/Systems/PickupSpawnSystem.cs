using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Components.PickupComponents;
using Spelkonstruktionsprojekt.ZEngine.Constants;
using Spelkonstruktionsprojekt.ZEngine.Helpers;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using System;
using System.Linq;
using Penumbra;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Systems;
using ZEngine.Components;
using ZEngine.EventBus;
using ZEngine.Managers;

namespace ZEngine.Systems
{
    public class PickupSpawnSystem : ISystem, IUpdateables
    {
        public bool Enabled { get; set; } = true;
        public int UpdateOrder { get; set; }

        public void Update(GameTime gt)
        {
        }

        private EventBus.EventBus EventBus = ZEngine.EventBus.EventBus.Instance;
        private ComponentManager ComponentManager = ComponentManager.Instance;
        private Random rand;

        private enum PickupType
        {
            HealthPickup,
            AmmoPickup
        }

        public void Start()
        {
            EventBus.Subscribe<uint>(EventConstants.Death, SpawnPickup);
            rand = new Random();
        }

        private void SpawnPickup(uint entityId)
        {
            //if (!ComponentManager.EntityHasComponent<AIComponent>(entityId)) { return; }
            PositionComponent positionComponent = ComponentManager.GetEntityComponentOrDefault<PositionComponent>(entityId);
            if (positionComponent == null) { return; }

            if (rand.Next(0, 1) == 0)
            {
                if (rand.Next(0, 2) == 0)
                {

                    var HealthpickupEntities =
                        ComponentManager.GetEntitiesWithComponent(typeof(FlyweightPickupComponent));

                    var HealthpickupComponent =
                        ComponentManager
                            .GetEntityComponentOrDefault<SpriteComponent>(HealthpickupEntities.First().Key);
                    var soundComponent =
                        ComponentManager
                            .GetEntityComponentOrDefault<SoundComponent>(HealthpickupEntities.First().Key);

                    CreatePickup(HealthpickupComponent, positionComponent.Position, soundComponent);
                }
                else
                {
                    
                    var HealthpickupEntities =
                        ComponentManager.GetEntitiesWithComponent(typeof(FlyweightPickupComponent));

                    var HealthpickupComponent =
                        ComponentManager
                            .GetEntityComponentOrDefault<SpriteComponent>(HealthpickupEntities.Last().Key);
                    var soundComponent =
                        ComponentManager
                            .GetEntityComponentOrDefault<SoundComponent>(HealthpickupEntities.Last().Key);

                    CreatePickup(HealthpickupComponent, positionComponent.Position, soundComponent);
                }

            }

        }
        private void AddPickup(PickupType pickup, Vector2 position)
        {
            //TODO: check out component factory it has issues, it does not really work well with pickups!

            //var flyweightEntityList = ComponentManager.GetEntitiesWithComponent(typeof(FlyweightPickupComponent));
            //if (flyweightEntityList.Count == 0){return;}
            //var flyweightEntity = flyweightEntityList.FirstOrDefault();
            //var spriteComponent = ComponentManager.GetEntityComponentOrDefault<SpriteComponent>(flyweightEntity.Key);
            //var soundComponent = ComponentManager.GetEntityComponentOrDefault<SoundComponent>(flyweightEntity.Key);



            //var entity = new EntityBuilder()
            //    .SetRendering(40, 40)
            //    .SetRectangleCollision()
            //    .SetPosition(position, 500)        
            //    .Build()
            //    .GetEntityKey();



            //ComponentManager.AddComponentToEntity(soundComponent, entity);


            //if (pickup == PickupType.HealthPickup)
            //{
            //    var pick = ComponentManager.Instance.ComponentFactory.NewComponent<HealthPickupComponent>();
            //    ComponentManager.Instance.AddComponentToEntity(pick, entity);
            //    ComponentManager.AddComponentToEntity(spriteComponent, entity);
            //}
            //else if (pickup == PickupType.AmmoPickup)
            //{
            //    var pick = ComponentManager.Instance.ComponentFactory.NewComponent<AmmoPickupComponent>();
            //    ComponentManager.Instance.AddComponentToEntity(pick, entity);
            //    ComponentManager.AddComponentToEntity(spriteComponent, entity);
            //}
        }

        private uint CreatePickup(SpriteComponent pickupSprite, Vector2 position, SoundComponent sound)
        {
            var entity = new EntityBuilder()
                .SetSound("pickup", soundEffect: sound.SoundEffect)
                .SetPosition(new Vector2(position.X, position.Y), 100)
                .SetRendering(40, 40)
                .SetSprite(
                    spriteName: pickupSprite.SpriteName, 
                    sprite: pickupSprite.Sprite, 
                    tileWidth: pickupSprite.TileWidth,
                    tileHeight: pickupSprite.TileHeight,
                    scale: pickupSprite.Scale,
                    startPosition: pickupSprite.Position)
                .SetLight(new PointLight())
                .SetRectangleCollision()
                .BuildAndReturnId();

            if (pickupSprite.SpriteName == "healthpickup")
            {
                //ComponentManager.Instance.AddComponentToEntity(
                //    ComponentManager.Instance.ComponentFactory.NewComponent<HealthPickupComponent>(),
                //    entity
                //);

            ComponentManager.Instance.AddComponentToEntity(new HealthPickupComponent(), entity);
            }

            else if(pickupSprite.SpriteName == "knife")
            {
                //ComponentManager.Instance.AddComponentToEntity(
                //    ComponentManager.Instance.ComponentFactory.NewComponent<HealthPickupComponent>(),
                //    entity
                //);

                ComponentManager.Instance.AddComponentToEntity(new AmmoPickupComponent(), entity);
            }
            return entity;
        }
    }
}
