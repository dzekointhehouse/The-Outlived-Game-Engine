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
using ZEngine.EventBus;
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
        private EventBus EventBus = EventBus.Instance;

        private StringBuilder gameHUD = new StringBuilder(0, 50);
        private StringBuilder scoreGameHUD = new StringBuilder(0, 50);
        private StringBuilder ammoGameHUD = new StringBuilder(0, 50);
        private StringBuilder playerGameHUD = new StringBuilder(0, 50);
        private SpriteFont spriteFont;


        public void Start(SpriteFont spriteFont)
        {
            EventBus.Subscribe<uint>(EventConstants.PlayerDeath, UpdateTeamKills);
            this.spriteFont = spriteFont;
        }
        // This draw method is used to start the system process.
        // it uses DrawTitlesafeStrings to draw the components.
        public void Draw(SpriteBatch sb)
        {
            var viewportComponent = ComponentManager.Instance.GetEntitiesWithComponent(typeof(DefaultViewport)).First().Value as DefaultViewport;

            sb.GraphicsDevice.Viewport = viewportComponent.Viewport;

            sb.Begin(SpriteSortMode.FrontToBack);
            DrawTitlesafeStrings(sb);
            DrawTitlesafeTextures(sb);
            sb.End();
        }


        // DrwaTitleSafe gets all the components and draws them in a
        // correct format, so the data will sortet so that each entity will
        // have it's own row, and each component will be sorted by column.
        private void DrawTitlesafeStrings(SpriteBatch sb)
        {
            GraphicsDevice graphicsDevice = sb.GraphicsDevice;
            Rectangle titlesafearea = graphicsDevice.Viewport.TitleSafeArea;

            Dictionary<uint, IComponent> HUDComponents = ComponentManager.Instance.GetEntitiesWithComponent(typeof(RenderHUDComponent));

            //SpriteFont spriteFont = default(SpriteFont);
            // We save the previous text height so we can stack
            // them (the text for every player) on top of eachother.

            float healthSpacing = 0.33f;
            float scoreSpacing = 0.33f;
            float ammoSpacing = 0.33f;
            float playerSpacing = 0.33f;

           // ContentManager contentManager = _gameDependencies.GameContent as ContentManager;

            foreach (var instance in HUDComponents)
            {
                gameHUD.Clear();
                scoreGameHUD.Clear();
                ammoGameHUD.Clear();
                playerGameHUD.Clear();

                var HUD = instance.Value as RenderHUDComponent;

               // spriteFont = contentManager.Load<SpriteFont>(HUD.SpriteFont);
                Vector2 position = Vector2.Zero;

                gameHUD.AppendLine();
                scoreGameHUD.AppendLine();

                if (HUD.HUDtext != null)
                {
                    gameHUD.Append(HUD.HUDtext);
                    scoreGameHUD.Append(HUD.HUDtext);
                    ammoGameHUD.Append(HUD.HUDtext);
                    playerGameHUD.Append(HUD.HUDtext);
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
                            healthSpacing += 0.06f;

                            position = new Vector2(xPosition, yPosition);
                            sb.DrawString(spriteFont, gameHUD, position, HUD.Color);

                        }

                        // adding player name
                        if (ComponentManager.Instance.EntityHasComponent<EntityScoreComponent>(instance.Key))
                        {
                            playerGameHUD.Clear();

                            playerGameHUD.AppendLine();
                            playerGameHUD.Append(player.Name);
                        
                            float playerXPosition = titlesafearea.Width * playerSpacing;
                            float playerYPosition = titlesafearea.Height * 0.78f;
                            playerSpacing += 0.06f;

                            Vector2 scorePosition = new Vector2(playerXPosition, playerYPosition);

                            sb.DrawString(spriteFont, playerGameHUD, scorePosition, HUD.Color);
                        }

                        // adding score
                        if (ComponentManager.Instance.EntityHasComponent<EntityScoreComponent>(instance.Key))
                        {
                            scoreGameHUD.Clear();
                            var currentScore = (int)score.score;

                            scoreGameHUD.AppendLine();
                            scoreGameHUD.Append(currentScore);

                            float scoreXPosition = titlesafearea.Width * scoreSpacing;
                            float scoreYPosition = titlesafearea.Height * 0.86f;
                            scoreSpacing += 0.06f;

                            Vector2 scorePosition = new Vector2(scoreXPosition, scoreYPosition);

                            sb.DrawString(spriteFont, scoreGameHUD, scorePosition, HUD.Color);
                        }

                        // adding ammo here the same way.
                        if (ComponentManager.Instance.EntityHasComponent<AmmoComponent>(instance.Key))
                        {
                            gameHUD.Clear();
                            AmmoComponent ammo = ComponentManager.Instance.GetEntityComponentOrDefault<AmmoComponent>(instance.Key);

                            if (health.Alive)
                            {
                                // for formating and adding the amount to the HUD.
                                ammoGameHUD.AppendLine();
                                ammoGameHUD.Append(ammo.Amount);
                                ammoGameHUD.Append('/');
                                ammoGameHUD.Append(ammo.SpareAmmoAmount);
                            }
                            else
                            {
                                ammoGameHUD.AppendLine();
                                ammoGameHUD.Append(0);
                            }


                            // this call gives us the height of the text,
                            // so now we are able to stack them on top of each other.
                            float ammoXPosition = titlesafearea.Width * ammoSpacing;
                            float ammoYPosition = titlesafearea.Height * 0.91f;
                            ammoSpacing += 0.06f;

                            Vector2 ammoPosition = new Vector2(ammoXPosition, ammoYPosition);

                            sb.DrawString(spriteFont, ammoGameHUD, ammoPosition, HUD.Color);

                        }
                    }

                }
                // Drawing other strings

                if (HUD.IsOnlyHUD)
                {
                    var pos =
                        ComponentManager.Instance.GetEntityComponentOrDefault<PositionComponent>(instance.Key);
                    sb.DrawString(
                        spriteFont,
                        gameHUD,
                        new Vector2(titlesafearea.X + pos.Position.X, titlesafearea.Y + pos.Position.Y),
                        HUD.Color);
                }
            }


            var nCameras = ComponentManager.Instance.GetEntitiesWithComponent(typeof(CameraViewComponent)).Count;

            if (nCameras > 1)
            {

                var gameScoreComponent = ComponentManager.Instance.GetEntitiesWithComponent(typeof(GameScoreComponent))
                    .FirstOrDefault();
                var gamescore = gameScoreComponent.Value as GameScoreComponent;

                string scoreTextTeamOne = "Team 1: " + gamescore.KillsTeamOne;

                sb.DrawString(
                    spriteFont,
                    scoreTextTeamOne,
                    new Vector2(titlesafearea.Width * 0.5f - (spriteFont.MeasureString(scoreTextTeamOne).X + 50),
                        titlesafearea.Y + 50),
                    Color.Blue);

                string scoreTextTeamTwo = "Team 2: " + gamescore.KillsTeamTwo;

                sb.DrawString(
                    spriteFont,
                    scoreTextTeamTwo,
                    new Vector2(titlesafearea.Width * 0.5f + 50, titlesafearea.Y + 50),
                    Color.Red);
            }
        }

        private void DrawTitlesafeTextures(SpriteBatch sb)
        {
            Rectangle titlesafearea = sb.GraphicsDevice.Viewport.TitleSafeArea;

            Dictionary<uint, IComponent> HUDComponents = ComponentManager.Instance.GetEntitiesWithComponent(typeof(RenderHUDComponent));

            foreach (var instance in HUDComponents)
            {
                var HUD = instance.Value as RenderHUDComponent;

                if (HUD.IsOnlyHUD)
                {
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

                    sb.Draw(
                        texture: sprite.Sprite,
                        destinationRectangle: destinationRectangle,
                        sourceRectangle: null,
                        color: HUD.Color * sprite.Alpha
                        );
                }
            }
        }
        private void UpdateTeamKills(uint entity)
        {
            var team = ComponentManager.Instance.GetEntityComponentOrDefault<TeamComponent>(entity);

            if (team.TeamId == 1)
            {
                var gameScoreComponent = ComponentManager.Instance.GetEntitiesWithComponent(typeof(GameScoreComponent)).FirstOrDefault();
                var gamescore = gameScoreComponent.Value as GameScoreComponent;
                gamescore.KillsTeamTwo++;
            }
            else if (team.TeamId == 2)
            {
                var gameScoreComponent = ComponentManager.Instance.GetEntitiesWithComponent(typeof(GameScoreComponent)).FirstOrDefault();
                var gamescore = gameScoreComponent.Value as GameScoreComponent;
                gamescore.KillsTeamOne++;
            }
        }
    }
}
