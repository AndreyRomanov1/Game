using UnityEngine;

public static class CurrentGame
{
    public static GameObject GridGameObject;
    public static GameObject PlayerGameObject;

    public static GridScript Grid;
    public static PlayerScript Player;
    public static float GameSpeed => IsSlowGame ? 0.01f : 1f;
    public static bool IsSlowGame => Player.PlayerState == PlayerState.CrouchedToJump;
    // Здоровье, сколько прошёл блоков от начала забега. Список сгенерированных блоков 

    public static void ResetGame(string pathToLevelBlocks)
    {
        GridGameObject = Model.Game.CreateGrid();
        PlayerGameObject = Model.Game.CreatePlayer();
        
        Grid = GridGameObject.GetComponent<GridScript>();
        Player = PlayerGameObject.GetComponent<PlayerScript>();
        Grid.InitGrid(pathToLevelBlocks);
    }


    public static void KillGame()
    {
        Model.Game.Destroy(PlayerGameObject);
        Model.Game.Destroy(GridGameObject);
    }
}