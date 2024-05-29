using UnityEngine;

public class PistolScript : BaseGunScript
{
    protected override float bulletSpeed { get; set; } = 20;
    protected override float rateOfFire { get; set; } = 4;
    protected override float bulletLifetime { get; set; } = 10;
    protected override float damage { get; set; } = 20;
}