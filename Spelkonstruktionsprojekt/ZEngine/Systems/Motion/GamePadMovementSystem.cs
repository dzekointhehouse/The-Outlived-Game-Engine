using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Components.Input;
using Spelkonstruktionsprojekt.ZEngine.Constants;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using Spelkonstruktionsprojekt.ZEngine.Systems.InputHandler;
using ZEngine.Components;
using ZEngine.EventBus;
using ZEngine.Managers;

namespace Spelkonstruktionsprojekt.ZEngine.Systems.Motion
{
    public class GamePadMovementSystem : ISystem
    {
        private readonly EventBus EventBus = EventBus.Instance;
        private readonly ComponentManager ComponentManager = ComponentManager.Instance;

        public void WalkForwards(GameTime gameTime)
        {
            foreach (var entity in ComponentManager.GetEntitiesWithComponent(typeof(GamePadComponent)))
            {
                var moveComponent = ComponentManager.Instance.GetEntityComponentOrDefault<MoveComponent>(entity.Key);
                if (moveComponent == null) return;

                var gamePadIndex = (entity.Value as GamePadComponent).GamePadPlayerIndex;
                var gamePadState = GamePad.GetState(gamePadIndex);
                var leftStickPosition = gamePadState.ThumbSticks.Left;
                if (leftStickPosition.X > 0.15 || leftStickPosition.X < -0.15 || leftStickPosition.Y > 0.15 ||
                    leftStickPosition.Y < -0.15)
                {
                    var direction = Math.Atan2(-leftStickPosition.Y, leftStickPosition.X);
                    moveComponent.Direction = (float) direction;

                    var xThreshold = 0.99f;
                    if (leftStickPosition.X > xThreshold || leftStickPosition.X < -xThreshold ||
                        leftStickPosition.Y > xThreshold || leftStickPosition.Y < -xThreshold)
                    {
                        moveComponent.CurrentAcceleration = moveComponent.AccelerationSpeed;
                        StateManager.TryAddState(entity.Key, State.WalkingForward,
                            gameTime.TotalGameTime.TotalMilliseconds);
                    }
                }
                else if (gamePadState.IsButtonDown(Buttons.DPadDown))
                {
                    moveComponent.CurrentAcceleration = -moveComponent.AccelerationSpeed;
                    StateManager.TryAddState(entity.Key, State.WalkingBackwards, gameTime.TotalGameTime.TotalMilliseconds);
                }
                else
                {
                    moveComponent.CurrentAcceleration = 0;
                    StateManager.TryRemoveState(entity.Key, State.WalkingForward,
                        gameTime.TotalGameTime.TotalMilliseconds);
                }

            }
        }
    }
}