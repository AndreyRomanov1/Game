using UnityEngine;

public static class Model
{
    public const int BlockWidth = 64;
    public const int BlockHeight = 48;

    public static GameScript Game;
    public static GameState GameState;
    public static bool IsActiveGame => GameState == GameState.ActiveGame;

    // Сколько блоков было пройдено всего, максимальная длинна в блоках забега, список оружий игрока, флаг пройдено ли обучение, различная статистика

    public static void StartGame()
    {
        GameState = GameState.StartGameMenu;
        LevelSelectionUIController.Show();
        Game.StartCoroutine(Pause.PauseCoroutine());
    }

    public static void StartNewGame(string pathToLevelBlocks)
    {
        CurrentGame.ResetGame(pathToLevelBlocks);
    }
}