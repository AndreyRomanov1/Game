
using UnityEngine;

public class CollectionTriggerSound: SoundController
{
    [SerializeField] private AudioClip collectionSound;

    public void CollectionSound() =>
        source.PlayOneShot(collectionSound);
}
