using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ZEngine.Components;
using ZEngine.Managers;
using ZEngine.EventBus;
using ZEngine.Wrappers;

namespace ZEngine.Systems
{
    class LoadContentSystem : ISystem
    {
        public void LoadContent(ContentManager contentManager)
        {
            var entities = ComponentManager.Instance
                .GetEntitiesWithComponent(typeof(SpriteComponent))
                .Where(entity => !(entity.Value as SpriteComponent).SpriteIsLoaded);

            foreach (var entity in entities)
            {
                var spriteComponent = entity.Value as SpriteComponent;
                if (string.IsNullOrEmpty(spriteComponent.SpriteName)) continue;
                spriteComponent.Sprite = contentManager.Load<Texture2D>(spriteComponent.SpriteName);
                spriteComponent.SpriteIsLoaded = true;
                spriteComponent.Width = spriteComponent.Sprite.Width;
                spriteComponent.Height = spriteComponent.Sprite.Height;
            }

            var soundEntities = ComponentManager.Instance.GetEntitiesWithComponent(typeof(SoundComponent));
            foreach (var entity in soundEntities)
            {
                SoundComponent soundComponent = (SoundComponent)entity.Value;
                soundComponent.SoundEffect = contentManager.Load<SoundEffect>(soundComponent.SoundEffectName);
            }
        }
    }
}
