public static class Model
{
    public static GameScript Game;

    public static CurrentGameScript CurrentGame;

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

    public static bool IsEducation;

    public static bool IsActiveGame => GameState == GameStates.ActiveGame;

    public static void StartGame()
    {
        GameState = GameStates.StartGameMenu;
        UI.MainCanvas.Init();
        UI.Show(UI.LevelSelectionGameObject);
        Game.StartCoroutine(Pause.PauseCoroutine());
        Game.StartCoroutine(TimeController.TimeSlowerCoroutine());
    }

    public static void StartCurrentGame(string pathToLevelBlocks)
    {
        CurrentGame.KillCurrentGame();
        CurrentGame = new CurrentGameScript(pathToLevelBlocks);
    }
}