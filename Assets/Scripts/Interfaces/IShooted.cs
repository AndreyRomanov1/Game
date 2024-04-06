using UnityEngine;

public interface IShooted
{
    public void Shoot(Vector3 position, UnityEngine.Quaternion rotation, float speed, GameController gameController,
        LayerMask mask);

    public void Shoot(Transform transformParent, float speed, GameController gameController, LayerMask mask);
    // {
    // Shoot(transformParent.position, transformParent.rotation, speed, gameController, mask);
    // }
}