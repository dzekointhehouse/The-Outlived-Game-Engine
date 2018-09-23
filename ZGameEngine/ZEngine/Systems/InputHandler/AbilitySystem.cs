using System;
using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Constants;
using ZEngine.Components;
using ZEngine.EventBus;
using ZEngine.Managers;

namespace ZEngine.Systems.InputHandler
{
    class AbilitySystem : ISystem
    {
        private string TurnAroundEventName = EventConstants.TurnAround;


        public void Start()
        {
        }

    }
}