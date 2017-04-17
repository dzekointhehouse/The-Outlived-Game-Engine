﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Components;
using ZEngine.Managers;

namespace Spelkonstruktionsprojekt.ZEngine.Systems
{
    class AnimationSystem : ISystem
    {
        public void RunAnimations(GameTime gameTime)
        {
            var entities = ComponentManager.Instance.GetEntitiesWithComponent<AnimationComponent>();
            foreach (var entity in entities)
            {
                var doneAnimations = new List<int>();
                var usedUniqueAnimationTypes = new List<string>();

                for (var i = 0; i < entity.Value.Animations.Count; i++)
                {
                    var animationWrapper = entity.Value.Animations[i];
                    if(animationWrapper.Unique && usedUniqueAnimationTypes.Contains(animationWrapper.AnimationType)) continue;

                    animationWrapper.Animation.Invoke(gameTime.TotalGameTime.TotalMilliseconds);

                    if(animationWrapper.Unique) usedUniqueAnimationTypes.Add(animationWrapper.AnimationType);
                    if(animationWrapper.IsDone) doneAnimations.Add(i);
                }

                foreach (int animationIndex in doneAnimations)
                {
                    entity.Value.Animations.RemoveAt(animationIndex);
                }
            }
        }
    }
}