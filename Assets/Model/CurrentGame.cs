public static class CurrentGame
{
    public static float GameSpeed => isSlowGame ? 0.01f : 1f;
    public static bool isSlowGame;
}