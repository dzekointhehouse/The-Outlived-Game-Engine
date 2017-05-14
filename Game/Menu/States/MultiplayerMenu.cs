using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Controllers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ZEngine.Wrappers;

namespace Game.Menu.States
{
    class MultiplayerMenu : IMenu
    {
        // Dependencies
        private readonly Microsoft.Xna.Framework.Game game;
        private readonly GameManager gameManager;
        private readonly ControlsConfig controls;
        private SpriteBatch sb = GameDependencies.Instance.SpriteBatch;

        // players current position
        private OptionsState PlayerOneChoice = OptionsState.NoTeam;
        private OptionsState PlayerTwoChoice = OptionsState.NoTeam;
        private OptionsState PlayerThreeChoice = OptionsState.NoTeam;
        private OptionsState PlayerFourChoice = OptionsState.NoTeam;

        // used for the keyboard.
        private PlayerIndex currentPlayer;


        // enum so we can keep track on which option
        // we currently are at.
        private enum OptionsState
        {
            TeamOne,
            NoTeam,
            TeamTwo,
        }

        public MultiplayerMenu(GameManager gameManager)
        {
            this.gameManager = gameManager;
            game = this.gameManager.Engine.Dependencies.Game;
            // Adding the options interval and gamemanager.
            controls = new ControlsConfig(0, 2, gameManager);
        }

        // Draws the character names and the button at the option that
        // is the current option that we are positioned at.
        private void DisplayPlayerChoice(OptionsState playerChoice, float heightPercentage)
        {
            var viewport = game.GraphicsDevice.Viewport;
            sb.Draw(gameManager.GameContent.TeamOptions, viewport.Bounds, Color.White);
            switch (playerChoice)
            {
                case OptionsState.NoTeam:
                    sb.Draw(gameManager.GameContent.GamePadIcon, new Vector2((float) (viewport.Width * 0.4), viewport.Height * heightPercentage), Color.White);
                    break;
                case OptionsState.TeamOne:
                    sb.Draw(gameManager.GameContent.GamePadIconHighlight, new Vector2((float)(viewport.Width * 0.2), viewport.Height * heightPercentage), Color.White);
                    break;
                case OptionsState.TeamTwo:
                    sb.Draw(gameManager.GameContent.GamePadIconHighlight, new Vector2((float)(viewport.Width * 0.6), viewport.Height * heightPercentage), Color.White);
                    break;

            }
        }

        // Here is all the drawing called for this
        // class, so if some drawing isn't in here
        // then it won't be drawn.
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            // We check if the players are connected. If they are,
            // we draw their option state on the screen.
            //if(GamePad.GetState(PlayerIndex.One).IsConnected)
                DisplayPlayerChoice(PlayerOneChoice, 0.2f);
            //if (GamePad.GetState(PlayerIndex.Two).IsConnected)
                DisplayPlayerChoice(PlayerTwoChoice, 0.4f);
            //if (GamePad.GetState(PlayerIndex.Three).IsConnected)
                DisplayPlayerChoice(PlayerThreeChoice, 0.6f);
            //if (GamePad.GetState(PlayerIndex.Four).IsConnected)
                DisplayPlayerChoice(PlayerFourChoice, 0.8f);



        }

        // The update method for this class
        // that takes care of all the updates, that
        // are to be done.
        public void Update(GameTime gameTime)
        {
            // If doing it with the keyboard.
            // state interval for n player's not the
            // same as for the 3 options states, So
            // we set it to 3 (four players).
            controls.MaxLimit = 3;
            currentPlayer = (PlayerIndex)controls.MoveOptionPositionVertically((int)currentPlayer);
            // Reset
            controls.MaxLimit = 2;
            switch (currentPlayer)
            {
                case PlayerIndex.One:
                    PlayerOneChoice = (OptionsState)controls.MoveOptionPositionHorizontally((int)PlayerOneChoice, PlayerIndex.One);
                    break;
                case PlayerIndex.Two:
                    PlayerTwoChoice = (OptionsState)controls.MoveOptionPositionHorizontally((int)PlayerTwoChoice, PlayerIndex.Two);
                    break;
                case PlayerIndex.Three:
                    PlayerThreeChoice = (OptionsState)controls.MoveOptionPositionHorizontally((int)PlayerThreeChoice, PlayerIndex.Three);
                    break;
                case PlayerIndex.Four:
                    PlayerFourChoice = (OptionsState)controls.MoveOptionPositionHorizontally((int)PlayerFourChoice, PlayerIndex.Four);
                    break;

            }
            
            // which player does the move
            PlayerOneChoice = (OptionsState)controls.MoveOptionPositionHorizontally((int)PlayerOneChoice);
            PlayerTwoChoice = (OptionsState)controls.MoveOptionPositionHorizontally((int)PlayerTwoChoice);
            PlayerThreeChoice = (OptionsState)controls.MoveOptionPositionHorizontally((int)PlayerThreeChoice);
            PlayerFourChoice = (OptionsState)controls.MoveOptionPositionHorizontally((int)PlayerFourChoice);

            // Proceed if continue is pressed
            controls.ContinueButton(GameManager.GameState.CharacterMenu);
            UpdateGameConfigurations();
        }

        private void UpdateGameConfigurations()
        {
            // Add players on option team one -> to team one
            if(PlayerOneChoice == OptionsState.TeamOne)
                gameManager.gameConfig.TeamOne.Add(PlayerIndex.One);
            if (PlayerTwoChoice == OptionsState.TeamOne)
                gameManager.gameConfig.TeamOne.Add(PlayerIndex.Two);
            if (PlayerThreeChoice == OptionsState.TeamOne)
                gameManager.gameConfig.TeamOne.Add(PlayerIndex.Three);
            if (PlayerFourChoice == OptionsState.TeamOne)
                gameManager.gameConfig.TeamOne.Add(PlayerIndex.Four);

            // Add players on option team two -> to team two
            if (PlayerOneChoice == OptionsState.TeamTwo)
                gameManager.gameConfig.TeamTwo.Add(PlayerIndex.One);
            if (PlayerTwoChoice == OptionsState.TeamTwo)
                gameManager.gameConfig.TeamTwo.Add(PlayerIndex.Two);
            if (PlayerThreeChoice == OptionsState.TeamTwo)
                gameManager.gameConfig.TeamTwo.Add(PlayerIndex.Three);
            if (PlayerFourChoice == OptionsState.TeamTwo)
                gameManager.gameConfig.TeamTwo.Add(PlayerIndex.Four);
        }
    }
}
