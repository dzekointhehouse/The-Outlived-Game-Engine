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

namespace ZEngine.Systems
{
    class LoadContentSystem : ISystem
    {
        private EventBus.EventBus EventBus = ZEngine.EventBus.EventBus.Instance;

        private readonly Action<ContentManager> _systemAction;

        public LoadContentSystem()
        {
            _systemAction = new Action<ContentManager>(LoadContent);
        }

        public ISystem Start()
        {
            EventBus.Subscribe<ContentManager>("LoadContent", _systemAction);
            return this;
        }

        public ISystem Stop()
        {
            EventBus.Unsubscribe<ContentManager>("LoadContent", _systemAction);
            return this;
        }

        public void LoadContent(ContentManager contentManager)
        {
            var entities = ComponentManager.Instance
                .GetEntitiesWithComponent<SpriteComponent>()
                .Where(entity => !entity.Value.SpriteIsLoaded);

            foreach (var entity in entities)
            {
                entity.Value.Sprite = contentManager.Load<Texture2D>(entity.Value.SpriteName);
                entity.Value.SpriteIsLoaded = true;
                entity.Value.Width = entity.Value.Sprite.Width;
                entity.Value.Height = entity.Value.Sprite.Height;
            }
        }
    }
}
