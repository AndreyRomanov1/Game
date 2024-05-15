using UnityEngine;

public static class CurrentGame
{
    public static GridScript Grid;
    public static PlayerScript Player;

    public static Camera PlayerCamera;

    public static float GameSpeed => IsSlowGame ? 0.01f : 1f;

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
    }

    public static void KillGame()
    {
        Object.Destroy(Player.gameObject);
        Object.Destroy(Grid.gameObject);
    }
}