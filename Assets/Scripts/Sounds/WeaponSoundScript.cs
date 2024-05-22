using UnityEngine;

public class WeaponSoundScript: MonoBehaviour
{
    [SerializeField] private AudioClip ShootSound;
    [SerializeField] private AudioClip ReloadSound;

    public void Shoot(Vector3 shootPosition) => 
        AudioSource.PlayClipAtPoint(ShootSound, shootPosition);

    public void Reload(Vector3 gunPosition) =>
        AudioSource.PlayClipAtPoint(ReloadSound, gunPosition);
}