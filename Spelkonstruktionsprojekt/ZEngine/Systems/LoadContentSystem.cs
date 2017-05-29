using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using ZEngine.Components;
using ZEngine.Managers;
using ZEngine.EventBus;
using ZEngine.Wrappers;

namespace ZEngine.Systems
{
    public class LoadContentSystem : ISystem
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
                if (spriteComponent.TileWidth == 0) spriteComponent.TileWidth = spriteComponent.Sprite.Width;
                if (spriteComponent.TileHeight == 0) spriteComponent.TileHeight = spriteComponent.Sprite.Height;
            }

            var soundEntities = ComponentManager.Instance.GetEntitiesWithComponent(typeof(SoundComponent));
            foreach (var entity in soundEntities)
            {
                SoundComponent soundComponent = (SoundComponent) entity.Value;
                if(soundComponent.SoundEffectName != null)
                    soundComponent.SoundEffect = contentManager.Load<SoundEffect>(soundComponent.SoundEffectName);
            }

            foreach (var entity in ComponentManager.Instance.GetEntitiesWithComponent(typeof(TextComponent)))
            {
                var textComponent = entity.Value as TextComponent;
                textComponent.SpriteFont = contentManager.Load<SpriteFont>(textComponent.SpriteFontName);
                textComponent.LoadedFont = true;
            }
        }
    }
}