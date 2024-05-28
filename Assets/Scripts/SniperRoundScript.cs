using UnityEngine;

public class SniperRoundScript: BaseProjectileScript
{
    public int MaxNumberEnemiesPenetrated = 3;
    private int numberEnemyPenetrated = 0;
    
    protected override void CollisionLogic(GameObject other)
    {
        other.GetComponent<IDamageable>()?.TakeDamage(damage);
        if (LayerMask.LayerToName(other.layer) == "Enemies" && numberEnemyPenetrated < MaxNumberEnemiesPenetrated)
            numberEnemyPenetrated++;
        else
            Destroy();
    }
    
}