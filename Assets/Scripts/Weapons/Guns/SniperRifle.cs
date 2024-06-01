using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SniperRifle: BaseGunScript
{
    protected override float bulletSpeed { get; set; } = 60;
    protected override float rateOfFire { get; set; } = 0.25f;
    protected override float bulletLifetime { get; set; } = 10;
    protected override float damage { get; set; } = 100;
    
    private bool needToReload = false;

    protected override void SelfStart()
    {
        bullet = Resources.Load("Weapons/Bullets/снайперский_патрон").GameObject();
        StartCoroutine(ReloadCoroutine());
    }
    
    protected override void Shoot()
    {
        var currentBullet = GameScript.CreateByGameObjectInCurrentGame(bullet).GetComponent<SniperRoundScript>();
        soundSource?.Shoot();
        currentBullet.Shoot(transform, bulletSpeed, bulletLifetime, mask, damage);
        needToReload = true;
    }

    private IEnumerator ReloadCoroutine()
    {
        while (true)
        {
            yield return new WaitUntil(() => needToReload);
            yield return new WaitForSeconds(1.7f);
            soundSource.Reload();
            needToReload = false;
        }
    }
}
