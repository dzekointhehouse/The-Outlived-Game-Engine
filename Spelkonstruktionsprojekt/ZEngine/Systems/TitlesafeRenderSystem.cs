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
    // This system is used for rendering the HUD, those entities
    // that have instances ofcomponents like health or ammo will 
    // be able to se the component data on the screen when playing.
    // This can be used to that the player always can see the status
    // of his player. (everything drawn here is and should be titlesafe) 
    class TitlesafeRenderSystem : ISystem
    {
        public static string SystemName = "TitlesafeRender";
        private GameDependencies _gameDependencies;

        // This draw method is used to start the system process.
        // it uses DrawTitleSafe to draw the components.
        public void Draw(GameDependencies gameDependencies)
        {
            this._gameDependencies = gameDependencies;

            _gameDependencies.SpriteBatch.Begin(SpriteSortMode.FrontToBack);
            DrawTitleSafe();
            _gameDependencies.SpriteBatch.End();
        }


        // DrwaTitleSafe gets all the components and draws them in a
        // correct format, so the data will sortet so that each entity will
        // have it's own row, and each component will be sorted by column.
        private void DrawTitleSafe()
        {
            var graphics = _gameDependencies.GraphicsDeviceManager.GraphicsDevice;
            var titlesafearea = graphics.Viewport.TitleSafeArea;

            var playerComponents = ComponentManager.Instance.GetEntitiesWithComponent<PlayerComponent>();
            //var healthComponents = ComponentManager.Instance.GetEntitiesWithComponent<HealthComponent>();
            //var renderable = ComponentManager.Instance.GetEntitiesWithComponent<AmmoComponent>();

            // We save the previous text height so we can stack
            // them on top of eachother.
            var previousHeight = 0f;

            var g = _gameDependencies.GameContent as ContentManager;
            // Need to let the user decide
            var spriteFont = g.Load<SpriteFont>("Healthfont");

            foreach (var playerInstance in playerComponents)
            {
                //var g = _gameDependencies.GameContent as ContentManager;
                //var spriteFont = g.Load<SpriteFont>("Healthfont");

                //string text = instance.Value.CurrentHealth + " | " + instance.Value.MaxHealth;

                //var textHeight = spriteFont.MeasureString(text).Y;

                //var xPosition = titlesafearea.Width - spriteFont.MeasureString(text).X - 10;
                //var yPosition = titlesafearea.Height - textHeight - previousHeight;
                //previousHeight = textHeight;

                //var position = new Vector2(xPosition, yPosition);
                //_gameDependencies.SpriteBatch.DrawString(spriteFont, text, position, Color.White);

                string text = playerInstance.Value.Name;

                if (ComponentManager.Instance.EntityHasComponent<HealthComponent>(playerInstance.Key))
                {
                    var health = ComponentManager.Instance.GetEntityComponent<HealthComponent>(playerInstance.Key);
                    text = text + ": " + health.CurrentHealth + "|" + health.MaxHealth;
                }
                if (ComponentManager.Instance.EntityHasComponent<AmmoComponent>(playerInstance.Key))
                {
                    var ammo = ComponentManager.Instance.GetEntityComponent<AmmoComponent>(playerInstance.Key);
                    text = text + " Ammo: " + ammo.Amount;
                }

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
