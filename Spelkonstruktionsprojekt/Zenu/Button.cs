using Microsoft.Xna.Framework;
using ZEngine.Components;
using ZEngine.Managers;

namespace Spelkonstruktionsprojekt.Zenu
{
    public class Button
    {
        public Button(string text, Rectangle rectangle)
        {
            var entityId = EntityManager.GetEntityManager().NewEntity();
            var renderComponent = new RenderComponentBuilder()
                .Position(rectangle.X, rectangle.Y, 800)
                .Dimensions(rectangle.Width, rectangle.Height)
                .Build();
            var spriteComponent = new SpriteComponent()
            {
                SpriteName = "dot"
            };
            ComponentManager.Instance.AddComponentToEntity(renderComponent, entityId);
            ComponentManager.Instance.AddComponentToEntity(spriteComponent, entityId);
        }
    }
}