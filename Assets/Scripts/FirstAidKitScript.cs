using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstAidKitScript : MonoBehaviour, IPickable
{
    public float HealCount = 20;

    public void PickUp(PlayerScript player)
    {
        player.life.Heal(HealCount);
    }
}
