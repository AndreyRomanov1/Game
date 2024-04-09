using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed;
    private LayerMask mask;
    private Vector3 dir;

    public void Shoot(Transform transformParent, float speed0, LayerMask mask0)
    {
        transform.position = transformParent.position;
        transform.rotation = Quaternion.Euler(0f, 0f, transformParent.eulerAngles.z);
        speed = speed0;
        mask = mask0;
    }

    // TODO: можно переписать на корутину
    private void FixedUpdate()
    {
        var lastPos = transform.position;

        transform.Translate(transform.right * (CurrentGame.GameSpeed * speed * Time.fixedDeltaTime), Space.World);

        if (Tools.FindObjectOnLine(lastPos, transform.position, mask, out var collision))
            CollisionLogic(collision);
    }

    private void CollisionLogic(GameObject other)
    {
        Destroy(this.GameObject());
    }

    // TODO: можно переписать на расширения класса Physics2D
    // private bool FindObjectOnLine(Vector3 startPosition, Vector3 endPosition, out GameObject result)
    // {
    //     result = Physics2D.Linecast(startPosition, endPosition, mask).transform.GameObject();
    //     return result is not null;
    // }
}