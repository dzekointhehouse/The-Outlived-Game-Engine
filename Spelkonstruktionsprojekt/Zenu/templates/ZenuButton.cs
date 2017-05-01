using System.Runtime.Serialization.Formatters;
using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Components.RenderComponent;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using ZEngine.Components;
using ZEngine.Managers;

namespace Spelkonstruktionsprojekt.Zenu.templates
{
    public static class ZenuButton
    {
        public static void RenderComponent(int entityId)
        {
            var renderComponent = new RenderComponent
            {
                DimensionsComponent = new DimensionsComponent
                {
                    Width = 300,
                    Height = 80
                },
                Fixed = true,
                IsVisible = true
            };
            ComponentManager.Instance.AddComponentToEntity(renderComponent, entityId);
        }

        public static void PositionComponent(int entityId, float x, float y)
        {
            var positionComponent = new PositionComponent
            {
                Position = new Vector2(x, y),
                ZIndex = 100
            };
            ComponentManager.Instance.AddComponentToEntity(positionComponent, entityId);
        }

        public static void SpriteComponent(int entityId)
        {
            var spriteComponent = new SpriteComponent
            {
                SpriteName = "dot"
            };
            ComponentManager.Instance.AddComponentToEntity(spriteComponent, entityId);
        }

        public static int Template()
        {
            var entityId = EntityManager.GetEntityManager().NewEntity();
            RenderComponent(entityId);
            PositionComponent(entityId, 0, 0);
            SpriteComponent(entityId);
            return entityId;
        }
    }

    public class ButtonBuilder
    {
        private readonly int _id = EntityManager.GetEntityManager().NewEntity();
        private PositionComponent _positionComponent;
        private RenderComponent _renderComponent;
        private RenderOffsetComponent _offsetComponent;
        private SpriteComponent _sprite;

        public int Build()
        {
            if (_positionComponent != null) ComponentManager.Instance.AddComponentToEntity(_positionComponent, _id);
            if (_renderComponent != null) ComponentManager.Instance.AddComponentToEntity(_renderComponent, _id);
            if (_offsetComponent != null) ComponentManager.Instance.AddComponentToEntity(_offsetComponent, _id);
            if (_sprite != null) ComponentManager.Instance.AddComponentToEntity(_sprite, _id);

            return _id;
        }

        public ButtonBuilder Position(float? x = null, float? y = null, int? z = null)
        {
            if (x == null) x = _positionComponent.Position.X;
            if (y == null) y = _positionComponent.Position.Y;
            if (z == null) z = _positionComponent.ZIndex;

            _positionComponent = new PositionComponent
            {
                Position = new Vector2(x.Value, y.Value),
                ZIndex = z.Value
            };

            return this;
        }

        public ButtonBuilder Dimensions(int width, int height)
        {
            _renderComponent = new RenderComponent
            {
                DimensionsComponent = new DimensionsComponent
                {
                    Width = 300,
                    Height = 80
                },
                Fixed = true,
                IsVisible = true
            };

            return this;
        }

        public ButtonBuilder Offset(float x, float y)
        {
            _offsetComponent = new RenderOffsetComponent
            {
                Offset = new Vector2(x, y)
            };

            return this;
        }

        public ButtonBuilder Sprite(string spriteName)
        {
            _sprite = new SpriteComponent
            {
                SpriteName = spriteName
            };

            return this;
        }

        public ButtonBuilder FromTemplate(int templateId)
        {
            var spriteComponent = ComponentManager.Instance.GetEntityComponentOrDefault<SpriteComponent>(templateId);
            if (spriteComponent != null) Sprite(spriteComponent.SpriteName);

            var renderComponent = ComponentManager.Instance.GetEntityComponentOrDefault<RenderComponent>(templateId);
            if (renderComponent != null)
                Dimensions(renderComponent.DimensionsComponent.Width, renderComponent.DimensionsComponent.Height);

            var positionComponent =
                ComponentManager.Instance.GetEntityComponentOrDefault<PositionComponent>(templateId);
            if (positionComponent != null)
                Position(positionComponent.Position.X, positionComponent.Position.Y, positionComponent.ZIndex);

            var offsetComponent =
                ComponentManager.Instance.GetEntityComponentOrDefault<RenderOffsetComponent>(templateId);
            if (offsetComponent != null) Offset(offsetComponent.Offset.X, offsetComponent.Offset.Y);

            return this;
        }
    }
}