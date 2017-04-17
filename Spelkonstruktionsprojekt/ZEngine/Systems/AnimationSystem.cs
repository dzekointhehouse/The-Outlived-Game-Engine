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
            var delta = gameTime.ElapsedGameTime.TotalSeconds;
            var entities = ComponentManager.Instance.GetEntitiesWithComponent<AnimationComponent>();
            foreach (var entity in entities)
            {
                if (entity.Value.ElapsedTime > entity.Value.LenghtInSeconds)
                {
                    ComponentManager.Instance.RemoveComponentFromEntity(typeof(AnimationComponent), entity.Key);
                }
                else
                {
                    entity.Value.Animation.Invoke(entity.Value.ElapsedTime);
                    entity.Value.ElapsedTime += delta;
                }

            }
        }
    }
}
