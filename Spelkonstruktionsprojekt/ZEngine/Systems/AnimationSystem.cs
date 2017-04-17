using System;
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
                foreach (var animation in entity.Value.Animations)
                {
                    animation.Invoke(gameTime.TotalGameTime.Milliseconds);
                }
            }
        }
    }
}
