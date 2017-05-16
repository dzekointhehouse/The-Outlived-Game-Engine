using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Components.RenderComponent;
using Spelkonstruktionsprojekt.ZEngine.Constants;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using ZEngine.Components;
using ZEngine.Managers;
using ZEngine.Wrappers;

namespace Spelkonstruktionsprojekt.ZEngine.Systems.Rendering
{
    // TODO: Maybe use keyevent to display hud effects depending on key pressed.
    // This system is used for rendering the HUD, those entities
    // that have instances ofcomponents like health or ammo will 
    // be able to se the component data on the screen when playing.
    // This can be used to that the player always can see the status
    // of his player. (everything drawn here is and should be titlesafe) 
    class RenderHUDSystem : ISystem
    {
        public static string SystemName = "RenderHUDSystem";
        private GameDependencies _gameDependencies;

        private StringBuilder gameHUD = new StringBuilder(0, 50);
        private StringBuilder scoreGameHUD = new StringBuilder(0, 50);
        private StringBuilder ammoGameHUD = new StringBuilder(0, 50);

        // This draw method is used to start the system process.
        // it uses DrawTitlesafeStrings to draw the components.
        public void Draw(GameDependencies gameDependencies)
        {
            this._gameDependencies = gameDependencies;

            _gameDependencies.SpriteBatch.Begin(SpriteSortMode.FrontToBack);
            DrawTitlesafeStrings();
            DrawTitlesafeTextures();
            _gameDependencies.SpriteBatch.End();
        }


        // DrwaTitleSafe gets all the components and draws them in a
        // correct format, so the data will sortet so that each entity will
        // have it's own row, and each component will be sorted by column.
        private void DrawTitlesafeStrings()
        {
            GraphicsDevice graphics = _gameDependencies.GraphicsDeviceManager.GraphicsDevice;
            Rectangle titlesafearea = graphics.Viewport.TitleSafeArea;

            Dictionary<int, IComponent> HUDComponents = ComponentManager.Instance.GetEntitiesWithComponent(typeof(RenderHUDComponent));

            // We save the previous text height so we can stack
            // them (the text for every player) on top of eachother.

            float healthSpacing = 0.33f;
            float scoreSpacing = 0.33f;
            float ammoSpacing = 0.33f;

            ContentManager contentManager = _gameDependencies.GameContent as ContentManager;

            foreach (var instance in HUDComponents)
            {
                gameHUD.Clear();
                scoreGameHUD.Clear();
                ammoGameHUD.Clear();

                var HUD = instance.Value as RenderHUDComponent;

                SpriteFont spriteFont = contentManager.Load<SpriteFont>(HUD.SpriteFont);
                Vector2 position = Vector2.Zero;

                gameHUD.AppendLine();
                scoreGameHUD.AppendLine();

                if (HUD.HUDtext != null)
                {
                    gameHUD.Append(HUD.HUDtext);
                    scoreGameHUD.Append(HUD.HUDtext);
                }

                // We execute this if - statement, if showstats is true,
                // which states that we should show the entity's health and
                // ammo if it has it.
                if (HUD.ShowStats)
                {
                    PlayerComponent player = ComponentManager.Instance.GetEntityComponentOrDefault<PlayerComponent>(instance.Key);
                    if (player != null)
                    {
                        HealthComponent health = ComponentManager.Instance.GetEntityComponentOrDefault<HealthComponent>(instance.Key);
                        if (health == null) return;

                        EntityScoreComponent score = ComponentManager.Instance.GetEntityComponentOrDefault<EntityScoreComponent>(instance.Key);
                        if (score == null) return;
                        // gameHUD.Append(player.Name);

                        float xPosition;
                        float yPosition;

                        if (health != null)
                        {
                            gameHUD.Clear();
                            var currentHealth = health.CurrentHealth;

                            // for formating and adding the amount to the HUD.
                            gameHUD.AppendLine();
                            gameHUD.Append(currentHealth);

                            // this call gives us the height of the text,
                            // so now we are able to stack them on top of each other.

                            xPosition = titlesafearea.Width * healthSpacing;
                            yPosition = titlesafearea.Height * 0.81f;
                            healthSpacing += 0.04f;

                            position = new Vector2(xPosition, yPosition);
                            _gameDependencies.SpriteBatch.DrawString(spriteFont, gameHUD, position, HUD.FontColor);                            

                        }

                        // adding score
                        if(ComponentManager.Instance.EntityHasComponent<EntityScoreComponent>(instance.Key))
                        {
                            scoreGameHUD.Clear();
                            var currentScore = (int)score.score;

                            scoreGameHUD.AppendLine();
                            scoreGameHUD.Append(currentScore);

                            float scoreXPosition = (titlesafearea.Width * 1f) * scoreSpacing;
                            float scoreYPosition = titlesafearea.Height * 0.86f;
                            scoreSpacing += 0.04f;

                            Vector2 scorePosition = new Vector2(scoreXPosition, scoreYPosition);

                            _gameDependencies.SpriteBatch.DrawString(spriteFont, scoreGameHUD, scorePosition, HUD.FontColor);
                        }

                        // adding ammo here the same way.
                        if (ComponentManager.Instance.EntityHasComponent<AmmoComponent>(instance.Key) && health.Alive)
                        {
                            gameHUD.Clear();
                            AmmoComponent ammo = ComponentManager.Instance.GetEntityComponentOrDefault<AmmoComponent>(instance.Key);
                            
                            // for formating and adding the amount to the HUD.
                            ammoGameHUD.AppendLine();
                            ammoGameHUD.Append(ammo.Amount);


                            // this call gives us the height of the text,
                            // so now we are able to stack them on top of each other.
                            float ammoXPosition = (titlesafearea.Width * 1f) * ammoSpacing;
                            float ammoYPosition = titlesafearea.Height * 0.91f;
                            ammoSpacing += 0.04f;

                            Vector2 ammoPosition = new Vector2(ammoXPosition, ammoYPosition);

                            _gameDependencies.SpriteBatch.DrawString(spriteFont, ammoGameHUD, ammoPosition, HUD.FontColor);

                        }
                    }
                }

                _gameDependencies.SpriteBatch.DrawString(spriteFont, gameHUD, position, HUD.FontColor);

            }

        }

        private void DrawTitlesafeTextures()
        {
            GraphicsDevice graphics = _gameDependencies.GraphicsDeviceManager.GraphicsDevice;
            Rectangle titlesafearea = graphics.Viewport.TitleSafeArea;

            Dictionary<int, IComponent> HUDComponents = ComponentManager.Instance.GetEntitiesWithComponent(typeof(RenderHUDComponent));

            foreach (var instance in HUDComponents)
            {
                var HUD = instance.Value as RenderHUDComponent;

                if (HUD.IsOnlyHUD) { 
                var position = ComponentManager.Instance.GetEntityComponentOrDefault<PositionComponent>(instance.Key);
                if (position == null) continue;

                var sprite = ComponentManager.Instance.GetEntityComponentOrDefault<SpriteComponent>(instance.Key);
                if (sprite == null) continue;

               // Vector2 titlesafePosition = new Vector2(titlesafearea.X + position.Position.X, titlesafearea.Y + position.Position.Y);

                    Rectangle destinationRectangle =
                        new Rectangle(
                            (int)(titlesafearea.X + position.Position.X),
                            (int)(titlesafearea.Y + position.Position.Y),
                            (int)(sprite.Sprite.Width * sprite.Scale),
                            (int)(sprite.Sprite.Height * sprite.Scale)
                        );

                    _gameDependencies.SpriteBatch.Draw(
                        texture: sprite.Sprite,
                        destinationRectangle: destinationRectangle,
                        sourceRectangle: null,
                        color: Color.White * sprite.Alpha
                        );
                }
            }



        }

    }
}
