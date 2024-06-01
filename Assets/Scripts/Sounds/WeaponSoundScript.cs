using System;
using UnityEngine;

public class WeaponSoundScript: SoundController
{
    [SerializeField] protected AudioClip ShootSound;
    [SerializeField] protected AudioClip ReloadSound;
    

    public void Shoot() => 
        source.PlayOneShot(ShootSound);

    public void Reload() =>
        source.PlayOneShot(ReloadSound);
}

