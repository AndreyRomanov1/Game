using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BazookaScript : BaseGunScript
{
    protected override float bulletSpeed { get; set; } = 5;
    protected override float rateOfFire { get; set; } = 0.5f;
    protected override float bulletLifetime { get; set; } = 50;
    protected override float damage { get; set; } = 50;
    
    private float explosionRadius = 4f;

    private bool needToReload = false;

    private Animator animator;

    protected override void SelfStart()
    {
        bullet = Resources.Load("Weapons/Bullets/ракета").GameObject();
        animator = GetComponent<Animator>();
        StartCoroutine(ReloadCoroutine());
    }

    protected override void Shoot()
    {
        var currentBullet = GameScript.CreateByGameObjectInCurrentGame(bullet).GetComponent<RocketScript>();
        soundSource?.Shoot();
        currentBullet.Shoot(transform, bulletSpeed, bulletLifetime, mask, damage, explosionRadius);
        needToReload = true;
        animator.Play("Reload");
    }

    private IEnumerator ReloadCoroutine()
    {
        while (true)
        {
            yield return new WaitUntil(() => needToReload);
            yield return new WaitForSeconds(1f);
            soundSource.Reload();
            needToReload = false;
        }
    }
}
