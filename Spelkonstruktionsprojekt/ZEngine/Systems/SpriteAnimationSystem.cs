using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Spelkonstruktionsprojekt.ZEngine.Components;
using ZEngine.Components;
using ZEngine.Managers;

namespace Spelkonstruktionsprojekt.ZEngine.Systems
{
    class SpriteAnimationSystem : ISystem
    {
        // This method is called when using this system. 
        // This will activate an animation for all the
        // entities that have an SpriteAnimationComponent.
        public void Update(GameTime gameTime)
        {
            DeathAnimation(gameTime);
        }

        // DeathAnimation as it states in the method name is used for entities that
        // have a sprite, sprite animation and a health component instance. This is 
        // intented to be used so that the entities show a splash of blood when they die.
        private void DeathAnimation(GameTime gameTime)
        {
            var animationComponents = ComponentManager.Instance.GetEntitiesWithComponent<SpriteAnimationComponent>();

            foreach (var animation in animationComponents)
            {
                var currentFrame = animation.Value.CurrentFrame;
                var sheetSize = animation.Value.SpritesheetSize;
                var frameSize = animation.Value.FrameSize;

                if (frameSize == default(Point))
                {
                    animation.Value.FrameSize = new Point(
                        animation.Value.Spritesheet.Width / animation.Value.SpritesheetSize.X,
                        animation.Value.Spritesheet.Height / animation.Value.SpritesheetSize.Y
                    );

                    frameSize = animation.Value.FrameSize;
                }

                // We get the health component to check if the entity is alive, and
                // the sprite component so we can set the new texture to render upon death.
                //if (ComponentManager.Instance.EntityHasComponent(typeof(HealthComponent), animation.Key))
                //    continue;

                var health = ComponentManager.Instance.GetEntityComponentOrDefault<HealthComponent>(animation.Key);
                var sprite = ComponentManager.Instance.GetEntityComponentOrDefault<SpriteComponent>(animation.Key);

                if (currentFrame.X != (sheetSize.X - 1) && currentFrame.Y != sheetSize.Y)
                {
                    if (!health.Alive)
                    {
                        animation.Value.TimeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;

                        if (animation.Value.TimeSinceLastFrame > animation.Value.MillisecondsPerFrame)
                        {
                            animation.Value.TimeSinceLastFrame -= animation.Value.MillisecondsPerFrame;
                            ++currentFrame.X;
                        
                            if (currentFrame.X >= animation.Value.SpritesheetSize.X)
                            {
                                currentFrame.X = 0;
                                ++currentFrame.Y;

                                if (currentFrame.Y >= animation.Value.SpritesheetSize.Y)
                                {
                                    currentFrame.Y = 3;

                                }
                            }
                        }

                        // Insert the new values in the component
                        animation.Value.CurrentFrame = new Point(currentFrame.X, currentFrame.Y);

                        sprite.Scale = 2;

                        sprite.Sprite = animation.Value.Spritesheet;
                        

                        // This will calculate the sourceRectangel
                        // from the spritesheet (which sprite is used).
                        sprite.SourceRectangle = new Rectangle(
                            currentFrame.X * frameSize.X, // x-offset into texture
                            currentFrame.Y * frameSize.Y, // y-offset into texture
                            frameSize.X, // frame width in pixels
                            frameSize.Y // frame height in pixels
                        );
                    }
                }
            }
        }
    }
}
