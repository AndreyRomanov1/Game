using UnityEngine;

public class FirstAidKitScript : MonoBehaviour, IPickable
{
    public float healCount = 20;

    public void PickUp(PlayerScript player)
    {
        player.Life.Heal(healCount);
    }
}