using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Game.Entities;
using Game.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using ZEngine.Wrappers;
using static Game.Menu.States.MultiplayerMenu.TeamState;
using static Game.Services.VirtualGamePad.MenuKeys;
using static Game.Services.VirtualGamePad.MenuKeyStates;

namespace Game.Menu.States
{
    public class MultiplayerMenu : IMenu, ILifecycle
    {
        public MenuNavigator MenuNavigator { get; }
        public PlayerControllers PlayerInputCollection { get; }

        // Dependencies
        private readonly Microsoft.Xna.Framework.Game game;
        private readonly GameManager gm;

        // enum so we can keep track on which option
        // we currently are at.
        public enum TeamState
        {
            TeamOne,
            NoTeam,
            TeamTwo,
        }

        private readonly TeamState[] _optionStates = { TeamOne, NoTeam, TeamTwo };
        private readonly OptionNavigator<TeamState>[] _playerChoices;

        public MultiplayerMenu(GameManager gameManager, MenuNavigator menuNavigator,
            PlayerControllers playerInputCollection)
        {
            MenuNavigator = menuNavigator;
            this.gm = gameManager;
            game = OutlivedGame.Instance();
            PlayerInputCollection = playerInputCollection;

            var playerOneChoice = new OptionNavigator<TeamState>(_optionStates, horizontalNavigation: true);
            var playerTwoChoice = new OptionNavigator<TeamState>(_optionStates, horizontalNavigation: true);
            var playerThreeChoice = new OptionNavigator<TeamState>(_optionStates, horizontalNavigation: true);
            var playerFourChoice = new OptionNavigator<TeamState>(_optionStates, horizontalNavigation: true);

            _playerChoices = new[]
            {
                playerOneChoice, playerTwoChoice, playerThreeChoice, playerFourChoice
            };

            for (var i = 0; i < _playerChoices.Length; i++)
            {
                _playerChoices[i].CurrentIndex = 1; // Set start position to second choice "NoTeam"
                _playerChoices[i].UpdatePosition(PlayerInputCollection.Controllers[i]);
            }
        }


        private void DisplayCurrentOption(TeamState choice, float heightPercentage, SpriteBatch sb)
        {
            var viewport = game.GraphicsDevice.Viewport;
            sb.Draw(AssetManager.Instance.Get<Texture2D>("Images/Menu/teamoptions")
                , viewport.Bounds, Color.White);
            switch (choice)
            {
                case NoTeam:
                    sb.Draw(AssetManager.Instance.Get<Texture2D>("Images/Gamepad/gamepad"),
                        new Vector2((float) (viewport.Width * 0.4), viewport.Height * heightPercentage), Color.White);
                    break;
                case TeamOne:
                    sb.Draw(AssetManager.Instance.Get<Texture2D>("Images/Gamepad/gamepad_h"),
                        new Vector2((float) (viewport.Width * 0.2), viewport.Height * heightPercentage), Color.White);
                    break;
                case TeamTwo:
                    sb.Draw(AssetManager.Instance.Get<Texture2D>("Images/Gamepad/gamepad_h"),
                        new Vector2((float) (viewport.Width * 0.6), viewport.Height * heightPercentage), Color.White);
                    break;
            }
        }

        // Here is all the drawing called for this
        // class, so if some drawing isn't in here
        // then it won't be drawn.
        public void Draw(GameTime gameTime, SpriteBatch sb)
        {
            sb.Begin();
            gm.effects.DrawExpandingEffect(sb, AssetManager.Instance.Get<Texture2D>("Images/Menu/background3"));

            var heightPercentage = 0.2f;
            foreach (var playerChoice in _playerChoices)
            {
                DisplayCurrentOption(playerChoice.CurrentPosition, heightPercentage, sb);
                heightPercentage += 0.2f;
            }

            sb.End();
        }

        public void Update(GameTime gameTime)
        {
            if (PlayerInputCollection.Controllers.Any(c => c.Is(Cancel, Pressed)))
            {
                     MenuNavigator.GoBack();
            }

            for (var i = 0; i < _playerChoices.Length; i++)
            {
                _playerChoices[i].UpdatePosition(PlayerInputCollection.Controllers[i]);
            }

            if (PlayerInputCollection.PlayerOne().Is(Accept, Pressed))
            {
                var somePlayerHasTeam = _playerChoices.Any(player => player.CurrentPosition != NoTeam);
                if (somePlayerHasTeam)
                {
                    AssetManager.Instance.Get<SoundEffect>("sound/click2").Play();
                    UpdateGameConfigurations();
                    MenuNavigator.GoTo(OutlivedStates.GameState.CharacterMenu);
                }
            }
        }

        private Dictionary<int, PlayerIndex> IntegerToPlayerIndex = new Dictionary<int, PlayerIndex>
        {
            {0, PlayerIndex.One},
            {1, PlayerIndex.Two},
            {2, PlayerIndex.Three},
            {3, PlayerIndex.Four}
        };

        private void UpdateGameConfigurations()
        {
            // Clear before each game..
            gm.gameConfig.Players.Clear();
            for (var i = 0; i < _playerChoices.Length; i++)
            {
                if (_playerChoices[i].CurrentPosition == NoTeam) continue;
                gm.gameConfig.Players.Add(new Player
                {
                    Index = IntegerToPlayerIndex[i],
                    Team = _playerChoices[i].CurrentPosition
                });
            }
        }

        public void ResetPlayerChoicesState()
        {
            foreach (var playerChoice in _playerChoices)
            {
                playerChoice.CurrentPosition = playerChoice.Options[0];
            }
        }

        public void Reset()
        {
        }

        public void BeforeShow()
        {
            ResetPlayerChoicesState();
            
            for (var i = 0; i < _playerChoices.Length; i++)
            {
                _playerChoices[i].CurrentIndex = 1; // Set start position to second choice "NoTeam"
                _playerChoices[i].UpdatePosition(PlayerInputCollection.Controllers[i]);
            }
        }

        public void BeforeHide()
        {
        }
    }
}