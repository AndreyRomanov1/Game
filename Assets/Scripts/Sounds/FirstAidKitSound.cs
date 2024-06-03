using UnityEngine;

public class FirstAidKitSound: SoundController
{
    [SerializeField] private AudioClip heal;

    public void Heal() =>
        source.PlayOneShot(heal);
}
