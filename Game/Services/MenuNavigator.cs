using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using Game.Menu;
using static Game.GameManager;

namespace Game.Services
{
    public class MenuNavigator
    {
        public Dictionary<GameState, ILifecycle> MenuStates { get; set; }
        public Dictionary<GameState, IMenu> GameStateMenuMap { get; set; }
        private GameManager GameManager { get; }
        private Stack<GameState> OldStates { get; } = new Stack<GameState>(10);

        public MenuNavigator(GameManager gameManager)
        {
            GameManager = gameManager;
        }
        
        public void GoBack()
        {
            GameManager.MenuContent.ClickSound.Play();
            GameManager.CurrentGameState = OldStates.Pop();
        }

        public void GoTo(GameState newState)
        {
            var currentGameState = GameManager.CurrentGameState;
            if (MenuStates.ContainsKey(currentGameState))
            {
                MenuStates[currentGameState].BeforeHide();
            }
            if (MenuStates.ContainsKey(newState))
            {
                MenuStates[newState].BeforeShow();
            }

            OldStates.Push(currentGameState);
            GameManager.SetCurrentState(newState);
        }

        public void Pause()
        {
            if (GameManager.CurrentGameState == GameState.Paused)
            {
                GameManager.SetCurrentState(OldStates.Pop());
            }
            else
            {
                OldStates.Push(GameManager.CurrentGameState);
                GameManager.SetCurrentState(GameState.Paused);
            }
        }

    }
}