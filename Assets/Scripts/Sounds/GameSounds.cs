using System;
using UnityEngine;
using UnityEngine.UI;

public static class GameSounds
{
     private static AudioClip buttonPress = Resources.Load<AudioClip>("Sound/нажатие_клавиши");
     private static AudioClip pauseStart = Resources.Load<AudioClip>("Sound/запуск_паузы");
     private static AudioClip pauseEnd = Resources.Load<AudioClip>("Sound/выход_с_паузы");
     private static AudioClip gameEnd = Resources.Load<AudioClip>("Sound/окончание_игры");

    public static void ButtonPress() =>
        AudioSource.PlayClipAtPoint(buttonPress, Vector3.zero);

    public static void StartPause() =>
        AudioSource.PlayClipAtPoint(pauseStart, Vector3.zero);

    public static void EndPause() =>
        AudioSource.PlayClipAtPoint(pauseEnd, Vector3.zero);

    public static void EndGame() =>
        AudioSource.PlayClipAtPoint(gameEnd, Vector3.zero);
}
