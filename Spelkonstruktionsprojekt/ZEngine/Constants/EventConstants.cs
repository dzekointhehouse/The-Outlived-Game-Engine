using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spelkonstruktionsprojekt.ZEngine.Constants
{
    public static class EventConstants
    {
        // These constants are used to simplify the usage of the eventbus,
        // e.g. when the user wants to set an action for when the entity
        // walks forward We use these variables so the user doesn't have
        // to know the strings, or misspell them for that matter.
        public const string WalkForward = "entityWalkForwards";
        public const string WalkBackward = "entityWalkBackwards";
        public const string TurnLeft = "entityTurnLeft";
        public const string TurnRight = "entityTurnRight";
        public const string TurnAround = "entityTurnAround";
        public const string Running = "entityRun";

        // Weapons
        public const string FireWeaponSound = "fireWeaponSound"; //Command
        public const string FireRifleWeapon = "entityFireRifleWeapon";
        public const string FirePistolWeapon = "entityFirePistolWeapon";
        public const string FireShotgunWeapon = "entityFireShotgunWeapon";
        public const string ReloadWeaponSound = "ReloadWeaponSound";
        public const string ReloadWeapon = "entityReloadWeapon";
        public const string EmptyMagSound = "EmptyMagSound";


        // Collisions
        public const string WallCollision = "WallCollision";
        public const string EnemyCollision = "EnemyCollision";
        public const string BulletCollision = "BulletCollision";
        public const string PickupCollision = "PickupCollision";
        public const string AiWallCollision = "AiWallCollision";


        // Other
        public const string HealthPickup = "HealthPickup";
        public const string AmmoPickup = "AmmoPickup";
        public const string LightStatus = "LightStatus";

    }
}
