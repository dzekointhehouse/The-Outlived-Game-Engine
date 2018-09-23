using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using ZEngine.Components;
using ZEngine.Managers;

namespace Spelkonstruktionsprojekt.ZEngine.Systems
{
    public class TextSystem : ISystem, IDrawables
    {
        public bool Enabled { get; set; } = true;
        public int DrawOrder { get; set; }
        public void Draw()
        {
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            foreach (var entity in ComponentManager.Instance.GetEntitiesWithComponent(typeof(TextComponent)))
            {
                var textComponent = entity.Value as TextComponent;
                if (!textComponent.LoadedFont) continue;
                var positionComponent =
                    ComponentManager.Instance.GetEntityComponentOrDefault<PositionComponent>(entity.Key);
                if (positionComponent == null) continue;
                spriteBatch.DrawString(textComponent.SpriteFont, textComponent.Text, positionComponent.Position, Color.White);
            }
            spriteBatch.End();
        }
    }
}