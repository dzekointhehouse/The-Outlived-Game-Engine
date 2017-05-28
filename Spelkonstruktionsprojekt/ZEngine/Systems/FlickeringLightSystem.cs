using System;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using Spelkonstruktionsprojekt.ZEngine.Systems.InputHandler;
using ZEngine.Helpers;
using ZEngine.Managers;

namespace Spelkonstruktionsprojekt.ZEngine.Systems
{
    public class FlickeringLightSystem : ISystem
    {
        public void Update(GameTime gameTime)
        {
            foreach (var entity in ComponentManager.Instance.GetEntitiesWithComponent(typeof(FlickeringLight)))
            {
                var flickerComponent = entity.Value as FlickeringLight;
                var lightComponent = ComponentManager.Instance.GetEntityComponentOrDefault<LightComponent>(entity.Key);
                if (lightComponent == null) continue;

                var rand = new Random().NextDouble();
                var chance = flickerComponent.FlickerChance * gameTime.ElapsedGameTime.TotalSeconds;
                if (rand < chance)
                {
                    var animationComponent = GetOrCreateDefault(entity.Key);

                    var animationAlreadyRunning = false;
                    for (var index = 0; index < animationComponent.Animations.Count; index++)
                    {
                        var ongoingAnimation = animationComponent.Animations[index];
                        if (ongoingAnimation.AnimationType == "Flicker")
                        {
                            animationAlreadyRunning = true;
                            break;
                        }
                    }
                    if (animationAlreadyRunning) continue;

                    var animation = new GeneralAnimation()
                    {
                        AnimationType = "Flicker",
                        StartOfAnimation = gameTime.TotalGameTime.TotalMilliseconds,
                        Length = flickerComponent.FlickerSpeed,
                        Unique = true
                    };
                    var shouldDim = rand < chance * 0.5;
                    NewFlickerAnimation(animation, lightComponent, shouldDim);
                    animationComponent.Animations.Add(animation);
                }
            }
        }

        public void NewFlickerAnimation(GeneralAnimation generalAnimation, LightComponent lightComponent,
            bool shouldDimNotKill)
        {
            var previousIntensity = lightComponent.Light.Intensity;
            if (shouldDimNotKill)
            {
                previousIntensity = lightComponent.Light.Intensity;
                lightComponent.Light.Intensity = (float) (lightComponent.Light.Intensity * 0.1);
            }
            else
            {
                lightComponent.Light.Enabled = false;
            }
            generalAnimation.Animation = delegate(double currentTimeInMilliseconds)
            {
                if (currentTimeInMilliseconds - generalAnimation.StartOfAnimation > generalAnimation.Length)
                {
                         lightComponent.Light.Intensity = previousIntensity;
                    lightComponent.Light.Enabled = true;
                    generalAnimation.IsDone = true;
                }
            };
        }

        private AnimationComponent GetOrCreateDefault(uint entityId)
        {
            var animationComponent =
                ComponentManager.Instance.GetEntityComponentOrDefault<AnimationComponent>(entityId);
            if (animationComponent != null) return animationComponent;

            animationComponent = ComponentManager.Instance.ComponentFactory.NewComponent<AnimationComponent>();
            ComponentManager.Instance.AddComponentToEntity(animationComponent, entityId);
            return animationComponent;
        }
    }
}