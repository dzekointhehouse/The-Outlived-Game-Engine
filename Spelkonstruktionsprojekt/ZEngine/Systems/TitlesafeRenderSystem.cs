using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Spelkonstruktionsprojekt.ZEngine.Components;
using ZEngine.Managers;
using ZEngine.Wrappers;

namespace Spelkonstruktionsprojekt.ZEngine.Systems
{
    class TitlesafeRenderSystem : ISystem
    {
        public static string SystemName = "TitlesafeRender";
        private ComponentManager _componentManager = ComponentManager.Instance;
        private GameDependencies _gameDependencies;


        public void Draw(GameDependencies gameDependencies)
        {
            this._gameDependencies = gameDependencies;

            _gameDependencies.SpriteBatch.Begin(SpriteSortMode.FrontToBack);
            DrawTitleSafe();
            _gameDependencies.SpriteBatch.End();
        }

        private void DrawTitleSafe()
        {
            var graphics = _gameDependencies.GraphicsDeviceManager.GraphicsDevice;
            var titlesafearea = graphics.Viewport.TitleSafeArea;

            // Loading necessary components
            var renderable = _componentManager.GetEntitiesWithComponent<HealthComponent>();

            // We save the previous text height so we can stack
            // them on top of eachother.
            var previousHeight = 0f;

            foreach (var instance in renderable)
            {
                var g = _gameDependencies.GameContent as ContentManager;
                var spriteFont = g.Load<SpriteFont>("Healthfont");

                string text = instance.Value.CurrentHealth + " / " + instance.Value.MaxHealth;

                var textHeight = spriteFont.MeasureString(text).Y;

                var xPosition = titlesafearea.Width - spriteFont.MeasureString(text).X - 10;
                var yPosition = titlesafearea.Height - textHeight - previousHeight;
                previousHeight = textHeight;

                var position = new Vector2(xPosition, yPosition);
                _gameDependencies.SpriteBatch.DrawString(spriteFont, text, position, Color.White);
            }
        }
    }
}
