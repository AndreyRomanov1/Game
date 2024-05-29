using UnityEngine;

public class BazookaScript : BaseWeaponScript
{
    public float explosionRadius = 4f;
    public LayerMask explosionMask;
    
    protected override void Shoot()
    {
        var currentBullet = GameScript.CreateByGameObjectInCurrentGame(bullet).GetComponent<RocketScript>();
        soundSource?.Shoot(transform.position);
        currentBullet.Shoot(transform, bulletSpeed, bulletLifetime, mask, explosionMask, damage, explosionRadius);
    }
}
