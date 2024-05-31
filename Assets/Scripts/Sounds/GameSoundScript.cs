using System;
using UnityEngine;

public class GameSoundScript :MonoBehaviour
{
    [SerializeField] private AudioClip buttonPress;
    [SerializeField] private AudioClip pauseStart;
    [SerializeField] private AudioClip pauseEnd;

    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void ButtonPress() =>
        source.PlayOneShot(buttonPress);

    public void PauseStart() =>
        source.PlayOneShot(pauseStart);

    public void PauseEnd() =>
        source.PlayOneShot(pauseEnd);
}
