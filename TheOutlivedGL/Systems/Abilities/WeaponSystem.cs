using Spelkonstruktionsprojekt.ZEngine.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Game.Systems.Abilities;
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
    class WeaponSystem : ISystem, IUpdateables
    {
        private BulletFactory BulletFactory { get; set; } = new BulletFactory();
        private PistolAbilitySystem PistolAbilitySystem { get; set; } = new PistolAbilitySystem();
        private ShotgunAbilitySystem ShotgunAbilitySystem { get; set; } = new ShotgunAbilitySystem();
        private RifleAbilitySystem RifleAbilitySystem { get; set; } = new RifleAbilitySystem();

        public bool Enabled { get; set; } = true;
        public int UpdateOrder { get; set; }
        public void Update(GameTime gt)
        {
        }
        // On start we subsribe to the events that
        // will be necessary for this system.
        public ISystem Start()
        {
            PistolAbilitySystem.Start(BulletFactory);
            RifleAbilitySystem.Start(BulletFactory);
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