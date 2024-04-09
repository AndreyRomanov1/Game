public static class CurrentGame
{
    public static float GameSpeed => isSlowGame ? 0.01f : 1f;
    public static bool isSlowGame;
    // Здоровье, сколько прошёл блоков от начала забега. список сгенерированных блоков 

    public static void ResetGame()
    {
        isSlowGame = false;
    }
}