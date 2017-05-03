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
using ZEngine.Components;

namespace Spelkonstruktionsprojekt.ZEngine.Systems
{
    class AnimationSystem : ISystem
    {
        // FPS higher if kept outside.
        private List<int> doneAnimations = new List<int>(50);
        private List<string> usedUniqueAnimationTypes = new List<string>(50);

        // Will be updatin the animations that unfortunately
        // other systems use, which makes them dependent on this
        // system. But that's a design choice that we live by.
        public void UpdateAnimations(GameTime gameTime)
        {
            Dictionary<int, IComponent> animations = ComponentManager.Instance.GetEntitiesWithComponent(typeof(AnimationComponent));
            foreach (var entity in animations)
            {
                doneAnimations.Clear();
                usedUniqueAnimationTypes.Clear();

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
