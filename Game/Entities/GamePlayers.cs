using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Menu.States;
using Game.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Penumbra;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Components.Input;
using Spelkonstruktionsprojekt.ZEngine.Components.SpriteAnimation;
using Spelkonstruktionsprojekt.ZEngine.Constants;
using Spelkonstruktionsprojekt.ZEngine.Helpers;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using ZEngine.Components;
using ZEngine.Managers;
using static Game.Menu.States.CharacterMenu;
using Game = Microsoft.Xna.Framework.Game;

namespace Game.Entities
{
    public class GamePlayers
    {
        private GameConfig config;
        private GameViewports gameViewports;
        private Dictionary<PlayerIndex, Viewport> viewports;
        private bool createdCamera = false;
        private GameMap maps;

        public GamePlayers(GameConfig config, GameViewports gameViewports)
        {
            this.config = config;
            this.gameViewports = gameViewports;
            this.viewports = gameViewports.GetViewports();
        }

        public void CreatePlayers(GameMap maps)
        {
            this.maps = maps;
            foreach (var player in config.Players)
            {
                if (player.Team == MultiplayerMenu.TeamState.TeamOne)
                {
                    switch (player.Index)
                    {
                        case PlayerIndex.One:
                            this.InitPlayerOne(player);
                            break;
                        case PlayerIndex.Two:
                            this.InitPlayerTwo(player);
                            break;
                        case PlayerIndex.Three:
                            this.InitPlayerThree(player);
                            break;
                        case PlayerIndex.Four:
                            this.InitPlayerFour(player);
                            break;
                    }
                }
                if (player.Team == MultiplayerMenu.TeamState.TeamTwo)
                {
                    switch (player.Index)
                    {
                        case PlayerIndex.One:
                            this.InitPlayerOne(player);
                            break;
                        case PlayerIndex.Two:
                            this.InitPlayerTwo(player);
                            break;
                        case PlayerIndex.Three:
                            this.InitPlayerThree(player);
                            break;
                        case PlayerIndex.Four:
                            this.InitPlayerFour(player);
                            break;
                    }
                }
            }
        }


        // TODO should get spawn positions depending on map
        private void InitPlayerOne(Player player)
        {
            var actionBindings1 = new ActionBindingsBuilder()
                .SetAction(Keys.W, EventConstants.WalkForward) //Use of the next gen constants :)
                .SetAction(Keys.S, EventConstants.WalkBackward)
                .SetAction(Keys.A, EventConstants.TurnLeft)
                .SetAction(Keys.D, EventConstants.TurnRight)
                .SetAction(Keys.Q, EventConstants.TurnAround)
                .SetAction(Keys.E, EventConstants.FirePistolWeapon)
                .SetAction(Keys.F, EventConstants.LightStatus)
                .SetAction(Keys.R, EventConstants.ReloadWeapon)
                .SetAction(Keys.D1, EventConstants.KillAllLights)
                .Build();

            CreatePlayer(
                sprite: player.SpriteName,
                actionBindings: actionBindings1,
                position: maps.spawnPositionOne, // spawn point
                viewport: viewports[player.Index],
                characterType: player.CharacterType, cageId: player.CameraId,
                useGamePad: GamePad.GetState(0).IsConnected, gamePadIndex: 0);
        }

        private void InitPlayerTwo(Player player)
        {
            var actionBindings2 = new ActionBindingsBuilder()
                .SetAction(Keys.I, EventConstants.WalkForward)
                .SetAction(Keys.K, EventConstants.WalkBackward)
                .SetAction(Keys.J, EventConstants.TurnLeft)
                .SetAction(Keys.L, EventConstants.TurnRight)
                .SetAction(Keys.O, EventConstants.FirePistolWeapon)
                .SetAction(Keys.U, EventConstants.TurnAround)
                .SetAction(Keys.H, EventConstants.LightStatus)
                .SetAction(Keys.P, EventConstants.ReloadWeapon)
                .Build();


            var actionBindings1 = new ActionBindingsBuilder()
                .SetAction(Keys.W, EventConstants.WalkForward) //Use of the next gen constants :)
                .SetAction(Keys.S, EventConstants.WalkBackward)
                .SetAction(Keys.A, EventConstants.TurnLeft)
                .SetAction(Keys.D, EventConstants.TurnRight)
                .SetAction(Keys.Q, EventConstants.TurnAround)
                .SetAction(Keys.E, EventConstants.FirePistolWeapon)
                .SetAction(Keys.F, EventConstants.LightStatus)
                .SetAction(Keys.R, EventConstants.ReloadWeapon)
                .Build();

            CreatePlayer(
                sprite: player.SpriteName,
                actionBindings: actionBindings1,
                position: maps.spawnPositionTwo, // spawn point,
                viewport: viewports[player.Index],
                characterType: player.CharacterType, cageId: player.CameraId,
                useGamePad: GamePad.GetState(1).IsConnected, gamePadIndex: 1);
        }

        private void InitPlayerThree(Player player)
        {
            var actionBindings = new ActionBindingsBuilder()
                .SetAction(Keys.Up, EventConstants.WalkForward)
                .SetAction(Keys.Down, EventConstants.WalkBackward)
                .SetAction(Keys.Left, EventConstants.TurnLeft)
                .SetAction(Keys.Right, EventConstants.TurnRight)
                .SetAction(Keys.PageDown, EventConstants.FirePistolWeapon)
                .SetAction(Keys.PageUp, EventConstants.TurnAround)
                .SetAction(Keys.RightControl, EventConstants.Running)
                .Build();


            var actionBindings1 = new ActionBindingsBuilder()
                .SetAction(Keys.W, EventConstants.WalkForward) //Use of the next gen constants :)
                .SetAction(Keys.S, EventConstants.WalkBackward)
                .SetAction(Keys.A, EventConstants.TurnLeft)
                .SetAction(Keys.D, EventConstants.TurnRight)
                .SetAction(Keys.Q, EventConstants.TurnAround)
                .SetAction(Keys.E, EventConstants.FirePistolWeapon)
                .SetAction(Keys.F, EventConstants.LightStatus)
                .SetAction(Keys.R, EventConstants.ReloadWeapon)
                .Build();

            CreatePlayer(
                sprite: player.SpriteName,
                actionBindings: actionBindings1,
                position: maps.spawnPositionThree, // spawn point,
                viewport: viewports[player.Index],
                characterType: player.CharacterType, cageId: player.CameraId,
                useGamePad: GamePad.GetState(2).IsConnected, gamePadIndex: 2);
        }

        private void InitPlayerFour(Player player)
        {
            var actionBindings = new ActionBindingsBuilder()
                .SetAction(Keys.Up, EventConstants.WalkForward)
                .SetAction(Keys.Down, EventConstants.WalkBackward)
                .SetAction(Keys.Left, EventConstants.TurnLeft)
                .SetAction(Keys.Right, EventConstants.TurnRight)
                .SetAction(Keys.PageDown, EventConstants.FirePistolWeapon)
                .SetAction(Keys.PageUp, EventConstants.TurnAround)
                .SetAction(Keys.RightControl, EventConstants.Running)
                .Build();

            CreatePlayer(
                sprite: player.SpriteName,
                actionBindings: actionBindings,
                position: maps.spawnPositionFour, // spawn point,
                viewport: viewports[player.Index],
                characterType: player.CharacterType, cageId: player.CameraId);
        }

        //The multitude of options here is for easy debug purposes
        private void CreatePlayer(string sprite, ActionBindings actionBindings, Vector2 position, Viewport viewport,
            CharacterType characterType, int cageId = 0, bool disabled = false, bool useGamePad = false, int gamePadIndex = 0)
        {
            if (disabled) return;

            var light = new Spotlight()
            {
                Position = position,
                Scale = new Vector2(650f),
                Radius = (float)0.0008f,
                Intensity = (float)0.5,
                ShadowType = ShadowType.Solid // Will not lit hulls themselves
            };
            EntityBuilder playerEntity = new EntityBuilder()
                .SetPosition(position, 500)
                .SetRendering(100, 100)
                .SetInertiaDampening()
                .SetBackwardsPenalty()
                .SetSprite(sprite, new Point(1252, 206), 313, 206)
                .SetLight(light)
                .SetSound(soundList: CreateSound(characterType))
                .SetMovement(200, 380, 2f, new Random(DateTime.Now.Millisecond).Next(0, 40) / 10) // Random direction
                .SetRectangleCollision()
                .SetCameraFollow(cageId)
                .SetPlayer(sprite)
                .SetTeam(cageId)
                .SetHealth()
                //.SetCameraView(viewport, 0.5f, cageId)
                .SetScore()
                .SetHUD(false, showStats: true);

            // Add camera view only if there are two teams or if it is the first camera,
            // and the same .
            if (gameViewports.IsTeamOne && gameViewports.IsTeamTwo ||
                gameViewports.IsTeamOne && !gameViewports.IsTeamTwo && cageId == 1 && !createdCamera
                || !gameViewports.IsTeamOne && gameViewports.IsTeamTwo && cageId == 1 && !createdCamera)
            {
                createdCamera = true;
                var cameraView = new CameraViewComponent()
                {
                    CameraId = cageId,
                    View = viewport,
                    MinScale = 0.5f,
                    MaxScale = 1.5f
                };
                ComponentManager.Instance.AddComponentToEntity(cameraView, playerEntity.GetEntityKey());
            }
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

            var cageComponent = ComponentManager.Instance.ComponentFactory.NewComponent<CageComponent>();
            cageComponent.CageId = cageId;
            ComponentManager.Instance.AddComponentToEntity(cageComponent, playerEntity.GetEntityKey());

            var weaponComponent = ComponentManager.Instance.ComponentFactory.NewComponent<WeaponComponent>();
            switch (characterType)
            {
                case CharacterType.Bob:
                    weaponComponent.Damage = 50;
                    weaponComponent.ClipSize = 16;
                    weaponComponent.WeaponType = WeaponComponent.WeaponTypes.Pistol;
                    playerEntity.SetAmmo(weaponComponent.ClipSize, weaponComponent.ClipSize * 14);
                    break;
                case CharacterType.Edgar:
                    weaponComponent.Damage = 15;
                    weaponComponent.ClipSize = 128;
                    weaponComponent.WeaponType = WeaponComponent.WeaponTypes.Rifle;
                    playerEntity.SetAmmo(weaponComponent.ClipSize, weaponComponent.ClipSize * 8);
                    break;
                case CharacterType.Ward:
                    weaponComponent.Damage = 75;
                    weaponComponent.ClipSize = 16;
                    weaponComponent.WeaponType = WeaponComponent.WeaponTypes.Shotgun;
                    playerEntity.SetAmmo(weaponComponent.ClipSize, weaponComponent.ClipSize * 6);
                    break;
                case CharacterType.Jimmy:
                    weaponComponent.Damage = 100;
                    weaponComponent.ClipSize = 16;
                    weaponComponent.WeaponType = WeaponComponent.WeaponTypes.Pistol;
                    playerEntity.SetAmmo(weaponComponent.ClipSize, weaponComponent.ClipSize * 8);
                    break;
            }

            var barrelFlash = new BarrelFlashComponent
            {
                Light = new PointLight
                {
                    ShadowType = ShadowType.Solid,
                    Radius = 0.008f,
                    Intensity = 0.4f,
                    Scale = new Vector2(1800f),
                    Enabled = false,
                    Color = Color.AntiqueWhite
                }
            };

            playerEntity.Build();
            ComponentManager.Instance.AddComponentToEntity(weaponComponent, playerEntity.GetEntityKey());
            ComponentManager.Instance.AddComponentToEntity(barrelFlash, playerEntity.GetEntityKey());
            if (useGamePad)
            {
                var gamePadComponent = ComponentManager.Instance.ComponentFactory.NewComponent<GamePadComponent>();
                gamePadComponent.GamePadPlayerIndex = gamePadIndex;
                ComponentManager.Instance.AddComponentToEntity(gamePadComponent, playerEntity.GetEntityKey());
            }
        }


        private Dictionary<SoundComponent.SoundBank, SoundEffectInstance> CreateSound(CharacterType type)
        {
            Dictionary<SoundComponent.SoundBank, SoundEffectInstance> soundList = new Dictionary<SoundComponent.SoundBank, SoundEffectInstance>(5);

            soundList.Add(SoundComponent.SoundBank.EmptyMag, OutlivedGame.Instance()
                .Content.Load<SoundEffect>("Sound/Weapon/EmptyMag")
                .CreateInstance());
            soundList.Add(SoundComponent.SoundBank.Death, OutlivedGame.Instance()
                .Content.Load<SoundEffect>("Sound/Splash")
                .CreateInstance());

            var soundEffectInstance = OutlivedGame.Instance()
                .Content.Load<SoundEffect>("Sound/Weapon/Reload")
                .CreateInstance();
            soundEffectInstance.Volume = 0.7f;
            soundList.Add(SoundComponent.SoundBank.Reload, soundEffectInstance);


            return soundList;
        }
    }
}