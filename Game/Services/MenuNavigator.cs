namespace Game.Services
{
    public class MenuNavigator
    {
        private GameManager GameManager { get; }

        public MenuNavigator(GameManager gameManager)
        {
            GameManager = gameManager;
        }
        
        public void GoBack()
        {
            GameManager.CurrentGameState = GameManager.PreviousGameState;
        }

        public void GoTo(GameManager.GameState newState)
        {
            GameManager.SetCurrentState(newState);
        }

        public void Pause()
        {
            if (GameManager.CurrentGameState == GameManager.GameState.Paused)
            {
                GameManager.CurrentGameState = GameManager.PreviousGameState;
            }
            else
            {
                GameManager.PreviousGameState = GameManager.CurrentGameState;
                GameManager.CurrentGameState = GameManager.GameState.Paused;
            }
        }
    }
}