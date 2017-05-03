using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using Spelkonstruktionsprojekt.ZEngine.Systems.InputHandler;
using ZEngine.Components;
using ZEngine.EventBus;
using ZEngine.Managers;

namespace Spelkonstruktionsprojekt.ZEngine.Systems
{
    class EntityRemovalSystem : ISystem
    {
        public void Update(GameTime gameTime)
        {
            DeadEntities(gameTime);
            ImmediateRemoval();
        }

        private void ImmediateRemoval()
        {
            ComponentManager.Instance.GetEntitiesWithComponent(typeof(TagComponent))
                .Where(e =>
                {
                    var tagComponent = e.Value as TagComponent;
                    if (tagComponent == null) return false;
                    var isTaggedForDeletion = tagComponent.Tags.Contains(Tag.Delete);
                    return isTaggedForDeletion;
                })
                .Select(e => e.Key) //Get only entityIds that are to be removed
                .ToList()
                .ForEach(entityId =>
                {
                    var lightComponent = ComponentManager.Instance
                        .GetEntityComponentOrDefault<LightComponent>(entityId);
                    if (lightComponent != null)
                    {
                        lightComponent.Light.Enabled = false;
                    }
                    ComponentManager.Instance.DeleteEntity(entityId);
                });
        }

        // Used for animating blood when entities die.
        // They will ned to have the SpriteAnimationComponent.
        private void DeadEntities(GameTime gameTime)
        {
            var healthEntities = ComponentManager.Instance.GetEntitiesWithComponent(typeof(HealthComponent));

            foreach (var entity in healthEntities)
            {
                // Better yet would be to use a component to determine if they should be deleted
                // then when they should be deleted, and be able to get the associated components.
                var healthComponent = entity.Value as HealthComponent;
                if (!healthComponent.Alive)
                {
                    // We want to remove all the components for the entity except for the 
                    // spriteComponent and health, we need them still.
                    ComponentManager.Instance.RemoveComponentFromEntity(typeof(CameraFollowComponent), entity.Key);
                    ComponentManager.Instance.RemoveComponentFromEntity(typeof(PlayerComponent), entity.Key);
                    ComponentManager.Instance.RemoveComponentFromEntity(typeof(SoundComponent), entity.Key);
                    ComponentManager.Instance.RemoveComponentFromEntity(typeof(WeaponComponent), entity.Key);
                    ComponentManager.Instance.RemoveComponentFromEntity(typeof(ActionBindings), entity.Key);
                    ComponentManager.Instance.RemoveComponentFromEntity(typeof(CollisionComponent), entity.Key);
                    ComponentManager.Instance.RemoveComponentFromEntity(typeof(MoveComponent), entity.Key);
                    ComponentManager.Instance.RemoveComponentFromEntity(typeof(AIComponent), entity.Key);
                    //TODO reinsert removals
                    var lightComponent =
                        ComponentManager.Instance.GetEntityComponentOrDefault<LightComponent>(entity.Key);
                    if (lightComponent != null)
                        lightComponent.Light.Enabled = false;

                    var animationComponent =
                        ComponentManager.Instance.GetEntityComponentOrDefault<AnimationComponent>(entity.Key);
                    if (animationComponent == null)
                    {
                        animationComponent = new AnimationComponent();
                        ComponentManager.Instance.AddComponentToEntity(animationComponent, entity.Key);
                    }


                    // For this animation we need the lenght (to when we'll start fading the blood)
                    // this animation also needs to be unique, which means we won't create it 
                    // every time we come to this system. Also the gametime is needed.
                    var animation = new GeneralAnimation()
                    {
                        AnimationType = "BloodPool",
                        StartOfAnimation = gameTime.TotalGameTime.TotalMilliseconds,
                        Length = 6000,
                        Unique = true
                    };

                    // Now we add it.
                    AttachNewDeathFadeAwayAnimation(animation, entity.Key);
                    animationComponent.Animations.Add(animation);
                }
            }
        }

        // This one is fascinating. We add this to our entitys animation component instance which
        // contains a list of GeneralAnimations, this method is the action that is stored in the
        // general animation. This action will then be performed in another system and can do it's
        // own stuff independently of this system later on. Good for when we want to delete the sprite
        // later on.
        public void AttachNewDeathFadeAwayAnimation(GeneralAnimation animation, int entityKey)
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