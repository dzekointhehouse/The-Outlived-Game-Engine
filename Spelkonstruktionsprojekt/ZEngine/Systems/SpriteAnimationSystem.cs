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
        public void Update(GameTime gameTime)
        {
            DeathAnimation(gameTime);
        }

        private void DeathAnimation(GameTime gameTime)
        {
           var animationComponents = ComponentManager.Instance.GetEntitiesWithComponent<SpriteAnimationComponent>();

            foreach (var animation in animationComponents)
            {
                var valueCurrentFrame = animation.Value.CurrentFrame;
                var frameSize = animation.Value.FrameSize;

                frameSize = new Point(
                    animation.Value.Spritesheet.Width / animation.Value.SpritesheetSize.X,
                    animation.Value.Spritesheet.Height / animation.Value.SpritesheetSize.Y
                    );

                var health = ComponentManager.Instance.GetEntityComponentOrDefault<HealthComponent>(animation.Key);
                var sprite = ComponentManager.Instance.GetEntityComponentOrDefault<SpriteComponent>(animation.Key);

               // if (!health.Alive && animation.Value.CurrentFrame != animation.Value.SpritesheetSize)
                    if (!health.Alive)
                {
                    animation.Value.TimeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;

                    if (animation.Value.TimeSinceLastFrame > animation.Value.MillisecondsPerFrame)
                    {
                        animation.Value.TimeSinceLastFrame -= animation.Value.MillisecondsPerFrame;

                        ++valueCurrentFrame.X;

                        if (animation.Value.CurrentFrame.X >= animation.Value.SpritesheetSize.X)
                        {
                            var currentFrame = animation.Value.CurrentFrame;
                            currentFrame.X = 0;
                            ++valueCurrentFrame.Y;

                            if (valueCurrentFrame.Y >= animation.Value.SpritesheetSize.Y)
                            {
                                valueCurrentFrame.Y = 0;
                            }
                        }
                    }

                    sprite.Sprite = animation.Value.Spritesheet;
                    // Calculate the current animation frame
                    sprite.SourceRectangle = new Rectangle(
                        valueCurrentFrame.X * frameSize.X, // x-offset into texture
                        valueCurrentFrame.Y * frameSize.Y, // y-offset into texture
                        frameSize.X, // frame width in pixels
                        frameSize.Y // frame height in pixels
                    );
                }
            }
        }
    }
}
