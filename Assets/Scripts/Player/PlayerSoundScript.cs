using UnityEngine;

public class PlayerSoundScript : MonoBehaviour
{
    public SoundPlayer sound;

    private void Start()
    {
        sound = GetComponent<SoundPlayer>();
        Instantiate(Resources.Load("Sound/Background Music"), transform);
    }
}
