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

                        animation.Value.CurrentFrame = new Point(currentFrame.X, currentFrame.Y);
                        sprite.Scale = 2;

                        sprite.Sprite = animation.Value.Spritesheet;
                        // Calculate the current animation frame
                        sprite.SourceRectangle = new Rectangle(
                            currentFrame.X * frameSize.X, // x-offset into texture
                            currentFrame.Y * frameSize.Y, // y-offset into texture
                            frameSize.X, // frame width in pixels
                            frameSize.Y // frame height in pixels
                        );
                        //if (!health.Alive && animation.Value.CurrentFrame == animation.Value.SpritesheetSize)
                        //    ComponentManager.Instance.RemoveComponentFromEntity(typeof(SpriteAnimationComponent), animation.Key);
                    }
                }
            }
        }
    }
}
