using System;
using System.IO;
using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Constants;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using Spelkonstruktionsprojekt.ZEngine.Systems.InputHandler;
using ZEngine.Components;
using ZEngine.EventBus;
using ZEngine.Managers;

namespace Spelkonstruktionsprojekt.ZEngine.Systems
{
    public class KillSwitchSystem : ISystem, IUpdateables
    {
        public bool Enabled { get; set; } = true;
        public int UpdateOrder { get; set; }

        public void Update(GameTime gt)
        {
        }

        private EventBus EventBus = EventBus.Instance;
        private ComponentManager ComponentManager = ComponentManager.Instance;

        public KillSwitchSystem Start()
        {
            EventBus.Subscribe<KillSwitchEvent>(EventConstants.KillLights, KillLights);
            return this;
        }

        public KillSwitchSystem Stop()
        {
            EventBus.Unsubscribe<KillSwitchEvent>(EventConstants.KillLights, KillLights);
            return this;
        }

        public void KillLights(KillSwitchEvent killSwitchEvent)
        {
            foreach (var entity in ComponentManager.GetEntitiesWithComponent(typeof(LightComponent)))
            {
                var lightComponent = entity.Value as LightComponent;

                //Meet Type requirements
                //Could be that it is a flashlight or only a street light etc.
                var hasRequiredComponents = true;
                if (killSwitchEvent.LightTypes != null)
                {
                    for (var i = 0; i < killSwitchEvent.LightTypes.Length; i++)
                    {
                        if (!ComponentManager.EntityHasComponent(killSwitchEvent.LightTypes[i], entity.Key))
                        {
                            hasRequiredComponents = false;
                            break;
                        }
                    }
                    if (!hasRequiredComponents) continue;
                }

                //Used to only kill lights within in radius of player
                var withinRadius = true;
                if (killSwitchEvent.Radius > 0)
                {
                    var sourcePosition =
                        ComponentManager.GetEntityComponentOrDefault<PositionComponent>(killSwitchEvent.Source);
                    if (sourcePosition == null)
                    {
                        if (Vector2.Distance(sourcePosition.Position, lightComponent.Light.Position) >
                            killSwitchEvent.Radius)
                        {
                            withinRadius = false;
                        }
                    }
                }
                if(!withinRadius) continue;

                if (lightComponent.KillSwitchOn)
                {
                    lightComponent.KillSwitchOn = false;
                    lightComponent.Light.Enabled = true;
                    continue;
                }
                
                lightComponent.KillSwitchOn = true;
                lightComponent.Light.Enabled = false;
                
                //Automatically turn on the light again if an OffTime is specified
                if (killSwitchEvent.OffTime > 0)
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
                        StartOfAnimation = killSwitchEvent.EventTime,
                        Length = killSwitchEvent.OffTime,
                        Unique = true
                    };
                    NewKillLightAnimation(animation, lightComponent);
                    animationComponent.Animations.Add(animation);   
                }
            }
        }
        public void NewKillLightAnimation(GeneralAnimation generalAnimation, LightComponent lightComponent)
        {
            generalAnimation.Animation = delegate(double currentTimeInMilliseconds)
            {
                if (currentTimeInMilliseconds - generalAnimation.StartOfAnimation > generalAnimation.Length)
                {
                    lightComponent.KillSwitchOn = false;
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

    public class KillSwitchEvent
    {
        public double EventTime { get; set; }
        public Type[] LightTypes { get; set; } = null; //null as kill all light types
        public double Radius { get; set; } = 0; //0 as select all
        public uint Source { get; set; } //If has Radius, this is the entity from whose position the radius apply
        public double OffTime { get; set; } = 0; //0 as once killed dont automatically turn on again
    }
}