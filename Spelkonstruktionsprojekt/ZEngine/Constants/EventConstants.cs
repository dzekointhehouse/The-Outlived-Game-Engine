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
        public static readonly string WalkForward = "entityWalkForwards";
        public static readonly string WalkBackward = "entityWalkBackwards";
        public static readonly string TurnLeft = "entityTurnLeft";
        public static readonly string TurnRight = "entityTurnRight";
        public static readonly string TurnAround = "entityTurnAround";
        public static readonly string FireWeapon = "entityFireWeapon";
        public static readonly string ReloadWeapon = "entityReloadWeapon";
        public static readonly string Running = "entityRun";


        // Collisions
        public static readonly string WallCollision = "WallCollision";
        public static readonly string EnemyCollision = "EnemyCollision";
        public static readonly string BulletCollision = "BulletCollision";
    }
}
