using UnityEngine;

public class SoundPlayer: SoundController
{
    [SerializeField] private AudioClip damage;

    public void Damage() =>
        source.PlayOneShot(damage);
}
