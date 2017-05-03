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
        private Vector2 closestPlayerPosition;
        private float closestPlayerDistance;

        public void Update(GameTime gameTime)
        {
            var delta = gameTime.ElapsedGameTime.TotalSeconds;
            foreach (var entity in ComponentManager.GetEntitiesWithComponent(typeof(AIComponent)))
            {
                var aiMoveComponent = ComponentManager.GetEntityComponentOrDefault<MoveComponent>(entity.Key);
                var aiPositionComponent = ComponentManager.GetEntityComponentOrDefault<PositionComponent>(entity.Key);
                var aiComponent = entity.Value as AIComponent;
                var aiPosition = aiPositionComponent.Position;

                var playerEntities = ComponentManager.GetEntitiesWithComponent(typeof(PlayerComponent));
                if (playerEntities.Count == 0)
                {
                    aiMoveComponent.CurrentAcceleration = 0;
                    continue;
                }
                //Get closest players that
                //    - Has position component
                //    - Has flashlight component.
                //    - Has a flashlight turned on
                // We want to get all the players that achieve this criteria above.
                // We do this by using linq. The first where clause gets the players that have
                // that criteria, select clause selects the player properties that we need, and
                // minby compares the distances and gives us the player with the smallest distance
                // to the player.
                //var closestPlayer = playerEntities
                //    .Where(e =>
                //    {
                //        var hasPositionComponent =
                //            ComponentManager.Instance.EntityHasComponent<PositionComponent>(e.Key);
                //        if (!hasPositionComponent) return false;

                //       else return true;
                //    })
                //    .Select(e =>
                //    {
                //        var positionComponent =
                //            ComponentManager.Instance.GetEntityComponentOrDefault<PositionComponent>(e.Key);
                //        var distance = Vector2.Distance(positionComponent.Position, aiPosition);

                //        return new Tuple<float, PositionComponent>(
                //            distance,
                //            positionComponent
                //        );
                //    })
                //    .MinBy(e =>
                //    {
                //        // item1 is the distance
                //        var distance = e.Item1;
                //        return distance;
                //    });
                var testplayer = ComponentManager.GetEntitiesWithComponent(typeof(PlayerComponent));
                foreach (var player in ComponentManager.GetEntitiesWithComponent(typeof(PlayerComponent)))
                {
                  //  Tuple<float, PositionComponent> test;
                    //Vector2 closestPlayerPosition;
                    //float closestPlayerDistance;
                    if (ComponentManager.Instance.EntityHasComponent<PositionComponent>(player.Key)) {
                       
                        var positionComponent =
                            ComponentManager.Instance.GetEntityComponentOrDefault<PositionComponent>(player.Key);
                        var distance = Vector2.Distance(positionComponent.Position, aiPosition);
                        float olddistance = 999.9f;
                        if (distance < olddistance) {
                            closestPlayerDistance = distance;
                            closestPlayerPosition = positionComponent.Position;

                        }
                        
                        closestPlayerDistance = olddistance;
                        closestPlayerPosition = positionComponent.Position;
                        olddistance = distance;
                    }
                  
                    LightComponent light = ComponentManager.Instance.GetEntityComponentOrDefault<LightComponent>(player.Key);
                    bool hasFlashlightOn = light.Light.Enabled;
                    if (!hasFlashlightOn)
                    {
                        if (closestPlayerPosition != null && closestPlayerDistance < aiComponent.FollowDistance)
                        {
                            aiComponent.Wander = false;
                            var dir = closestPlayerPosition - aiPosition;
                            dir.Normalize();
                            var newDirection = Math.Atan2(dir.Y, dir.X);

                            aiMoveComponent.Direction = (float)newDirection;

                            aiMoveComponent.CurrentAcceleration = aiMoveComponent.AccelerationSpeed; //Make AI move.
                        }
                        else if (!aiComponent.Wander)
                        {
                            aiComponent.Wander = true;
                            aiMoveComponent.CurrentAcceleration = 3;
                            //InitTimer();
                            BeginWander(entity.Key, gameTime.TotalGameTime.TotalMilliseconds);
                        }
                    }
                    else {

                        aiComponent.Wander = false;
                        var dir = closestPlayerPosition - aiPosition;
                        dir.Normalize();
                        var newDirection = Math.Atan2(dir.Y, dir.X);

                        aiMoveComponent.Direction = (float)newDirection;

                        aiMoveComponent.CurrentAcceleration = aiMoveComponent.AccelerationSpeed; //Make AI move.

                    }
                }
            }
        }

        public void BeginWander(int entityId, double startTime)
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

        public void NewWanderAnimation(int entityId, GeneralAnimation generalAnimation)
        {
            var moveComponent = ComponentManager.GetEntityComponentOrDefault<MoveComponent>(entityId);
            var aiComponent = ComponentManager.GetEntityComponentOrDefault<AIComponent>(entityId);
            double start = moveComponent.Direction;
            double target = Random.NextDouble() * MathHelper.TwoPi % MathHelper.TwoPi;

            generalAnimation.Animation = delegate(double currentTime)
            {
                if (!aiComponent.Wander)
                {
                    generalAnimation.IsDone = true;
                    return;
                }

                var elapsedTime = currentTime - generalAnimation.StartOfAnimation;
                if (elapsedTime > generalAnimation.Length)
                {
                    target = Random.NextDouble() * MathHelper.TwoPi % MathHelper.TwoPi;
                    start = moveComponent.Direction;
                    generalAnimation.StartOfAnimation = currentTime;
                }
                //Algorithm for turning stepwise on each iteration
                //Modulus is for when the direction makes a whole turn
                moveComponent.Direction = (float)
                    ((start + (target - start) / generalAnimation.Length * elapsedTime) % MathHelper.TwoPi);
            };
        }

//        public void InitTimer()
//        {
//            Random r = new Random();
//            var rand = r.Next(2000, 4000);
//
//            Timer timer = new Timer();
//            timer.Elapsed += new ElapsedEventHandler(Wandering);
//            timer.Interval = rand;
//            timer.Start();
//        }

//        public void Wandering(object sender, EventArgs e)
//        {
//            Random rnd = new Random();
//
//            var prevPos = aiMoveComponent.Direction;
//
//            float randX = (float) rnd.NextDouble();
//            float randY = (float) rnd.NextDouble();
//
//            var newDirection = Math.Atan2(randX, randY);
//
//            if (newDirection < prevPos)
//            {
//                aiMoveComponent.RotationMomentum = 0.5;
//            }
//            else if (newDirection > prevPos) aiMoveComponent.RotationMomentum = -0.5;
//
//
//            aiMoveComponent.Speed = 10f;
//        }

        private AnimationComponent GetOrCreateDefault(int entityId)
        {
            var animationComponent = ComponentManager.GetEntityComponentOrDefault<AnimationComponent>(entityId);
            if (animationComponent != null) return animationComponent;

            animationComponent = new AnimationComponent();
            ComponentManager.AddComponentToEntity(animationComponent, entityId);
            return animationComponent;
        }
    }
}