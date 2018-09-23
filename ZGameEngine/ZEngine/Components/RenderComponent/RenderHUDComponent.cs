using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using ZEngine.Components;
using static System.String;

namespace Spelkonstruktionsprojekt.ZEngine.Components.RenderComponent
{
    /// <summary>
    /// Entities that have this component added will always
    /// have information visible on the screen, by drawing to 
    /// the titlesafe area.
    /// </summary>
    public class RenderHUDComponent : IComponent
    {
        /// <summary>
        /// Adding a HUD text to this component
        /// will show the text that is added to the screen.
        /// </summary>
        public string HUDtext { get; set; }

        /// <summary>
        /// Determines if we are going to display the
        /// player name, health and ammo for the entity.
        /// </summary>
        public bool ShowStats { get; set; }

        /// <summary>
        /// This is a quick fic because animation sprites that used
        /// to have the player component get rendered as a HUD, so
        /// things get weird. True if the entity is strictly HUD, we only 
        /// want to show those textures as HUD.
        /// </summary>
        public bool IsOnlyHUD { get; set; }


        public string SpriteFont { get; set; }

        public Color Color { get; set; }

        public RenderHUDComponent()
        {
            Reset();
        }

        public IComponent Reset()
        {
            HUDtext = Empty;
            ShowStats = false;
            IsOnlyHUD = false;
            SpriteFont = "ZEone";
            Color = Color.White;
            return this;
        }
    }
}
