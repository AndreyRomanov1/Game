using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Model
{
    public const int BlockWidth = 64;
    public const int BlockHeight = 48;

    public static Camera UICamera;
    public static GameScript Game;
    public static LevelSelectionCanvasScript LevelSelectionCanvas;
    public static GameState GameState;
    public static readonly List<GameObject> Clouds = new();

    // Сколько блоков было пройдено всего, максимальная длинна в блоках забега, список оружий игрока, флаг пройдено ли обучение, различная статистика

    public static void StartGame()
    {
        UICamera = Game.CreateByGameObject(Game.LoadByName("Cameras/UICamera")).GetComponent<Camera>();
        GameState = GameState.StartGameMenu;

        LoadClouds();        
        ShowLevelSelectionUI();
    }

    private static void ShowLevelSelectionUI()
    {
        UICamera.gameObject.SetActive(true);
        LevelSelectionCanvas = Game.CreateByGameObject(Game.LoadByName("LevelSelectionCanvas")).GetComponent<LevelSelectionCanvasScript>();
    }

    public static void StartNewGame(string pathToLevelBlocks)
    {
        UICamera.gameObject.SetActive(false);
        CurrentGame.ResetGame(pathToLevelBlocks);
    }

    private static void LoadClouds()
    {
        var test = Game.LoadByName("clouds/TestCloud");
        Clouds.Add(test);
    }
}