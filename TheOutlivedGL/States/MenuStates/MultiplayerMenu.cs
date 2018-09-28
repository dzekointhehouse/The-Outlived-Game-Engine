using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Game.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using TheOutlivedGL;
using static Game.Menu.States.MultiplayerMenu.TeamStates;
using static Game.Services.VirtualGamePad.MenuKeys;
using static Game.Services.VirtualGamePad.MenuKeyStates;

namespace Game.Menu.States
{
    public partial class MultiplayerMenu : IMenu, ILifecycle
    {
        private readonly Microsoft.Xna.Framework.Game game;
        private readonly GameManager gameManager;
        private TeamStates[] _optionStates;
        private OptionNavigator<TeamStates>[] _playerChoices;

        public MultiplayerMenu(GameManager gameManager)
        {
            this.gameManager = gameManager;
            game = OutlivedGame.Instance();
        }

        private void DisplayCurrentOption(TeamStates choice, float heightPercentage, SpriteBatch sb)
        {
            var viewport = gameManager.viewport;
            sb.Draw(AssetManager.Instance.Get<Texture2D>("Images/Menu/teamoptions"), viewport.Bounds, Color.White);

            switch (choice)
            {
                case NoTeam:
                    sb.Draw(AssetManager.Instance.Get<Texture2D>("Images/Gamepad/gamepad"),
                        new Vector2((float)(viewport.Width * 0.4), viewport.Height * heightPercentage), Color.White);
                    break;
                case TeamOne:
                    sb.Draw(AssetManager.Instance.Get<Texture2D>("Images/Gamepad/gamepad_h"),
                        new Vector2((float)(viewport.Width * 0.2), viewport.Height * heightPercentage), Color.White);
                    break;
                case TeamTwo:
                    sb.Draw(AssetManager.Instance.Get<Texture2D>("Images/Gamepad/gamepad_h"),
                        new Vector2((float)(viewport.Width * 0.6), viewport.Height * heightPercentage), Color.White);
                    break;
            }
        }

        // Here is all the drawing called for this
        // class, so if some drawing isn't in here
        // then it won't be drawn.
        public void Draw(GameTime gameTime, SpriteBatch sb)
        {
            sb.Begin();
            gameManager.effects.DrawExpandingEffect(sb, AssetManager.Instance.Get<Texture2D>("Images/Menu/background3"));

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
            if (gameManager.playerControllers.Controllers.Any(c => c.Is(Cancel, Pressed)))
                gameManager.MenuNavigator.GoBack();

            for (var i = 0; i < _playerChoices.Length; i++)
            {
                _playerChoices[i].UpdatePosition(gameManager.playerControllers.Controllers[i]);
            }

            if (gameManager.playerControllers.PlayerOne().Is(Accept, Pressed))
            {
                var somePlayerHasTeam = _playerChoices.Any(player => player.CurrentPosition != NoTeam);
                var separateTeamsIfDeathmatch = true;

                if (gameManager.gameConfig.GameMode == OutlivedStates.GameState.PlayDeathMatchGame)
                    separateTeamsIfDeathmatch = _playerChoices.Any(player => player.CurrentPosition == TeamOne)
                        && _playerChoices.Any(player => player.CurrentPosition == TeamTwo);

                if (somePlayerHasTeam && separateTeamsIfDeathmatch)
                {
                    AssetManager.Instance.Get<SoundEffect>("sound/click2").Play();
                    SetGameConfigurations();
                    gameManager.MenuNavigator.GoTo(OutlivedStates.GameState.CharacterMenu);
                }
            }
        }

        private void SetGameConfigurations()
        {
            // Clear before each game..
            gameManager.gameConfig.Players.Clear();

            for (var i = 0; i < _playerChoices.Length; i++)
            {
                if (_playerChoices[i].CurrentPosition == NoTeam) continue;

                gameManager.gameConfig.Players.Add(new Player
                {
                    Index = (PlayerIndex)i,
                    Team = _playerChoices[i].CurrentPosition
                });
            }
        }

        public void Reset()
        {
        }

        public void BeforeShow()
        {
            if(gameManager.gameConfig.GameMode == OutlivedStates.GameState.PlayDeathMatchGame)
                _optionStates = new TeamStates[]{ TeamOne, NoTeam, TeamTwo };
            else
                _optionStates = new TeamStates[] { TeamOne, NoTeam };

            var playerOneChoice = new OptionNavigator<TeamStates>(_optionStates, NoTeam, horizontalNavigation: true);
            var playerTwoChoice = new OptionNavigator<TeamStates>(_optionStates, NoTeam, horizontalNavigation: true);
            var playerThreeChoice = new OptionNavigator<TeamStates>(_optionStates, NoTeam, horizontalNavigation: true);
            var playerFourChoice = new OptionNavigator<TeamStates>(_optionStates, NoTeam, horizontalNavigation: true);

            _playerChoices = new[] { playerOneChoice, playerTwoChoice, playerThreeChoice, playerFourChoice };
        }

        public void BeforeHide()
        {
        }
    }
}