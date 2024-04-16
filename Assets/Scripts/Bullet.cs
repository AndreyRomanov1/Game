using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float _speed;
    private LayerMask _mask;
    private float _lifetime;

    public void Shoot(Transform transformParent, float speed0, float lifetime, LayerMask mask0)
    {
        transform.position = transformParent.position;
        transform.rotation = Quaternion.Euler(0f, 0f, transformParent.eulerAngles.z);
        _speed = speed0;
        _lifetime = lifetime;
        _mask = mask0;

        StartCoroutine(FixedUpdateCoroutine());
    }

    private void Destroy()
    {
        Destroy(this.GameObject());
    }

    private IEnumerator FixedUpdateCoroutine()
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
    
    private void CollisionLogic(GameObject other)
    {
        other.GetComponent<IDamageable>()?.TakeDamage(1f);
        Destroy();
    }

    // TODO: можно переписать на корутину
    // private void FixedUpdate()
    // {
    //     var lastPos = transform.position;
    //
    //     transform.Translate(transform.right * (CurrentGame.GameSpeed * _speed * Time.fixedDeltaTime), Space.World);
    //
    //     if (Tools.FindObjectOnLine(lastPos, transform.position, _mask, out var collision))
    //         CollisionLogic(collision);
    // }
    
    // TODO: можно переписать на расширения класса Physics2D
    // private bool FindObjectOnLine(Vector3 startPosition, Vector3 endPosition, out GameObject result)
    // {
    //     result = Physics2D.Linecast(startPosition, endPosition, mask).transform.GameObject();
    //     return result is not null;
    // }
}