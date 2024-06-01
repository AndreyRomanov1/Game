
using System;
using UnityEngine;
using UnityEngine.Serialization;

public class RocketSound: MonoBehaviour
{
    [FormerlySerializedAs("RocketFlyingSound")] [SerializeField] private AudioClip rocketFlyingSound;
    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        source.loop = true;
        source.clip = rocketFlyingSound;
        source.Play();
    }
}
