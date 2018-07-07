using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Helpers;

namespace Game.Entities
{
    public static class GameHUD
    {
        public static void CreateSinglePlayerHUD()
        {
            new EntityBuilder()
                .SetHUD(true)
                .SetPosition(new Vector2(590, 900))
                .SetSprite("health3_small")
                .Build();

            new EntityBuilder()
                .SetHUD(true)
                .SetPosition(new Vector2(590, 950))
                .SetSprite("medal")
                .Build();

            new EntityBuilder()
                .SetHUD(true)
                .SetPosition(new Vector2(550, 1000))
                .SetSprite("ammo")
                .Build();
        }
    }

    enum TypeHUD
    {
        Keyboard,
        Xbox,

    }
}
