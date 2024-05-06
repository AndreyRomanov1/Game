using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class PistolScript : MonoBehaviour
{
    public WeaponState weaponType;
    public GameObject bullet;
    public float bulletSpeed = 20;
    public float rateOfFire = 1;
    public float bulletLifetime = 10;
    public float damage = 20f;
    public LayerMask mask;
    
    private bool isNeedShoot = false;
    private float timeBetweenShots;

    public void ShootSignal() => isNeedShoot = true;

    private void Start()
    {
        timeBetweenShots = 1 / rateOfFire;

        if (weaponType == WeaponState.Player)
            StartCoroutine(ShootByClick());
        else if (weaponType == WeaponState.Enemy)
            StartCoroutine(ShootBySignal());
        else
            StartCoroutine(WaitForEvent());
    }

    private IEnumerator ShootByClick()
    {
        while (true)
        {
            if (Input.GetMouseButton((int)MouseButton.LeftMouse))
            {
                Shoot();
                yield return new WaitForSeconds(timeBetweenShots);
            }
            else
                yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator ShootBySignal()
    {
        while (true)
        {
            if (isNeedShoot)
            {
                Shoot();
                yield return new WaitForSeconds(timeBetweenShots);
                isNeedShoot = false;
            }
            else
                yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator WaitForEvent()
    {
        yield break;
    }

    // void Update()
    // {
    //     if (Input.GetMouseButton((int)MouseButton.LeftMouse))
    //         Shoot();
    // }

    private void Shoot()
    {
        var currentBullet = Instantiate(bullet).GetComponent<BulletScript>();
        currentBullet.Shoot(transform, bulletSpeed, bulletLifetime, mask, damage);
    }
}