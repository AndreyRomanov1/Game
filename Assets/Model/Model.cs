public static class Model
{
    public static GameScript Game;

    private static GameStates gameState;
    public static GameStates GameState
    {
        get => gameState;
        set
        {
            TimeController.ChangeGameState(gameState, value);
            gameState = value;
        }
    }

    public static bool IsActiveGame => GameState == GameStates.ActiveGame;

    public static void StartGame()
    {
        GameState = GameStates.StartGameMenu;
        UI.MainCanvas.Init();
        UI.Show(UI.LevelSelectionGameObject);
        Game.StartCoroutine(Pause.PauseCoroutine());
        Game.StartCoroutine(TimeController.TimeSlowerCoroutine());
    }
}