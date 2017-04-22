using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Systems.InputHandler;
using ZEngine.Components;
using ZEngine.EventBus;
using ZEngine.Managers;

namespace Spelkonstruktionsprojekt.ZEngine.Systems
{
    class EntityRemovalSystem : ISystem
    {
        EventBus EventBus = EventBus.Instance;

        public void Update(GameTime gameTime)
        {
            DeadEntities(gameTime);
        }

        private void DeadEntities(GameTime gameTime)
        {
            var healthEntities = ComponentManager.Instance.GetEntitiesWithComponent<HealthComponent>();
            foreach (var entity in healthEntities)
            {
                // Better yet would be to use a component to determine if they should be deleted
                // then when they should be deleted, and be able to get the associated components.
                if (!entity.Value.Alive)
                {
                    // We want to remove all the components for the entity except for the 
                    // spriteComponent and health, we need them still yet.
                    ComponentManager.Instance.RemoveComponentFromEntity(typeof(CameraFollowComponent), entity.Key);
                    ComponentManager.Instance.RemoveComponentFromEntity(typeof(PlayerComponent), entity.Key);
                    ComponentManager.Instance.RemoveComponentFromEntity(typeof(SoundComponent), entity.Key);
                    ComponentManager.Instance.RemoveComponentFromEntity(typeof(WeaponComponent), entity.Key);
                    ComponentManager.Instance.RemoveComponentFromEntity(typeof(ActionBindings), entity.Key);
                    ComponentManager.Instance.RemoveComponentFromEntity(typeof(CollisionComponent), entity.Key);
                    ComponentManager.Instance.RemoveComponentFromEntity(typeof(MoveComponent), entity.Key);

                    var animationComponent =
                        ComponentManager.Instance.GetEntityComponentOrDefault<AnimationComponent>(entity.Key);

                    if (animationComponent == null)
                    {
                        animationComponent = new AnimationComponent();
                        ComponentManager.Instance.AddComponentToEntity(animationComponent, entity.Key);
                    }



                    var animation = new GeneralAnimation()
                    {
                        // this seems to be weird
                        StartOfAnimation = gameTime.ElapsedGameTime.Milliseconds,
                        Length = 6000
                    };

                    var animationAction = NewDeathFadeAwayAnimation(animation, entity.Key);
                    animation.Animation = animationAction;

                    // Create the animation only if it doesn't already exist
                    if (!animationComponent.Animations.Contains(animation))
                        animationComponent.Animations.Add(animation);
                }
            }

            // This one is fascinating. We add this to our entitys animation component instance which
            // contains a list of GeneralAnimations, this method is the action that is stored in the
            // general animation. This action will then be performed in another system and can do it's
            // own stuff independently of this system later on. Good for when we want to delete the sprite
            // later on.

        }
        public Action<double> NewDeathFadeAwayAnimation(GeneralAnimation animation, int entityKey)
        {
            return delegate (double currentTime)
            {
                Debug.WriteLine("current: {0} start: {1} lenght : {2}", currentTime, animation.StartOfAnimation, animation.Length);
                Debug.WriteLine("Span: {0}", currentTime - animation.StartOfAnimation);
                // if more time has passed since the start of the animation
                // then the specified lenght that we want it to run

                var elapsedTime = currentTime - animation.StartOfAnimation;

                if (animation.Length < elapsedTime)
                {
                    var lightComponent =
                        ComponentManager.Instance.GetEntityComponentOrDefault<LightComponent>(entityKey);
                    lightComponent.Light.Scale = Vector2.Zero;
                    
                    //ComponentManager.Instance.RemoveComponentFromEntity(typeof(SpriteAnimationComponent), entityKey);
                    //ComponentManager.Instance.RemoveComponentFromEntity(typeof(HealthComponent), entityKey);
                    //ComponentManager.Instance.RemoveComponentFromEntity(typeof(SpriteComponent), entityKey);

                    animation.IsDone = true;
                }
            };
        }


    }
}


