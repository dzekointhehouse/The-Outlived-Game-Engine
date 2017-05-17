using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Components.SpriteAnimation;
using Spelkonstruktionsprojekt.ZEngine.Constants;
using Spelkonstruktionsprojekt.ZEngine.Helpers;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using ZEngine.Components;
using ZEngine.EventBus;
using ZEngine.Managers;
using ZEngine.Systems;
using Penumbra;
using Spelkonstruktionsprojekt.ZEngine.Components.PickupComponents;

namespace Spelkonstruktionsprojekt.ZEngine.Systems
{
    class SpawnSystem : ISystem
    {
        private ComponentManager ComponentManager = ComponentManager.Instance;

        public void CreateEnemy(int x, int y, SpriteComponent SpawnSpriteComponent)
        {

            var monster = new EntityBuilder()
                .SetPosition(new Vector2(x, y), layerDepth: 20)
                .SetRendering(200, 200)
                .SetSound("zombiewalking")
                .SetMovement(205, 5, 4, new Random(DateTime.Now.Millisecond).Next(0, 40) / 10)
                .SetArtificialIntelligence()
                .SetRectangleCollision()
                .SetSpawn()
                .SetHealth()
                .BuildAndReturnId();

            var animationBindings = new SpriteAnimationBindingsBuilder()
                .Binding(
                    new SpriteAnimationBindingBuilder()
                        .Positions(new Point(1252, 206), new Point(0, 1030))
                        .StateConditions(State.WalkingForward)
                        .Length(40)
                        .Build()
                )
                .Binding(
                    new SpriteAnimationBindingBuilder()
                        .Positions(new Point(0, 0), new Point(939, 206))
                        .StateConditions(State.Dead, State.WalkingForward)
                        .IsTransition(true)
                        .Length(30)
                        .Build()
                )
                .Binding(
                    new SpriteAnimationBindingBuilder()
                        .Positions(new Point(0, 0), new Point(939, 206))
                        .StateConditions(State.Dead, State.WalkingBackwards)
                        .IsTransition(true)
                        .Length(30)
                        .Build()
                )
                .Binding(
                    new SpriteAnimationBindingBuilder()
                        .Positions(new Point(0, 0), new Point(939, 206))
                        .StateConditions(State.Dead)
                        .IsTransition(true)
                        .Length(30)
                        .Build()
                )
                .Build();
            ComponentManager.AddComponentToEntity(SpawnSpriteComponent, monster);
            ComponentManager.AddComponentToEntity(animationBindings, monster);
        }
        public void HandleWaves()
        {
            //GlobalSpawn
            var GlobalSpawnEntities =
             ComponentManager.GetEntitiesWithComponent(typeof(GlobalSpawnComponent));
            if (GlobalSpawnEntities.Count <= 0) return;
            var GlobalSpawnComponent =
                ComponentManager.GetEntityComponentOrDefault<GlobalSpawnComponent>(GlobalSpawnEntities.First().Key);
            GlobalSpawnComponent.EnemiesDead = true;
            foreach (var entity in ComponentManager.GetEntitiesWithComponent(typeof(SpawnComponent)))
            {
                var HealthComponent = ComponentManager.GetEntityComponentOrDefault<HealthComponent>(entity.Key);
                if (HealthComponent == null) continue;
                if (HealthComponent.Alive)
                {
                    GlobalSpawnComponent.EnemiesDead = false;
                    // if anyone is alive break
                    break;
                }
            }
            if (GlobalSpawnComponent.EnemiesDead)
            {
                //SpawnSprite, the sprite for all monsters.
                var SpawnSpriteEntities =
                ComponentManager.GetEntitiesWithComponent(typeof(SpawnFlyweightComponent));
                if (SpawnSpriteEntities.Count <= 0) return;
                var SpawnSpriteComponent =
                    ComponentManager
                        .GetEntityComponentOrDefault<SpriteComponent>(SpawnSpriteEntities.First().Key);

                //camera
                var cameraEntities =
                ComponentManager.GetEntitiesWithComponent(typeof(CameraViewComponent));
                if (cameraEntities.Count <= 0) return;
                var cameraComponent =
                    (CameraViewComponent)cameraEntities.First().Value;
                //Health
                var HealthpickupEntities =
               ComponentManager.GetEntitiesWithComponent(typeof(FlyweightPickupComponent));
                if (HealthpickupEntities.Count <= 0) return;
                var HealthpickupComponent =
                    ComponentManager
                        .GetEntityComponentOrDefault<SpriteComponent>(HealthpickupEntities.First().Key);
                //Ammo
                var ammoPickUpEntities =
               ComponentManager.GetEntitiesWithComponent(typeof(FlyweightPickupComponent));
                if (ammoPickUpEntities.Count <= 0) return;
                var ammopickupComponent =
                    ComponentManager
                        .GetEntityComponentOrDefault<SpriteComponent>(ammoPickUpEntities.Last().Key);


                Random rand = new Random();
                Rectangle SpawnArea = new Rectangle();

                SpawnArea.Width = 100;
                SpawnArea.Height = 100;

                for (int i = 0; i < GlobalSpawnComponent.WaveSize; i++)
                {
                    do
                    {
                        SpawnArea.X = rand.Next(0, 2200);
                        SpawnArea.Y = rand.Next(0, 1100);
                    }
                    while (SpawnArea.Intersects(cameraComponent.View));
                    CreateEnemy(SpawnArea.Center.X, SpawnArea.Center.Y, SpawnSpriteComponent);
                }
                for (int j= 0; j < GlobalSpawnComponent.WaveSize; j++) {
                    
                    if(/*rand.Next(0, 1)*/1 == 1)
                    {
                        if (/*rand.Next(1, 3)*/2 == 1)
                        {
                           // CreatePickup(1,HealthpickupComponent);
                            
                        }
                        { CreatePickup(2, ammopickupComponent);}
                    }
                }
                GlobalSpawnComponent.WaveSize += GlobalSpawnComponent.WaveSizeIncreaseConstant;
            }
        }
       public int CreatePickup(int type, SpriteComponent pickupComponent)
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
           
            var pos = new PositionComponent()
            {
                Position = new Vector2(40, 40),
                ZIndex = 100
            };
            var sound = new SoundComponent()
            {
                SoundEffectName = "pickup"
            };
            var ligh = new LightComponent()
            {
                Light = new PointLight() { },
            };
            if (type == 1 )
            {
                ComponentManager.Instance.AddComponentToEntity(new HealthPickupComponent(), entity);
            }
            else
            {
                ComponentManager.Instance.AddComponentToEntity(new AmmoPickupComponent(), entity);
            }
            ComponentManager.Instance.AddComponentToEntity(pickupComponent, entity);
            ComponentManager.Instance.AddComponentToEntity(sound, entity);
            ComponentManager.Instance.AddComponentToEntity(ligh, entity);
            ComponentManager.Instance.AddComponentToEntity(coll, entity);
            ComponentManager.Instance.AddComponentToEntity(pos, entity);
            ComponentManager.Instance.AddComponentToEntity(dim, entity);
            ComponentManager.Instance.AddComponentToEntity(render, entity);
            return entity;
            //var entity = EntityManager.GetEntityManager().NewEntity();
            //var coll = new CollisionComponent();
            //var dim = new DimensionsComponent()
            //{
            //    Height = 40,
            //    Width = 40
            //};
            //var render = new RenderComponent()
            //{
            //    IsVisible = true,
            //};
            //var pos = new PositionComponent()
            //{
            //    Position = new Vector2(500, 500),
            //    ZIndex = 100
            //};

            //var sound = new SoundComponent()
            //{
            //    SoundEffectName = "pickup"
            //};
            //var ligh = new LightComponent()
            //{
            //    Light = new PointLight() { },
            //};

            //var pick = new HealthPickupComponent();

            //var sprite = new SpriteComponent()
            //{
            //    SpriteName = "knife",
            //};
            //ComponentManager.Instance.AddComponentToEntity(sprite, entity);
            ////Debug.WriteLine("YYYEEEEEEEEEEEEEEEEEAAAAAAAAAAHHHHHHHHHHHHH");

            //ComponentManager.Instance.AddComponentToEntity(pick, entity);

            ////if (type == 1)
            ////{
            ////    var pick = new HealthPickupComponent();

            ////    var sprite = new SpriteComponent()
            ////    {
            ////        SpriteName = "knife",
            ////    };
            ////    //ComponentManager.Instance.AddComponentToEntity(sprite, entity);
            ////    Debug.WriteLine("YYYEEEEEEEEEEEEEEEEEAAAAAAAAAAHHHHHHHHHHHHH");

            ////    ComponentManager.Instance.AddComponentToEntity(pick, entity);

            ////}
            ////else
            ////{
            ////    var pick = new AmmoPickupComponent();

            ////    var sprite = new SpriteComponent()
            ////    {
            ////        SpriteName = "knife",
            ////    };
            ////    //ComponentManager.Instance.AddComponentToEntity(sprite, entity);
            ////    Debug.WriteLine("YYYEEEEEEEEEEEEEEEEEAAAAAAAAAAHHHHHHHHHHHHH");

            ////    ComponentManager.Instance.AddComponentToEntity(pick, entity);
            ////}


            //ComponentManager.Instance.AddComponentToEntity(sound, entity);
            //ComponentManager.Instance.AddComponentToEntity(ligh, entity);
            //ComponentManager.Instance.AddComponentToEntity(coll, entity);
            //ComponentManager.Instance.AddComponentToEntity(pos, entity);
            //ComponentManager.Instance.AddComponentToEntity(dim, entity);
            //ComponentManager.Instance.AddComponentToEntity(render, entity);

            //return entity;

        }
    }
}
