using System.Collections.Generic;
using UnityEngine;

public class CurrentGameScript : MonoBehaviour
{
    public readonly GameObject CurrentGameObject;
    public readonly PlayerScript Player;
    public readonly Camera PlayerCamera;

    public Dictionary<SpeakersEnum, ISpeakingCharacter> EnumToSpeaker;

    public CurrentGameScript(string pathToLevelBlocks)
    {
        Model.IsEducation = pathToLevelBlocks == "StartBlocks";

        CurrentGameObject = GameScript.CreateByGameObject(GameScript.LoadByName("CurrentGame"), Model.Game.gameObject);

        Player = GameScript.CreatePlayer().GetComponent<PlayerScript>();
        PlayerCamera = Player.GetComponentInChildren<Camera>();
        GameScript.CreateGrid().GetComponent<GridScript>().InitGrid(pathToLevelBlocks);
        Model.GameState = GameStates.ActiveGame;
    }

    public void KillCurrentGame()
    {
        if (CurrentGameObject != null)
            Object.Destroy(CurrentGameObject);
    }
}
