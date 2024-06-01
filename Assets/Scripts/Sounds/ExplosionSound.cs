
using UnityEngine;

public class ExplosionSound : MonoBehaviour
{
    [SerializeField] private AudioClip explosion;

    public void Explosion(Vector2 position) =>
        AudioSource.PlayClipAtPoint(explosion, position);
}
