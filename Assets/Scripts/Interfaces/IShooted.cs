using UnityEngine;
using Quaternion = System.Numerics.Quaternion;

namespace DefaultNamespace.Interfaces
{
    public interface IShooted
    {
        public void Shoot(Vector3 position, UnityEngine.Quaternion rotation, float speed, GameController gameController);

        public void Shoot(Transform trasformPerent, float speed, GameController gameController)
        {
            Shoot(trasformPerent.position, trasformPerent.rotation, speed, gameController);
        }
    }
}