using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Components.RenderComponent;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using ZEngine.Components;
using ZEngine.Managers;
using ZEngine.Wrappers;

namespace Spelkonstruktionsprojekt.ZEngine.Systems.Rendering
{
    // This system is used for rendering the HUD, those entities
    // that have instances ofcomponents like health or ammo will 
    // be able to se the component data on the screen when playing.
    // This can be used to that the player always can see the status
    // of his player. (everything drawn here is and should be titlesafe) 
    class RenderHUDSystem : ISystem
    {
        public static string SystemName = "TitlesafeRender";
        private GameDependencies _gameDependencies;
        private StringBuilder gameHUD = new StringBuilder(0, 50);

        // This draw method is used to start the system process.
        // it uses DrawTitleSafe to draw the components.
        public void Draw(GameDependencies gameDependencies)
        {
            this._gameDependencies = gameDependencies;

           // _gameDependencies.SpriteBatch.Begin(SpriteSortMode.FrontToBack);
            DrawTitleSafe();
           // _gameDependencies.SpriteBatch.End();
        }


        // DrwaTitleSafe gets all the components and draws them in a
        // correct format, so the data will sortet so that each entity will
        // have it's own row, and each component will be sorted by column.
        private void DrawTitleSafe()
        {
            GraphicsDevice graphics = _gameDependencies.GraphicsDeviceManager.GraphicsDevice;
            Rectangle titlesafearea = graphics.Viewport.TitleSafeArea;

            Dictionary<int, IComponent> HUDComponents = ComponentManager.Instance.GetEntitiesWithComponent(typeof(RenderHUDComponent));

            // We save the previous text height so we can stack
            // them (the text for every player) on top of eachother.
            float previousHeight = 5f;

            ContentManager contentManager = _gameDependencies.GameContent as ContentManager;         
            // Maybe let the user decide?
            SpriteFont spriteFont = contentManager.Load<SpriteFont>("ZEone");

            Vector2 position = Vector2.Zero;

            gameHUD.Clear();

            foreach (var instance in HUDComponents)
            {

                var HUD = instance.Value as RenderHUDComponent;

                gameHUD.AppendLine();

                if (HUD.HUDtext != null)
                {
                    gameHUD.Append(HUD.HUDtext);
                }

                // We execute this if - statement, if showstats is true,
                // which states that we should show the entity's health and
                // ammo if it has it.
                if (HUD.ShowStats)
                {
                    PlayerComponent player = ComponentManager.Instance.GetEntityComponentOrDefault<PlayerComponent>(instance.Key);
                    if (player == null) return;
                    HealthComponent health = ComponentManager.Instance.GetEntityComponentOrDefault<HealthComponent>(instance.Key);
                    if (health == null) return;

                    gameHUD.Append(player.Name);

                    if (health.Alive)
                    {
                        var currentHealth = health.MaxHealth - health.Damage.Sum();
                        gameHUD.Append(":");
                        gameHUD.Append(currentHealth);
                        gameHUD.Append("HP");

                    }
                    else
                    {
                        gameHUD.Append(": Rest in peace");
                    }

                    // adding ammo here the same way.
                    if (ComponentManager.Instance.EntityHasComponent<AmmoComponent>(instance.Key) && health.Alive)
                    {
                        AmmoComponent ammo = ComponentManager.Instance.GetEntityComponentOrDefault<AmmoComponent>(instance.Key);
                        gameHUD.Append(" Ammo: ");
                        gameHUD.Append(ammo.Amount);

                    }
                }

                // this call gives us the height of the text,
                // so now we are able to stack them on top of each other.
                float textHeight = spriteFont.MeasureString(gameHUD).Y;

                float xPosition = titlesafearea.Width - spriteFont.MeasureString(gameHUD).X - 10;
                float yPosition = titlesafearea.Height - (textHeight + previousHeight);
                previousHeight += textHeight;

                position = new Vector2(xPosition, yPosition);
                

            }

            _gameDependencies.SpriteBatch.Begin(SpriteSortMode.FrontToBack);
            _gameDependencies.SpriteBatch.DrawString(spriteFont, gameHUD, position, Color.BlueViolet);
            _gameDependencies.SpriteBatch.End();
        }
    }
}
