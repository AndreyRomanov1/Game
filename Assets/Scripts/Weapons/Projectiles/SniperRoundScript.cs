using UnityEngine;

public class SniperRoundScript: BaseProjectileScript
{
    public int maxNumberEnemiesPenetrated = 3;
    private int numberEnemyPenetrated = 0;
    
    protected override void CollisionLogic(GameObject other)
    {
        other.GetComponent<IDamageable>()?.TakeDamage(damage);
        if ((LayerMask.LayerToName(other.layer) == "Enemies" || other.CompareTag("DestroyedObject"))
            && numberEnemyPenetrated < maxNumberEnemiesPenetrated)
            numberEnemyPenetrated++;
        else
            Destroy();
    }
    
}