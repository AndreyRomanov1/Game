using System.Collections.Generic;

public enum SpeakersEnum
{
    Player = 0,
    GreatCornEar = 1
}

public static class Speakers
{
    public static readonly Dictionary<SpeakersEnum, string> EnumToPath = new()
    {
        [SpeakersEnum.Player] = "Player",
        [SpeakersEnum.GreatCornEar] = "GreatCornEar"
    };
}