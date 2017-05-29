using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MoreLinq;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using ZEngine.Components;
using ZEngine.Managers;
using Spelkonstruktionsprojekt.ZEngine.Systems.InputHandler;
using System.Timers;

namespace Spelkonstruktionsprojekt.ZEngine.Systems
{
    class AISystem : ISystem
    {
        private ComponentManager ComponentManager = ComponentManager.Instance;
        private Random Random = new Random();

        public void Update(GameTime gameTime)
        {
            var worldComponent = ComponentManager.Instance.GetEntitiesWithComponent(typeof(WorldComponent)).First();
            var world = worldComponent.Value as WorldComponent;

            foreach (var entity in ComponentManager.GetEntitiesWithComponent(typeof(AIComponent)))
            {
                var aiMoveComponent = ComponentManager.GetEntityComponentOrDefault<MoveComponent>(entity.Key);
                var aiPositionComponent = ComponentManager.GetEntityComponentOrDefault<PositionComponent>(entity.Key);
                var aiComponent = entity.Value as AIComponent;
                if (aiMoveComponent == null || aiPositionComponent == null || aiComponent == null) continue;
                var aiPosition = aiPositionComponent.Position;

                var closestPlayerDistance = 9999.0;
                Vector2? closestPlayerPosition = null;
                var closestPlayerHasFlashlightOn = false;
                foreach (var player in ComponentManager.GetEntitiesWithComponent(typeof(PlayerComponent)))
                {
                    var positionComponent =
                        ComponentManager.Instance.GetEntityComponentOrDefault<PositionComponent>(player.Key);
                    if (positionComponent == null) continue;

                    var light = ComponentManager.Instance.GetEntityComponentOrDefault<LightComponent>(player.Key);
                    var hasFlashlightOn = light.Light.Enabled;

                    var distance = Vector2.Distance(positionComponent.Position, aiPosition);

                    if (closestPlayerHasFlashlightOn && hasFlashlightOn)
                    {
                        if (distance < closestPlayerDistance)
                        {
                            closestPlayerDistance = distance;
                            closestPlayerPosition = positionComponent.Position;
                        }
                    }
                    else if (hasFlashlightOn)
                    {
                        closestPlayerDistance = distance;
                        closestPlayerPosition = positionComponent.Position;
                        closestPlayerHasFlashlightOn = true;
                    }
                    else if (distance < closestPlayerDistance && distance <= aiComponent.FollowDistance)
                    {
                        closestPlayerDistance = distance;
                        closestPlayerPosition = positionComponent.Position;
                    }
                }

                var foundPlayerToFollow = closestPlayerPosition != null;
                if (foundPlayerToFollow)
                {
                    aiComponent.Wander = false;
                    var dir = closestPlayerPosition.Value - aiPosition;
                    //dir.Normalize();

                    //Vector2 v = GetWorldCoordinates(aiPosition, world);
                    //Node node = Astar.Search(world.World, new Node((int)v.X, (int)v.Y), new Node((int)closestPlayerPosition.Value.X, (int)closestPlayerPosition.Value.Y));
                    //Vector2 dir = GetScreenCoordinates(node, world);

                    if (gameTime.TotalGameTime.TotalMilliseconds - aiComponent.TimeOfLastWallCollision > 2000)
                    {
                        dir.Normalize();
                        var newDirection = Math.Atan2(dir.Y, dir.X);

                        if (aiMoveComponent.Direction < newDirection - Math.PI * 0.01
                            || aiMoveComponent.Direction > newDirection + Math.PI * 0.01)
                        {
                            Debug.WriteLine("A");
                            aiMoveComponent.Direction = (float) newDirection;
                            aiMoveComponent.RotationMomentum = 0;
                        }
                        else
                        {
                            if (aiMoveComponent.Direction <= Math.PI)
                            {
                                Debug.WriteLine("B");
                                aiMoveComponent.RotationMomentum = -0.01;
                            }
                            else
                            {
                                Debug.WriteLine("C");
                                aiMoveComponent.RotationMomentum = 0.01;
                            }
                        }

                        //aiMoveComponent.Direction = (float)newDirection;
                    }

                    aiMoveComponent.CurrentAcceleration = aiMoveComponent.AccelerationSpeed; //Make AI move
                    //var newDirection = Math.Atan2(dir.Y, dir.X);
                    //aiMoveComponent.Direction = (float) newDirection;
                    //aiMoveComponent.CurrentAcceleration = aiMoveComponent.AccelerationSpeed; //Make AI move.


                }
                else if (aiComponent.Wander == false)
                {
                    aiComponent.Wander = true;
                    aiMoveComponent.CurrentAcceleration = 3;
                    BeginWander(entity.Key, gameTime.TotalGameTime.TotalMilliseconds);
                }
            }
        }

        public Vector2 GetWorldCoordinates(Vector2 position, WorldComponent world)
        {
            var X = Math.Floor((position.X / world.WorldWidth) * 10);
            var Y = Math.Floor((position.Y / world.WorldHeight) * 10);

            return new Vector2((float) X,(float) Y);
        }

        public Vector2 GetScreenCoordinates(Node position, WorldComponent world)
        {
            var X = position.X * 10;
            var Y = position.Y * 10;

            return new Vector2((float)X, (float)Y);
        }

        public void BeginWander(uint entityId, double startTime)
        {
            var animationComponent = GetOrCreateDefault(entityId);
            var animation = new GeneralAnimation
            {
                AnimationType = "AiWander",
                StartOfAnimation = startTime,
                Unique = true,
                Length = 5000
            };
            NewWanderAnimation(entityId, animation);
            animationComponent.Animations.Add(animation);
        }

        public void NewWanderAnimation(uint entityId, GeneralAnimation generalAnimation)
        {
            var moveComponent = ComponentManager.GetEntityComponentOrDefault<MoveComponent>(entityId);
            var aiComponent = ComponentManager.GetEntityComponentOrDefault<AIComponent>(entityId);
            var turnRange = MathHelper.TwoPi * 0.8;
            double start = moveComponent.Direction;
            double target = (moveComponent.Direction - turnRange * 0.5)
                                     + Random.NextDouble() * turnRange;

            var originalMaxVelocity = moveComponent.MaxVelocitySpeed;
            moveComponent.MaxVelocitySpeed = 25;
            generalAnimation.Animation = delegate(double currentTime)
            {
                if (!aiComponent.Wander)
                {
                    moveComponent.MaxVelocitySpeed = originalMaxVelocity;
                    generalAnimation.IsDone = true;
                    return;
                }

                var elapsedTime = currentTime - generalAnimation.StartOfAnimation;
                if (elapsedTime > generalAnimation.Length)
                {
                    start = moveComponent.Direction;
                    target = (moveComponent.Direction - turnRange * 0.5)
                                    + Random.NextDouble() * turnRange;
                    generalAnimation.StartOfAnimation = currentTime;
                }

                var turnAroundTime = generalAnimation.Length * 0.3;
                if (elapsedTime < turnAroundTime)
                {
                    //Algorithm for turning stepwise on each iteration
                    //Modulus is for when the direction makes a whole turn
                    moveComponent.Direction = (float)
                        (start + (target - start) / turnAroundTime * elapsedTime);
                }
            };
        }



        private AnimationComponent GetOrCreateDefault(uint entityId)
        {
            var animationComponent = ComponentManager.GetEntityComponentOrDefault<AnimationComponent>(entityId);
            if (animationComponent != null) return animationComponent;

            animationComponent = ComponentManager.ComponentFactory.NewComponent<AnimationComponent>();
            ComponentManager.AddComponentToEntity(animationComponent, entityId);
            return animationComponent;
        }
    }
}