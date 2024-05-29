using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum WeaponStateEnum
{
    Nothing,
    Player,
    Enemy
}

public static class WeaponState
{
    public static readonly Dictionary<WeaponStateEnum, string> StateToLayerNameConnectedObject = new()
    {
        { WeaponStateEnum.Player, "Player"},
        { WeaponStateEnum.Enemy, "Enemies"},
        { WeaponStateEnum.Nothing, "Default"}
    };

    public static readonly Dictionary<string, WeaponStateEnum> TagObjectToConnectedState = new()
    {
        { "Player", WeaponStateEnum.Player},
        { "Enemies", WeaponStateEnum.Enemy},
        { "Default", WeaponStateEnum.Nothing}
    };

    public static LayerMask StateToLayerConnectedObject(WeaponStateEnum state)
        => LayerMask.NameToLayer(StateToLayerNameConnectedObject[state]);

}
