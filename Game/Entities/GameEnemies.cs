using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Components.SpriteAnimation;
using Spelkonstruktionsprojekt.ZEngine.Helpers;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using ZEngine.Components;

namespace Game.Entities
{
    class GameEnemies
    {
        public void CreateMonster(string spriteName)
        {
            var x = new Random(DateTime.Now.Millisecond).Next(1000, 3000);
            var y = new Random(DateTime.Now.Millisecond).Next(1000, 3000);

            var monster = new EntityBuilder()
                .SetPosition(new Vector2(600, 120), layerDepth: 20)
                .SetRendering(200, 200)
                .SetSprite("zombie1", new Point(1244, 311), 311, 311)
                .SetSound("zombiewalking")
                .SetMovement(205, 5, 4, new Random(DateTime.Now.Millisecond).Next(0, 40) / 10)
                .SetArtificialIntelligence()
                .SetSpawn()
                .SetCollision()
                .SetHealth()
                //.SetHUD("hello")
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

            ComponentManager.Instance.AddComponentToEntity(animationBindings, monster);
            
            //TODO should be given GameTime total elapsed milliseconds instead of a 0
            StateManager.TryAddState(monster, State.WalkingForward, 0);
        }

        public static SpriteComponent GetZombieSpriteComponentOrNull()
        {
            var SpawnSpriteEntities =
                ComponentManager.Instance.GetEntitiesWithComponent(typeof(SpawnFlyweightComponent));

            if (SpawnSpriteEntities.Count <= 0) return null;
            var SpawnSpriteComponent = ComponentManager.Instance.GetEntityComponentOrDefault<SpriteComponent>(SpawnSpriteEntities.First().Key);
            return SpawnSpriteComponent;
        }
        
        public static void NewZombie(Vector2 position, SpriteComponent spriteComponent)
        {
            Dictionary<SoundComponent.SoundBank, SoundEffectInstance> soundList = new Dictionary<SoundComponent.SoundBank, SoundEffectInstance>(1);

            soundList.Add(SoundComponent.SoundBank.Death, OutlivedGame.Instance()
                .Content.Load<SoundEffect>("Sound/Splash")
                .CreateInstance());

            var monster = new EntityBuilder()
                .FromLoadedSprite(spriteComponent.Sprite, spriteComponent.SpriteName, new Point(1244, 311), 311, 311)
                .SetPosition(position, layerDepth: 20)
                .SetRendering(200, 200)
                .SetSound(soundList: soundList)
                .SetMovement(50, 50, 0.5f, new Random(DateTime.Now.Millisecond).Next(0, 40) / 10)
                .SetArtificialIntelligence()
                .SetSpawn()
                .SetCollision()
                .SetHealth()
                .BuildAndReturnId();

            var animationBindings = new SpriteAnimationBindingsBuilder()
                .Binding(
                    new SpriteAnimationBindingBuilder()
                        .Positions(new Point(1244, 311), new Point(622, 1244))
                        .StateConditions(State.WalkingForward)
                        .Length(60)
                        .Build()
                )
                .Binding(
                    new SpriteAnimationBindingBuilder()
                        .Positions(new Point(0, 0), new Point(933, 311))
                        .StateConditions(State.Dead, State.WalkingForward)
                        .IsTransition(true)
                        .Length(30)
                        .Build()
                )
                .Binding(
                    new SpriteAnimationBindingBuilder()
                        .Positions(new Point(0, 0), new Point(933, 311))
                        .StateConditions(State.Dead, State.WalkingBackwards)
                        .IsTransition(true)
                        .Length(30)
                        .Build()
                )
                .Binding(
                    new SpriteAnimationBindingBuilder()
                        .Positions(new Point(0, 0), new Point(933, 311))
                        .StateConditions(State.Dead)
                        .IsTransition(true)
                        .Length(30)
                        .Build()
                )
                .Build();

            ComponentManager.Instance.AddComponentToEntity(animationBindings, monster);

            //TODO SEND STATE MANAGER A GAME TIME VALUE AND NOT 0
            StateManager.TryAddState(monster, State.WalkingForward, 0);
        }
    }
}
