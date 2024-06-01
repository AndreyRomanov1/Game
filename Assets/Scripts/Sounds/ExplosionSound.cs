
using System;
using UnityEngine;

public class ExplosionSound : MonoBehaviour
{
    [SerializeField] private AudioClip explosion;
    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        source.clip = explosion;
        source.Play();
    }

    public void Explosion(Vector2 position) =>
        AudioSource.PlayClipAtPoint(explosion, position);
}
