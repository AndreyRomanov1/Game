using UnityEngine;

public class SoundPlayer: SoundController
{
    private AudioClip damage;

    public void Damage() =>
        source.PlayOneShot(damage);
}
