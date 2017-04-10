using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
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
        private EventBus.EventBus EventBus = ZEngine.EventBus.EventBus.Instance;

        private readonly Action<ContentManager> _systemAction;

        public LoadContentSystem()
        {
            _systemAction = LoadContent;
        }


        public void LoadContent(ContentManager contentManager)
        {
            var entities2 = ComponentManager.Instance
                .GetEntitiesWithComponent<SpriteComponent>();

            var entities = entities2
                .Where(entity => !entity.Value.SpriteIsLoaded);

            foreach (var entity in entities)
            {
                entity.Value.Sprite = contentManager.Load<Texture2D>(entity.Value.SpriteName);
                entity.Value.SpriteIsLoaded = true;
                entity.Value.Width = entity.Value.Sprite.Width;
                entity.Value.Height = entity.Value.Sprite.Height;
            }
        }

        public void StartSystem(GameDependencies gd)
        {
            LoadContent(gd.GameContent as ContentManager);
        }
    }
}
