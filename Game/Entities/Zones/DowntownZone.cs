using System;
using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Constants;
using Spelkonstruktionsprojekt.ZEngine.Helpers;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using Spelkonstruktionsprojekt.ZEngine.Systems.Bullets;
using Spelkonstruktionsprojekt.ZEngine.Systems.Collisions;
using Spelkonstruktionsprojekt.ZEngine.Systems.InputHandler;
using ZEngine.EventBus;
using ZEngine.Managers;
using Game = Microsoft.Xna.Framework.Game;

namespace Game.Entities.Zones
{
    public class DowntownZone
    {
        public const string Event = "DowntownZone_DisplayLabel";
        private ComponentManager ComponentManager = ComponentManager.Instance;

        private bool IsDisplaying = false;

        public void Start()
        {
            EventBus.Instance.Subscribe<EventZoneSystem.ZoneEvent>(Event, DisplayLabel);
        }

        public void Stop()
        {
            EventBus.Instance.Unsubscribe<EventZoneSystem.ZoneEvent>(Event, DisplayLabel);
        }

        public void DisplayLabel(EventZoneSystem.ZoneEvent zoneEvent)
        {
            if (IsDisplaying) return;
            IsDisplaying = true;

            var entityId = new EntityBuilder()
                .SetPosition(new Vector2(200, 50), ZIndexConstants.InGameText)
                .BuildAndReturnId();

            var textComponent = new TextComponent();
            textComponent.SpriteFontName = "ZEone";
            textComponent.SpriteFont = OutlivedGame.Instance().Fonts["ZEone"];
            textComponent.LoadedFont = true;
            textComponent.Text = "Downtown";
            ComponentManager.AddComponentToEntity(textComponent, entityId);

            var animationComponent = ComponentManager.ComponentFactory.NewComponent<AnimationComponent>();
            ComponentManager.AddComponentToEntity(animationComponent, entityId);

            var animation = new GeneralAnimation()
            {
                AnimationType = "InGameText",
                StartOfAnimation = zoneEvent.EventTime,
                Length = 8000,
                Unique = true
            };
            NewBulletAnimation(animation, entityId);
            animationComponent.Animations.Add(animation);
        }

        // Animation for when the bullet should be deleted.
        public void NewBulletAnimation(GeneralAnimation generalAnimation, uint entityId)
        {
            var startSet = false;
            generalAnimation.Animation = delegate(double currentTimeInMilliseconds)
            {
                if (!startSet)
                {
                    generalAnimation.StartOfAnimation = currentTimeInMilliseconds;
                    startSet = true;
                }

                if (currentTimeInMilliseconds - generalAnimation.StartOfAnimation > generalAnimation.Length)
                {
                    EntityManager.AddEntityToDestructionList(entityId);
                    generalAnimation.IsDone = true;
                    IsDisplaying = false;
                }
            };
        }
    }
}