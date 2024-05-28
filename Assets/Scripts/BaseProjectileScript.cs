using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public abstract class BaseProjectileScript: MonoBehaviour
{
    protected float _speed;
    protected LayerMask _mask;
    protected float _lifetime;
    protected float damage;

    public virtual void Shoot(Transform transformParent, float speed0, float lifetime, LayerMask mask0, float weaponDamage = 20f)
    {
        transform.position = transformParent.position;
        transform.rotation = Quaternion.Euler(0f, 0f, transformParent.eulerAngles.z);
        _speed = speed0;
        _lifetime = lifetime;
        _mask = mask0;
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
        for (var time = 0f; time < _lifetime; time += Time.fixedDeltaTime)
        {
            var displacement = transform.right * (CurrentGame.GameSpeed * _speed * Time.fixedDeltaTime);
            transform.Translate(displacement, Space.World);

            if (Tools.FindObjectOnLine(lastPos, transform.position, _mask, out var collision))
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
