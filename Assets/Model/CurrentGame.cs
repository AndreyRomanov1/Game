using UnityEngine;

public static class CurrentGame
{
    public static GridScript Grid;
    public static PlayerScript Player;

    public static Camera PlayerCamera;

    public static float GameSpeed => IsSlowGame ? 0.01f : 1f;

    public static bool IsSlowGame => Player.PlayerState is
        PlayerState.CrouchedToJump
        or PlayerState.CrouchedToJumpFromLeftWall
        or PlayerState.CrouchedToJumpFromRightWall;
    // Здоровье, сколько прошёл блоков от начала забега. Список сгенерированных блоков 

    public static void ResetGame(string pathToLevelBlocks)
    {
        Grid = Model.Game.CreateGrid().GetComponent<GridScript>();
        Player = Model.Game.CreatePlayer().GetComponent<PlayerScript>();
        PlayerCamera = Player.GetComponentInChildren<Camera>();
        Grid.InitGrid(pathToLevelBlocks);
    }

    public static void KillGame()
    {
        Model.Game.Destroy(Player.gameObject);
        Model.Game.Destroy(Grid.gameObject);
    }
}