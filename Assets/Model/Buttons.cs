using System.Collections.Generic;

public enum Buttons
{
    F = 0,
    Space = 1,
}


public static class ButtonsEnum
{
    public static Dictionary<Buttons, string> EnumToName = new()
    {
        { Buttons.F, "F" },
        { Buttons.Space, "Space"}
    };
}