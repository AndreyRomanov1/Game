using System.Collections.Generic;
using UnityEngine;

public static class CurrentGame
{
    public static GridScript Grid;
    public static PlayerScript Player;

    public static Camera PlayerCamera;
    

    public static Dictionary<SpeakersEnum, ISpeakingCharacter> EnumToSpeaker;
    
    // Здоровье, сколько прошёл блоков от начала забега. Список сгенерированных блоков 

    public static void StartCurrentGame(string pathToLevelBlocks)
    {
        KillCurrentGame();
        Model.GameState = GameStates.ActiveGame;

        Grid = GameScript.CreateGrid().GetComponent<GridScript>();
        Player = GameScript.CreatePlayer().GetComponent<PlayerScript>();
        PlayerCamera = Player.GetComponentInChildren<Camera>();
        Grid.InitGrid(pathToLevelBlocks);
    }

    public static void KillCurrentGame()
    {
        if (Player is null || Grid is null)
            return;
        Object.Destroy(Player.gameObject);
        Object.Destroy(Grid.gameObject);
    }
}