using Unity.VisualScripting;
using UnityEngine;

public class BazookaScript : BaseGunScript
{
    protected override float bulletSpeed { get; set; } = 5;
    protected override float rateOfFire { get; set; } = 0.5f;
    protected override float bulletLifetime { get; set; } = 50;
    protected override float damage { get; set; } = 50;
    
    protected float explosionRadius = 4f;

    protected override void SelfStart()
    {
        bullet = Resources.Load("Weapons/Bullets/ракета").GameObject();
    }

    protected override void Shoot()
    {
        Debug.Log(Time.timeScale);
        var currentBullet = GameScript.CreateByGameObjectInCurrentGame(bullet).GetComponent<RocketScript>();
        soundSource?.Shoot(transform.position);
        currentBullet.Shoot(transform, bulletSpeed, bulletLifetime, mask, damage, explosionRadius);
    }
}
