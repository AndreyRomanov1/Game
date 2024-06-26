using UnityEngine;

public class PlayerTimeFreezeBacklightScript : MonoBehaviour
{
    public GameObject timeFreeze;

    private void Start()
    {
        timeFreeze = GameObject.Find("TimeFreeze");
        timeFreeze.SetActive(false);
    }
}
