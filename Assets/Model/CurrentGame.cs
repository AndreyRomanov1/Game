using System.Collections.Generic;
using UnityEngine;

public static class CurrentGame
{
    private static readonly GameObject CurrentGamePrefab = GameScript.LoadByName("CurrentGame");
    public static GameObject CurrentGameObject;
    public static PlayerScript Player;
    public static Camera PlayerCamera;

    public static Dictionary<SpeakersEnum, ISpeakingCharacter> EnumToSpeaker;

    public static void StartCurrentGame(string pathToLevelBlocks)
    {
        KillCurrentGame();
        Model.IsEducation = pathToLevelBlocks == "StartBlocks";

        CurrentGameObject = GameScript.CreateByGameObject(CurrentGamePrefab, Model.Game.gameObject);

        Player = GameScript.CreatePlayer().GetComponent<PlayerScript>();
        PlayerCamera = Player.GetComponentInChildren<Camera>();
        GameScript.CreateGrid().GetComponent<GridScript>().InitGrid(pathToLevelBlocks);
        Model.GameState = GameStates.ActiveGame;
    }

    public static void KillCurrentGame()
    {
        if (CurrentGameObject != null)
            Object.Destroy(CurrentGameObject);
    }
}