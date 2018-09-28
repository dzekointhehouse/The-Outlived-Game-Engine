using System.Collections.Generic;
using Game.Menu.States;
using Game.Menu.States.GameModes;
using Game.Menu.States.GameModes.DeathMatch;
using Game.Menu.States.GameModes.Extinction;
using Game.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using TheOutlivedGL;
using ZEngine.Systems;
using static Game.Menu.OutlivedStates;

namespace Game.Menu
{
    public class GameManager
    {
        public OutlivedContent OutlivedContent { get; }

        // Here we just say that the first state is the Intro
        protected internal GameState CurrentGameState = GameState.Intro;

        protected internal GameState PreviousGameState;
        //        protected internal KeyboardState OldKeyboardState;
        //        protected internal GamePadState OldGamepadState;

        public VirtualGamePad Controller { get; set; }
        protected internal MenuNavigator MenuNavigator { get; set; }

        //protected internal GameEngine Engine;
        protected internal Viewport viewport;
        protected internal SpriteBatch spriteBatch;
        protected internal BackgroundEffects effects;
        protected internal Microsoft.Xna.Framework.Game game;
        protected internal PlayerControllers playerControllers;

        // To keep track of the game configurations made
        protected internal GameConfig gameConfig;

        private IMenu mainMenu;
        private IMenu gameModesMenu;
        private IMenu characterMenu;
        private IMenu credits;
        private IMenu gameIntro;
        private IMenu survivalGame;
        private IMenu pausedMenu;
        private IMenu aboutMenu;
        private IMenu multiplayerMenu;
        private IMenu gameOver;
        private IMenu gameOverCredits;

        public Dictionary<GameState, IMenu> GameStateMenuMap;

        public void SetCurrentState(GameState state)
        {
            CurrentGameState = state;
        }

        public GameManager()
        {
            spriteBatch = OutlivedGame.Instance().spriteBatch;
            game = OutlivedGame.Instance();
            viewport = OutlivedGame.Instance().graphics.GraphicsDevice.Viewport;
            
            effects = new BackgroundEffects(viewport);
            gameConfig = new GameConfig();

            playerControllers = new PlayerControllers(new[]
            {
                new VirtualGamePad(PlayerIndex.One),
                new VirtualGamePad(PlayerIndex.Two),
                new VirtualGamePad(PlayerIndex.Three),
                new VirtualGamePad(PlayerIndex.Four)
            });
            
            MenuNavigator = new MenuNavigator(this);

            var gameModeDependencies = new GameModeDependencies()
            {
                GameConfig = gameConfig,
                MenuNavigator = MenuNavigator,
               // GameSystems = Engine,
                Viewport = viewport,
                VirtualInputs = playerControllers
            };


            // initializing the states, remember:
            // all the states need to exist in the 
            // manager.
            mainMenu = new MainMenu(this);
            gameModesMenu = new GameModeMenu(this);
            characterMenu = new CharacterMenu(this);
            credits = new Credits(this);
            gameIntro = new GameIntro(this);
            pausedMenu = new PausedMenu(this);
            multiplayerMenu = new MultiplayerMenu(this);
            aboutMenu = new AboutMenu(this);
            gameOver = new GameOver(this);
            gameOverCredits = new GameOverCredits(this);

            var gameModeSurvival = new Survival(gameModeDependencies);
            var deathMatch = new DeathMatch(gameModeDependencies);
            var extinction = new Extinction(gameModeDependencies);

            GameStateMenuMap = new Dictionary<GameState, IMenu>
            {
                {GameState.Intro, gameIntro},
                {GameState.MainMenu, mainMenu},
//                {GameState.SurvivalGame, survivalGame},
                {GameState.SurvivalGame, gameModeSurvival},
                {GameState.PlayDeathMatchGame, deathMatch},
                {GameState.PlayExtinctionGame, extinction},
                {GameState.Quit, mainMenu},
                {GameState.GameModesMenu, gameModesMenu},
                {GameState.CharacterMenu, characterMenu},
                {GameState.Credits, credits},
                {GameState.Paused, pausedMenu},
                {GameState.MultiplayerMenu, multiplayerMenu},
                {GameState.About, aboutMenu},
                {GameState.GameOver, gameOver},
                {GameState.GameOverCredits,  gameOverCredits}
            };
            var lifecycleStates = new Dictionary<GameState, ILifecycle>
            {
                {GameState.SurvivalGame, gameModeSurvival},
                {GameState.PlayDeathMatchGame, deathMatch},
                {GameState.PlayExtinctionGame, extinction},
                {GameState.MultiplayerMenu, (ILifecycle) multiplayerMenu},
                {GameState.CharacterMenu, (ILifecycle) characterMenu},
                {GameState.GameOver, (ILifecycle) gameOver}
            };

            MenuNavigator.GameStateMenuMap = GameStateMenuMap;
            MenuNavigator.MenuStates = lifecycleStates;
        }

        // Draw method consists of a switch case with all
        // the different states that we have, depending on which
        // state we are we use that state's draw method.
        public void Draw(GameTime gameTime)
        {
            if (CurrentGameState == GameState.Paused)
            {
                OutlivedGame.Instance().GraphicsDevice.Viewport = viewport;
            }

            if (CurrentGameState == GameState.Quit)
            {
                OutlivedGame.Instance().Exit();
            }
            else if (GameStateMenuMap.ContainsKey(CurrentGameState))
            {
                GameStateMenuMap[CurrentGameState].Draw(gameTime, spriteBatch);
            }
        }

        // Same as the draw method, the update method
        // we execute is the one of the current state.
        public void Update(GameTime gameTime)
        {
            foreach (var virtualGamePad in playerControllers.Controllers)
            {
                virtualGamePad.UpdateControllerStatus();
                virtualGamePad.UpdateKeyboardState();
            }

            if (CurrentGameState == GameState.Paused)
            {
                OutlivedGame.Instance().GraphicsDevice.Viewport = viewport;
            }

            if (CurrentGameState == GameState.Quit)
            {
                OutlivedGame.Instance().Exit();
            }
            else if (GameStateMenuMap.ContainsKey(CurrentGameState))
            {
                GameStateMenuMap[CurrentGameState].Update(gameTime);
            }

            foreach (var virtualGamePad in playerControllers.Controllers)
            {
                virtualGamePad.MoveCurrentStatesToOld();
            }
        }
    }
}