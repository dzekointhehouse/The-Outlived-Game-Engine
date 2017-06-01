using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using Spelkonstruktionsprojekt.ZEngine.Systems.InputHandler;
using UnityEngine;
using ZEngine.Components;
using ZEngine.EventBus;
using ZEngine.Managers;

namespace Spelkonstruktionsprojekt.ZEngine.Systems
{
    // Optimus prime
    class EntityRemovalSystem : ISystem
    {
        public void Start()
        {
            EventBus.Instance.Subscribe<StateChangeEvent>("StateChanged", _DeadEntities);
        }

        public void Update(GameTime gameTime)
        {
            ImmediateRemoval();
        } 

        private void ImmediateRemoval()
        {
            foreach (var entity in ComponentManager.Instance.GetEntitiesWithComponent(
                typeof(EntityDestructionComponent)))
            {
                var destructionComponent = entity.Value as EntityDestructionComponent;
                foreach (var id in destructionComponent.EntitiesToDestroy)
                {
                    var lightComponent = ComponentManager.Instance.GetEntityComponentOrDefault<LightComponent>(id);
                    if (lightComponent != null)
                    {
                        lightComponent.Light.Enabled = false;
                    };

                    EntityManager.GetEntityManager().DeleteEntity(id);
                }
                destructionComponent.EntitiesToDestroy.Clear();
            }
        }

        private void _DeadEntities(StateChangeEvent stateChangeEvent)
        {
            if (stateChangeEvent.NewState.Contains(State.Dead))
            {
                var entityId = stateChangeEvent.EntityId;
                // We want to remove all the components for the entity except for the 
                // spriteComponent and health, we need them still.
                ComponentManager.Instance.RemoveComponentFromEntity<CameraFollowComponent>(entityId);
                //ComponentManager.Instance.RemoveComponentFromEntity(typeof(PlayerComponent), entityId);
                ComponentManager.Instance.RemoveComponentFromEntity<SoundComponent>(entityId);
                ComponentManager.Instance.RemoveComponentFromEntity<WeaponComponent>(entityId);
                ComponentManager.Instance.RemoveComponentFromEntity<ActionBindings>(entityId);
                ComponentManager.Instance.RemoveComponentFromEntity<CollisionComponent>(entityId);
//                    ComponentManager.Instance.RemoveComponentFromEntity(typeof(MoveComponent), entityId);
                var moveComponent = ComponentManager.Instance
                    .GetEntityComponentOrDefault<MoveComponent>(entityId);
                if (moveComponent != null)
                {
                    moveComponent.Speed = 0;
                    moveComponent.CurrentAcceleration = 0;
                    moveComponent.AccelerationSpeed = 0;
                    moveComponent.RotationMomentum = 0;
                    moveComponent.RotationSpeed = 0;
                }

                ComponentManager.Instance.RemoveComponentFromEntity<AIComponent>(entityId);

                var lightComponent =
                    ComponentManager.Instance.GetEntityComponentOrDefault<LightComponent>(entityId);
                if (lightComponent != null)
                    lightComponent.Light.Enabled = false;

                var animationComponent =
                    ComponentManager.Instance.GetEntityComponentOrDefault<AnimationComponent>(entityId);
                if (animationComponent == null)
                {
                    animationComponent = ComponentManager.Instance.ComponentFactory.NewComponent<AnimationComponent>();
                    ComponentManager.Instance.AddComponentToEntity(animationComponent, entityId);
                }

                // For this animation we need the lenght (to when we'll start fading the blood)
                // this animation also needs to be unique, which means we won't create it 
                // every time we come to this system. Also the gametime is needed.
                var animation = new GeneralAnimation()
                {
                    AnimationType = "BloodPool",
                    StartOfAnimation = stateChangeEvent.EventTime,
                    Length = 6000,
                    Unique = true
                };

                // Now we add it.
                AttachNewDeathFadeAwayAnimation(animation, entityId);
                animationComponent.Animations.Add(animation);
            }
        }

        // This one is fascinating. We add this to our entitys animation component instance which
        // contains a list of GeneralAnimations, this method is the action that is stored in the
        // general animation. This action will then be performed in another system and can do it's
        // own stuff independently of this system later on. Good for when we want to delete the sprite
        // later on.
        public void AttachNewDeathFadeAwayAnimation(GeneralAnimation animation, uint entityKey)
        {
            animation.Animation = delegate(double currentTime)
            {
                // if more time has passed since the start of the animation
                // then the specified lenght that we want it to run
                var elapsedTime = currentTime - animation.StartOfAnimation;

                var sprite = ComponentManager.Instance.GetEntityComponentOrDefault<SpriteComponent>(entityKey);
                if (sprite == null) return;

                // This will make the blood disappear in a fading fashion.
                if (animation.Length < elapsedTime)
                {
                    if (sprite.Alpha > 0)
                    {
                        // fading rate can be adjusted here.
                        sprite.Alpha -= 0.0001f;
                        return;
                    }


//                    ComponentManager.Instance.RemoveComponentFromEntity(typeof(SpriteAnimationComponent), entityKey);
//                    ComponentManager.Instance.RemoveComponentFromEntity(typeof(HealthComponent), entityKey);
//                    ComponentManager.Instance.RemoveComponentFromEntity(typeof(SpriteComponent), entityKey);

                    animation.IsDone = true;
                }
            };
        }
    }
}