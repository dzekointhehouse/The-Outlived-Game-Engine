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
        private ComponentManager ComponentManager = ComponentManager.Instance;
        private RenderDependencies gm;
        


        public void Render(RenderDependencies gm)
        {
            this.gm = gm;
            var graphics = gm.GraphicsDeviceManager.GraphicsDevice;
            var spriteBatch = gm.SpriteBatch;
            spriteBatch.Begin(SpriteSortMode.FrontToBack);
            DrawAll(spriteBatch,graphics);
            spriteBatch.End();
        }

        private void DrawAll(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            var renderable = ComponentManager.GetEntitiesWithComponent<HealthComponent>();
            var titlesafearea = graphics.Viewport.TitleSafeArea;

            foreach (var instance in renderable)
            {
                //instance.Value.CurrentHealth + "/" + instance.Value.MaxHealth;
                var g = gm.GameContent as ContentManager;
                var spriteFont = g.Load<SpriteFont>("Healthfont");
                string text = instance.Value.CurrentHealth + " / " + instance.Value.MaxHealth;
               
                var position = new Vector2(titlesafearea.Width - spriteFont.MeasureString(text).X, titlesafearea.Height - spriteFont.MeasureString(text).Y);
                spriteBatch.DrawString(spriteFont, text, position, Color.White);
            }
        }
    }
}
