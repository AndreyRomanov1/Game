using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using MouseButton = UnityEngine.UIElements.MouseButton;

public abstract class BaseGunScript: MonoBehaviour, IPickable
{
    public LayerMask mask { get; private set; } 
    public WeaponStateEnum weaponType { get; private set; }
    
    public GameObject bullet { get; set; }
    protected abstract float bulletSpeed { get; set; }
    protected abstract float rateOfFire { get; set; }
    protected abstract float bulletLifetime { get; set; }
    protected abstract float damage { get; set; }
    public LayerMask baseMask { get; set; }

    protected BulletTrajectoryRenderScript trajectoryRender;
    protected bool isNeedShoot = false;
    protected float timeBetweenShots;
    protected Coroutine currentMode;
    protected WeaponSoundScript soundSource;

    public void ShootSignal() => isNeedShoot = true;

    protected virtual void Start()
    {
        trajectoryRender = GetComponent<BulletTrajectoryRenderScript>();
        timeBetweenShots = 1 / rateOfFire;
        soundSource = GetComponent<WeaponSoundScript>();
        baseMask = LayerMask.GetMask("Block");
        
        DetectMode();
        SelfStart();
    }

    protected virtual void SelfStart()
    {
        bullet = Resources.Load("Weapons/Bullets/Bullet").GameObject();
    }

    protected void DetectMode()
    {
        var parentLayer = transform.parent.gameObject.layer;
        if (parentLayer == LayerMask.NameToLayer("Player"))
            SetMode(WeaponStateEnum.Player);
        else if (parentLayer == LayerMask.NameToLayer("Enemies"))
            SetMode(WeaponStateEnum.Enemy);
        else if (parentLayer == LayerMask.NameToLayer("Default"))
            SetMode(WeaponStateEnum.Nothing);
    }

    public void SetMode(WeaponStateEnum mode)
    {
        if (currentMode is not null)
            StopCoroutine(currentMode);
        weaponType = mode;
        mask = baseMask;
        switch (weaponType)
        {
            case WeaponStateEnum.Player:
                mask |= LayerMask.GetMask("Enemies");
                currentMode = StartCoroutine(ShootByClick());
                break;
            case WeaponStateEnum.Enemy:
                mask |= LayerMask.GetMask("Player");
                currentMode = StartCoroutine(ShootBySignal());
                break;
            case WeaponStateEnum.Nothing:
                currentMode = StartCoroutine(WaitForEvent());
                break;
        }
    }

    protected virtual IEnumerator ShootByClick()
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

    protected virtual IEnumerator ShootBySignal()
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

    protected virtual IEnumerator WaitForEvent()
    {
        yield break;
    }
    
    protected virtual void Shoot()
    {
        var currentBullet = Instantiate(bullet).GetComponent<BaseProjectileScript>();
        soundSource?.Shoot(transform.position);
        currentBullet.Shoot(transform, bulletSpeed, bulletLifetime, mask, damage);
    }

    public virtual void PickUp(PlayerScript player)
    {
        transform.position = new Vector3(0, 0);
        player.SetGun(gameObject);
    }
}
