using System.Collections.Generic;
using UnityEngine;

public static class CurrentGame
{
    public static GridScript Grid;
    public static PlayerScript Player;

    public static Camera PlayerCamera;

    public static float GameSpeed => IsSlowGame ? 0.01f : 1f;

    public static Dictionary<SpeakersEnum, ISpeakingCharacter> EnumToSpeaker;

    public static bool IsSlowGame => Player.PlayerState is
        PlayerStates.CrouchedToJump
        or PlayerStates.CrouchedToJumpFromLeftWall
        or PlayerStates.CrouchedToJumpFromRightWall;
    // Здоровье, сколько прошёл блоков от начала забега. Список сгенерированных блоков 

    public static void ResetGame(string pathToLevelBlocks)
    {
        Model.GameState = GameState.ActiveGame;

        Grid = GameScript.CreateGrid().GetComponent<GridScript>();
        Player = GameScript.CreatePlayer().GetComponent<PlayerScript>();
        PlayerCamera = Player.GetComponentInChildren<Camera>();
        Grid.InitGrid(pathToLevelBlocks);
        EnumToSpeaker = new Dictionary<SpeakersEnum, ISpeakingCharacter>
        {
            [SpeakersEnum.Player] = Player,
            [SpeakersEnum.GreatCornEar] = Player
        };
        Model.Game.StartCoroutine(Dialogues.DialoguesCoroutine());

    }

    public static void KillGame()
    {
        Object.Destroy(Player.gameObject);
        Object.Destroy(Grid.gameObject);
    }
}