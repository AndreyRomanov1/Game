
using UnityEngine;

public class SoundController: MonoBehaviour
{
    protected AudioSource source;

    protected void Start()
    {
        source = GetComponent<AudioSource>();
    }
}
