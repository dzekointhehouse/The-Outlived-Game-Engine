using Spelkonstruktionsprojekt.ZEngine.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Constants;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using Spelkonstruktionsprojekt.ZEngine.Systems.Bullets;
using Spelkonstruktionsprojekt.ZEngine.Systems.InputHandler;
using ZEngine.Components;
using ZEngine.EventBus;
using ZEngine.Managers;

namespace Spelkonstruktionsprojekt.ZEngine.Systems
{
    // Weapons system will be handling weapons stuff.
    class WeaponSystem : ISystem
    {
        private BulletFactory BulletFactory { get; set; } = new BulletFactory();
        private PistolAbilitySystem PistolAbilitySystem { get; set; } = new PistolAbilitySystem();

        private ShotgunAbilitySystem ShotgunAbilitySystem { get; set; } = new ShotgunAbilitySystem();

        // On start we subsribe to the events that
        // will be necessary for this system.
        public ISystem Start()
        {
//            PistolAbilitySystem.Start(BulletFactory);
            ShotgunAbilitySystem.Start(BulletFactory);
            return this;
        }

        public ISystem Stop()
        {
            return this;
        }

        public void LoadBulletSpriteEntity()
        {
            BulletFactory.LoadBulletSprites();
        }
    }
}