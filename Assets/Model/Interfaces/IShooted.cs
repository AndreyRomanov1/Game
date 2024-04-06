using UnityEngine;

public interface IShooted
{
    public void Shoot(Vector3 position, UnityEngine.Quaternion rotation, float speed, LayerMask mask);

    public void Shoot(Transform transformParent, float speed, LayerMask mask);
}