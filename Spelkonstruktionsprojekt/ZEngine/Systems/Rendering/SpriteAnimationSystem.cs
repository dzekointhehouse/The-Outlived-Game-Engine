using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MoreLinq;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Components.SpriteAnimation;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using ZEngine.Components;
using ZEngine.EventBus;
using ZEngine.Managers;

namespace Spelkonstruktionsprojekt.ZEngine.Systems
{
    class SpriteAnimationSystem : ISystem
    {
        private readonly ComponentManager ComponentManager = ComponentManager.Instance;

        public void Start()
        {
            EventBus.Instance.Subscribe<StateChangeEvent>("StateChanged", UpdateAnimationState);
        }

        // This method is called when using this system.
        // This will activate an animation for all the
        // entities that have an SpriteAnimationComponent.
        public void Update(GameTime gameTime)
        {
//            DeathAnimation(gameTime);
            ProcessAnimations(gameTime);
        }

        private void UpdateAnimationState(StateChangeEvent stateChangeEvent)
        {
            var entityId = stateChangeEvent.EntityId;
            if (!ComponentManager.ContainsAllComponents(
                entityId,
                typeof(SpriteAnimationBindings)
            )) return;
            var animationBindings = ComponentManager.GetEntityComponentOrDefault<SpriteAnimationBindings>(entityId);

            var spriteAnimation = ComponentManager.GetEntityComponentOrDefault<SpriteAnimationComponent>(entityId);
            if (spriteAnimation == null)
            {
                spriteAnimation = new SpriteAnimationComponent();
                ComponentManager.AddComponentToEntity(spriteAnimation, entityId);
                spriteAnimation.AnimationStarted = stateChangeEvent.EventTime;
            }

            var binding = animationBindings.Bindings
                .FirstOrDefault(e => BindingsMatch(e.StateConditions, stateChangeEvent.NewState));
            if (binding == null)
            {
                spriteAnimation.NextAnimatedState = null;
            }
            else
            {
                spriteAnimation.NextAnimatedState = binding;
            }
        }

        private void ProcessAnimations(GameTime gameTime)
        {
            var entities = ComponentManager.GetEntitiesWithComponent(typeof(SpriteAnimationComponent));
            entities.ForEach(e => ProcessAnimation(gameTime, e.Key, (SpriteAnimationComponent) e.Value));
        }

        private void ProcessAnimation(GameTime gameTime, int entityId, SpriteAnimationComponent spriteAnimation)
        {
            if (!ComponentManager.ContainsAllComponents(
                entityId,
                typeof(SpriteComponent)
            )) return;
            var spriteComponent = ComponentManager.GetEntityComponentOrDefault<SpriteComponent>(entityId);

            if (spriteAnimation.CurrentAnimatedState == null && spriteAnimation.NextAnimatedState != null)
            {
                spriteAnimation.CurrentAnimatedState = spriteAnimation.NextAnimatedState;
            }
            else if (spriteAnimation.CurrentAnimatedState == null)
            {
                return;
            }

            var binding = spriteAnimation.CurrentAnimatedState;
            if (!timeEnoughToFlipFrame(gameTime, spriteAnimation, binding)) return;

            //Incremenet the x position by one tile width.
            var newX = spriteComponent.Position.X + spriteComponent.TileWidth;
            var newY = spriteComponent.Position.Y;
            //Check if the new positions exceeds the end position of the animation loop
            if (newY > binding.EndPosition.Y || newY == binding.EndPosition.Y && newX >= binding.EndPosition.X)
            {
                newX = binding.StartPosition.X;
                newY = binding.StartPosition.Y;
                spriteAnimation.CurrentAnimatedState = spriteAnimation.NextAnimatedState;
            }
            //Check if new sprite crop bounds exceeds the sprites actual bounds
            else if (newX + spriteComponent.TileWidth > spriteComponent.Sprite.Width)
            {
                newX = binding.StartPosition.X;
                newY = spriteComponent.Position.Y + spriteComponent.TileHeight;
                if (newY + spriteComponent.TileHeight > spriteComponent.Sprite.Height)
                {
                    newY = binding.StartPosition.Y;
                }
            }
            spriteComponent.Position = new Point(newX, newY);
            spriteAnimation.AnimationStarted = gameTime.TotalGameTime.TotalMilliseconds;
        }

        private bool timeEnoughToFlipFrame(GameTime gameTime, SpriteAnimationComponent spriteAnimationComponent,
            SpriteAnimationBinding binding)
        {
            var elapsedTimeForFrame = gameTime.TotalGameTime.TotalMilliseconds -
                                      spriteAnimationComponent.AnimationStarted;

            if (elapsedTimeForFrame >= binding.FrameLength)
            {
                return true;
            }
            return false;
        }

        private bool BindingsMatch(List<State> source, ImmutableList<State> target)
        {
            return source.All(target.Contains)
                   && source.Count == target.Count;
        }

        // DeathAnimation as it states in the method name is used for entities that
        // have a sprite, sprite animation and a health component instance. This is 
        // intented to be used so that the entities show a splash of blood when they die.
        private void DeathAnimation(GameTime gameTime)
        {
            var animationComponents = ComponentManager.Instance.GetEntitiesWithComponent(typeof(SpriteAnimationComponent));

            foreach (var animation in animationComponents)
            {
                var animationComponent = animation.Value as SpriteAnimationComponent;
                var currentFrame = animationComponent.CurrentFrame;
                var sheetSize = animationComponent.SpritesheetSize;
                var frameSize = animationComponent.FrameSize;

                if (frameSize == default(Point))
                {
                    animationComponent.FrameSize = new Point(
                        animationComponent.Spritesheet.Width / animationComponent.SpritesheetSize.X,
                        animationComponent.Spritesheet.Height / animationComponent.SpritesheetSize.Y
                    );

                    frameSize = animationComponent.FrameSize;
                }

                // the sprite component so we can set the new texture to render upon death.
                //if (ComponentManager.Instance.EntityHasComponent(typeof(HealthComponent), animation.Key))
                //    continue;

                var health = ComponentManager.Instance.GetEntityComponentOrDefault<HealthComponent>(animation.Key);
                var sprite = ComponentManager.Instance.GetEntityComponentOrDefault<SpriteComponent>(animation.Key);

                if (currentFrame.X != (sheetSize.X - 1) && currentFrame.Y != sheetSize.Y)
                {
                    if (!health.Alive)
                    {
                        animationComponent.TimeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;

                        if (animationComponent.TimeSinceLastFrame > animationComponent.MillisecondsPerFrame)
                        {
                            animationComponent.TimeSinceLastFrame -= animationComponent.MillisecondsPerFrame;
                            ++currentFrame.X;

                            if (currentFrame.X >= animationComponent.SpritesheetSize.X)
                            {
                                currentFrame.X = 0;
                                ++currentFrame.Y;

                                if (currentFrame.Y >= animationComponent.SpritesheetSize.Y)
                                {
                                    currentFrame.Y = 3;

                                }
                            }
                        }

                        // Insert the new values in the component
                        animationComponent.CurrentFrame = new Point(currentFrame.X, currentFrame.Y);

                        sprite.Scale = 2;

                        sprite.Sprite = animationComponent.Spritesheet;
                        

                        // This will calculate the sourceRectangel
                        // from the spritesheet (which sprite is used).
//                        sprite.SourceRectangle = new Rectangle(
//                            currentFrame.X * frameSize.X, // x-offset into texture
//                            currentFrame.Y * frameSize.Y, // y-offset into texture
//                            frameSize.X, // frame width in pixels
//                            frameSize.Y // frame height in pixels
//                        );
                    }
                }
            }
        }
    }
}