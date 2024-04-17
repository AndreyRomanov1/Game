using UnityEngine;

public static class Model
{
    public const int BlockWidth = 64;
    public const int BlockHeight = 48;


    public static GameScript Game;
    public static GameObject LevelSelectionCanvasGameObject;

    // Сколько блоков было пройдено всего, максимальная длинна в блоках забега, список оружий игрока, флаг пройдено ли обучение, различная статистика
    // Старт игры
    public static void StartGame()
    {
        ShowLevelSelectionUI();
    }

    // Вывести выбор уровней. Камера
    public static void ShowLevelSelectionUI()
    {
        LevelSelectionCanvasGameObject = Game.CreateByGameObject(Game.LoadByName("LevelSelectionCanvas"));
        // var LevelSelectionCanvas = LevelSelectionCanvasGameObject.GetComponent<LevelSelectionCanvasScript>();
    }

    // Запустить уровень. CurrentGame - экземпляр текущей игры.
    public static void StartNewGame(string pathToLevelBlocks)
    {
        Game.Destroy(Camera.main.gameObject);
        Game.Destroy(LevelSelectionCanvasGameObject);
        CurrentGame.ResetGame(pathToLevelBlocks);
    }
}