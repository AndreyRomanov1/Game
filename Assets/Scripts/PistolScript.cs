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
    public LayerMask baseMask;

    public LayerMask mask;
    private BulletTrajectoryRenderScript trajectoryRender;
    private bool isNeedShoot = false;
    private float timeBetweenShots;
    private Coroutine currentMode;

    public void ShootSignal() => isNeedShoot = true;

    private void Start()
    {
        trajectoryRender = GetComponent<BulletTrajectoryRenderScript>();
        timeBetweenShots = 1 / rateOfFire;
        DetectMode();
    }

    private void DetectMode()
    {
        var parentLayer = transform.parent.gameObject.layer;
        if (parentLayer == LayerMask.NameToLayer("Player"))
            SetMode(WeaponState.Player);
        else if (parentLayer == LayerMask.NameToLayer("Enemies"))
            SetMode(WeaponState.Enemy);
        else if (parentLayer == LayerMask.NameToLayer("Default"))
            SetMode(WeaponState.Nothing);

        
    }

    public void SetMode(WeaponState mode)
    {
        if (currentMode is not null)
            StopCoroutine(currentMode);
        weaponType = mode;
        mask = baseMask;
        switch (weaponType)
        {
            case WeaponState.Player:
                mask |= LayerMask.GetMask("Enemies");
                currentMode = StartCoroutine(ShootByClick());
                break;
            case WeaponState.Enemy:
                mask |= LayerMask.GetMask("Player");
                currentMode = StartCoroutine(ShootBySignal());
                break;
            case WeaponState.Nothing:
                currentMode = StartCoroutine(WaitForEvent());
                break;
        }
    }

    private IEnumerator ShootByClick()
    {
        yield return new WaitForFixedUpdate();
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
        yield return new WaitForFixedUpdate();
        while (true)
        {
            trajectoryRender?.ShowTrajectory();
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