using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using ZEngine.Managers;
using Spelkonstruktionsprojekt.ZEngine.Systems.InputHandler;

namespace Spelkonstruktionsprojekt.ZEngine.Systems
{
    class AnimationSystem : ISystem
    {
        public void RunAnimations(GameTime gameTime)
        {
            var animations = ComponentManager.Instance.GetEntitiesWithComponent(typeof(AnimationComponent));
            foreach (var entity in animations)
            {
                var doneAnimations = new List<int>();
                var usedUniqueAnimationTypes = new List<string>();

                var animationComponent = entity.Value as AnimationComponent;
                for (var i = 0; i < animationComponent.Animations.Count; i++)
                {
                    var animationWrapper = animationComponent.Animations[i];
                    if (animationWrapper.Unique && usedUniqueAnimationTypes.Contains(animationWrapper.AnimationType))
                    {
                        doneAnimations.Add(i);
                    }

                    animationWrapper.Animation.Invoke(gameTime.TotalGameTime.TotalMilliseconds);

                    if(animationWrapper.Unique) usedUniqueAnimationTypes.Add(animationWrapper.AnimationType);
                    if(animationWrapper.IsDone) doneAnimations.Add(i);
                }

                foreach (int animationIndex in doneAnimations)
                {
                    if (animationComponent.Animations.ElementAtOrDefault(animationIndex) != null)
                        animationComponent.Animations.RemoveAt(animationIndex);
                }
            }
        }
    }
}
