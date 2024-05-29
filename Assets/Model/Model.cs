﻿using UnityEngine;

public static class Model
{
    public const int BlockWidth = 64;
    public const int BlockHeight = 48;

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

    // Сколько блоков было пройдено всего, максимальная длинна в блоках забега, список оружий игрока, флаг пройдено ли обучение, различная статистика

    public static void StartGame()
    {
        GameState = GameStates.StartGameMenu;
        LevelSelectionUIController.Show();
        Game.StartCoroutine(Pause.PauseCoroutine());
        Game.StartCoroutine(TimeController.TimeSlowerCoroutine());
    }

    public static void StartNewGame(string pathToLevelBlocks)
    {
        CurrentGame.ResetGame(pathToLevelBlocks);
    }
}