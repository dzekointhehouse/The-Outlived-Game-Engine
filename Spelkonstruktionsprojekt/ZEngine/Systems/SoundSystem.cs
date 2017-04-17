using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Systems.InputHandler;
using ZEngine.Components;
using ZEngine.EventBus;
using ZEngine.Managers;
using ZEngine.Systems;

namespace Spelkonstruktionsprojekt.ZEngine.Systems
{
    public class SoundSystem : ISystem
    {
        private EventBus EventBus = EventBus.Instance;
        private ComponentManager ComponentManager = ComponentManager.Instance;

        public void Start()
        {
            // Here we subscribe what will happen when the entity walks forwards
            // We'll use WalkingSounds to handle it.
            EventBus.Subscribe<MoveEvent>("entityWalkForwards", WalkingSounds);
            EventBus.Subscribe<MoveEvent>("entityWalkBackwards", WalkingSounds);
            EventBus.Subscribe<InputEvent>("entityFireWeapon", WeaponSounds);
        }

        private void WeaponSounds(InputEvent obj)
        {
            var sound = ComponentManager.Instance.GetEntityComponentOrDefault<SoundComponent>(obj.EntityId);

            if (ComponentManager.Instance.EntityHasComponent<BulletComponent>(obj.EntityId))
                NewBulletSoundAnimation(sound);
        }


        public Action<double> NewBulletSoundAnimation(SoundComponent soundComponent)
        {
            SoundEffectInstance sound = soundComponent.SoundInstace;
            return delegate(double elapsedTime)
            {
                if (sound.State == SoundState.Playing) return;

                sound.Play();
            };
        }

        public void WalkingSounds(MoveEvent moveEvent)
        {
        }

        public Action<double> NewWalkingSoundAnimation(SoundComponent soundComponent, bool isLooped)
        {
            SoundEffectInstance sound = soundComponent.SoundInstace;
            return delegate(double elapsedTime)
            {
                if (sound.State == SoundState.Playing) return;
                if (isLooped) sound.IsLooped = true;

                sound.Play();
            };
        }
    }
}

//private void HandleTurnAroundEvent(InputEvent moveEvent)
//{
//if (moveEvent.KeyEvent != ActionBindings.KeyEvent.KeyPressed) return;

//var entityId = moveEvent.EntityId;
//if (ComponentManager.EntityHasComponent<MoveComponent>(entityId))
//{
//var animationComponent = ComponentManager.GetEntityComponentOrDefault<AnimationComponent>(entityId);
//    if (animationComponent == null)
//{
//    animationComponent = new AnimationComponent();
//    ComponentManager.AddComponentToEntity(animationComponent, entityId);
//}

//var animation = new GeneralAnimation()
//{
//    AnimationType = TurnAroundEventName,
//    StartOfAnimation = moveEvent.EventTime,
//    Unique = true,
//    Length = 220
//};
//var animationAction = NewTurningAnimation(entityId, animation);
//animation.Animation = animationAction;

//animationComponent.Animations.Add(animation);
//}
//}

//public Action<double> NewTurningAnimation(int entityId, GeneralAnimation generalAnimation)
//{
//var moveComponent = ComponentManager.GetEntityComponentOrDefault<MoveComponent>(entityId);
//if (moveComponent == null) return null;

//double start = moveComponent.Direction;
//double target = (start + Math.PI);
//return delegate (double currentTime)
//{
//var elapsedTime = currentTime - generalAnimation.StartOfAnimation;
//if (elapsedTime > generalAnimation.Length) generalAnimation.IsDone = true;

////Algorithm for turning stepwise on each iteration
////Modulus is for when the direction makes a whole turn
//moveComponent.Direction = (start + (target - start) / generalAnimation.Length * elapsedTime) % MathHelper.TwoPi;
//};
//}
