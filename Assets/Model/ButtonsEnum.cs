using System.Collections.Generic;

public enum ButtonsEnum
{
    F = 0,
    Space = 1,
}


public static class Buttons
{
    public static readonly Dictionary<ButtonsEnum, string> EnumToName = new()
    {
        { ButtonsEnum.F, "F" },
        { ButtonsEnum.Space, "Space"}
    };
}