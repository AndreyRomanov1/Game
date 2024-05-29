using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public abstract class BaseProjectileScript: MonoBehaviour
{
    protected float speed;
    protected LayerMask mask;
    protected float lifetime;
    protected float damage;

    public virtual void Shoot(Transform transformParent, float speed0, float lifetime, LayerMask mask0, float weaponDamage = 20f)
    {
        transform.position = transformParent.position;
        transform.rotation = Quaternion.Euler(0f, 0f, transformParent.eulerAngles.z);
        speed = speed0;
        this.lifetime = lifetime;
        mask = mask0;
        damage = weaponDamage;

        StartCoroutine(FixedUpdateCoroutine());
    }

    protected virtual void Destroy()
    {
        Destroy(this.GameObject());
    }

    protected virtual IEnumerator FixedUpdateCoroutine()
    {
        var lastPos = transform.position;
        for (var time = 0f; time < lifetime; time += Time.fixedDeltaTime)
        {
            var displacement = transform.right * (speed * Time.fixedDeltaTime);
            transform.Translate(displacement, Space.World);

            if (Tools.FindObjectOnLine(lastPos, transform.position, mask, out var collision))
                CollisionLogic(collision);

            yield return new WaitForFixedUpdate();

            lastPos += displacement;
        }
        
        Destroy();
    }
    
    protected virtual void CollisionLogic(GameObject other)
    {
        other.GetComponent<IDamageable>()?.TakeDamage(damage);
        Destroy();
    }
}
