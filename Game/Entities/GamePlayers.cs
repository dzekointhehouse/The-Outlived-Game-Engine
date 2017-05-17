using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Menu.States;
using Game.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Penumbra;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Components.SpriteAnimation;
using Spelkonstruktionsprojekt.ZEngine.Constants;
using Spelkonstruktionsprojekt.ZEngine.Helpers;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using ZEngine.Components;
using ZEngine.Managers;

namespace Game.Entities
{
   public class GamePlayers
    {
        public GamePlayers()
        {
            
        }

        public void CreatePlayers(GameConfig config)
        {
            foreach (var player in config.Players)
            {
                if (player.Team == MultiplayerMenu.TeamState.TeamOne)
                {
                    switch (player.Index)
                    {
                        case PlayerIndex.One:
                            this.InitPlayerOne(1);
                            break;
                        case PlayerIndex.Two:
                            this.InitPlayerTwo(1);
                            break;
                        case PlayerIndex.Three:
                            this.InitPlayerThree(1);
                            break;
                        case PlayerIndex.Four:
                            this.InitPlayerFour(1);
                            break;
                    };
                }
                if (player.Team == MultiplayerMenu.TeamState.TeamTwo)
                {
                    switch (player.Index)
                    {
                        case PlayerIndex.One:
                            this.InitPlayerOne(2);
                            break;
                        case PlayerIndex.Two:
                            this.InitPlayerTwo(2);
                            break;
                        case PlayerIndex.Three:
                            this.InitPlayerThree(2);
                            break;
                        case PlayerIndex.Four:
                            this.InitPlayerFour(2);
                            break;
                    };
                }

            }


        }

        // TODO should get spawn positions depending on map
        private void InitPlayerOne(int cageId)
        {
            var player1 = EntityManager.GetEntityManager().NewEntity();
            var actionBindings1 = new ActionBindingsBuilder()
                .SetAction(Keys.W, EventConstants.WalkForward) //Use of the next gen constants :)
                .SetAction(Keys.S, EventConstants.WalkBackward)
                .SetAction(Keys.A, EventConstants.TurnLeft)
                .SetAction(Keys.D, EventConstants.TurnRight)
                .SetAction(Keys.Q, EventConstants.TurnAround)
                .SetAction(Keys.E, EventConstants.FireWeapon)
                .SetAction(Keys.LeftShift, EventConstants.LightStatus)
                .SetAction(Keys.R, EventConstants.Running)
                .Build();

            CreatePlayer(
                new Vector2(1650, 1100),
                name: "Carlos",
                actionBindings: actionBindings1,
                position: new Vector2(200, 200), // spawn point
                cageId: cageId
            );
        }

        private void InitPlayerTwo(int cageId)
        {
            var player2 = EntityManager.GetEntityManager().NewEntity();
            var actionBindings2 = new ActionBindingsBuilder()
                .SetAction(Keys.I, EventConstants.WalkForward)
                .SetAction(Keys.K, EventConstants.WalkBackward)
                .SetAction(Keys.J, EventConstants.TurnLeft)
                .SetAction(Keys.L, EventConstants.TurnRight)
                .SetAction(Keys.O, EventConstants.FireWeapon)
                .SetAction(Keys.U, EventConstants.TurnAround)
                .SetAction(Keys.H, EventConstants.LightStatus)
                .Build();

            CreatePlayer(
                new Vector2(1650, 1100),
                "Elvir",
                actionBindings2,
                position: new Vector2(400, 400), // spawn point
                cageId: cageId,
                disabled: false
            );
        }

        private void InitPlayerThree(int cageId)
        {
            var player3 = EntityManager.GetEntityManager().NewEntity();
            var actionBindings = new ActionBindingsBuilder()
                .SetAction(Keys.Up, EventConstants.WalkForward)
                .SetAction(Keys.Down, EventConstants.WalkBackward)
                .SetAction(Keys.Left, EventConstants.TurnLeft)
                .SetAction(Keys.Right, EventConstants.TurnRight)
                .SetAction(Keys.PageDown, EventConstants.FireWeapon)
                .SetAction(Keys.PageUp, EventConstants.TurnAround)
                .SetAction(Keys.RightControl, EventConstants.Running)
                .Build();

            CreatePlayer(
                new Vector2(1650, 1100),
                "Jacob",
                actionBindings,
                position: new Vector2(300, 400), // spawn point
                cageId: cageId,
                disabled: false
                );

        }

        private void InitPlayerFour(int cageId)
        {
            var player3 = EntityManager.GetEntityManager().NewEntity();
            var actionBindings = new ActionBindingsBuilder()
                .SetAction(Keys.Up, EventConstants.WalkForward)
                .SetAction(Keys.Down, EventConstants.WalkBackward)
                .SetAction(Keys.Left, EventConstants.TurnLeft)
                .SetAction(Keys.Right, EventConstants.TurnRight)
                .SetAction(Keys.PageDown, EventConstants.FireWeapon)
                .SetAction(Keys.PageUp, EventConstants.TurnAround)
                .SetAction(Keys.RightControl, EventConstants.Running)
                .Build();

            CreatePlayer(
                new Vector2(1650, 1100),
                "Jacob",
                actionBindings,
                position: new Vector2(250, 250), // spawn point
                cageId: cageId,
                disabled: false
                );

        }

        //The multitude of options here is for easy debug purposes
        private void CreatePlayer(Vector2 playerPosition, string name, ActionBindings actionBindings,
            Vector2 position,
            MoveComponent customMoveComponent = null,
            bool disabled = false, int cageId = 0)
        {
            if (disabled) return;

            var light = new Spotlight()
            {
                Position = position,
                Scale = new Vector2(850f),
                Radius = (float)0.0001,
                Intensity = (float)0.6,
                ShadowType = ShadowType.Solid // Will not lit hulls themselves
            };
            IEntityBuilder playerEntity = new EntityBuilder()
                .SetPosition(position, 10)
                .SetRendering(100, 100)
                .SetInertiaDampening()
                .SetBackwardsPenalty()
                .SetSprite("player_sprites", new Point(1252, 206), 313, 206)
                .SetLight(light)
                .SetSound("walking")
                .SetMovement(200, 380, 4, new Random(DateTime.Now.Millisecond).Next(0, 40) / 10) // Random direction
                .SetRectangleCollision()
                .SetCameraFollow()
                .SetPlayer(name)
                .SetHealth()
                .SetScore()
                .SetAmmo()
                .SetHUD(false, showStats: true)
                .Build();


            var animationBindings = new SpriteAnimationBindingsBuilder()
                .Binding(
                    new SpriteAnimationBindingBuilder()
                        .Positions(new Point(1252, 206), new Point(0, 1030))
                        .StateConditions(State.WalkingForward)
                        .Length(40)
                        .Build()
                )
                .Binding(
                    new SpriteAnimationBindingBuilder()
                        .Positions(new Point(0, 0), new Point(939, 206))
                        .StateConditions(State.Dead, State.WalkingForward)
                        .IsTransition(true)
                        .Length(30)
                        .Build()
                )
                .Binding(
                    new SpriteAnimationBindingBuilder()
                        .Positions(new Point(0, 0), new Point(939, 206))
                        .StateConditions(State.Dead, State.WalkingBackwards)
                        .IsTransition(true)
                        .Length(30)
                        .Build()
                )
                .Binding(
                    new SpriteAnimationBindingBuilder()
                        .Positions(new Point(0, 0), new Point(939, 206))
                        .StateConditions(State.Dead)
                        .IsTransition(true)
                        .Length(30)
                        .Build()
                )
                .Build();

            ComponentManager.Instance.AddComponentToEntity(actionBindings, playerEntity.GetEntityKey());
            ComponentManager.Instance.AddComponentToEntity(animationBindings, playerEntity.GetEntityKey());


                var cageComponent = new CageComponent()
                {
                    CageId = cageId
                };
                ComponentManager.Instance.AddComponentToEntity(cageComponent, playerEntity.GetEntityKey());

            var weaponComponent = new WeaponComponent()
            {
                Damage = 10
            };
            ComponentManager.Instance.AddComponentToEntity(weaponComponent, playerEntity.GetEntityKey());

        }
    }
}
