using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using Game.Menu;
using static Game.GameManager;

namespace Game.Services
{
    public class MenuNavigator
    {
        public Dictionary<GameState, ILifecycle> LifecycleStates { get; set; }
        public Dictionary<GameState, IMenu> GameStateMenuMap { get; set; }
        private GameManager GameManager { get; }
        private Stack<GameState> History { get; } = new Stack<GameState>();

        public MenuNavigator(GameManager gameManager)
        {
            GameManager = gameManager;
        }
        
        public void GoBack()
        {
            GameManager.CurrentGameState = History.Pop();
        }

        public void GoTo(GameState newState)
        {
            var currentGameState = GameManager.CurrentGameState;
            if (LifecycleStates.ContainsKey(currentGameState))
            {
                LifecycleStates[currentGameState].BeforeHide();
            }
            if (LifecycleStates.ContainsKey(newState))
            {
                LifecycleStates[newState].BeforeShow();
            }

            History.Push(currentGameState);
            GameManager.SetCurrentState(newState);
        }

        public void Pause()
        {
            if (GameManager.CurrentGameState == GameState.Paused)
            {
                GameManager.SetCurrentState(History.Pop());
            }
            else
            {
                History.Push(GameManager.CurrentGameState);
                GameManager.SetCurrentState(GameState.Paused);
            }
        }
    }
}