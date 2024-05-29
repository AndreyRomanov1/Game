using Unity.VisualScripting;
using UnityEngine;

public class SniperRifle: BaseGunScript
{
    protected override float bulletSpeed { get; set; } = 60;
    protected override float rateOfFire { get; set; } = 0.25f;
    protected override float bulletLifetime { get; set; } = 10;
    protected override float damage { get; set; } = 100;

    protected override void SelfStart()
    {
        bullet = Resources.Load("Weapons/Bullets/снайперский_патрон").GameObject();
    }
}
