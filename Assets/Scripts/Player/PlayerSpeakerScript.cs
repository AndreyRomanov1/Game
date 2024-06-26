using System.Collections.Generic;
using UnityEngine;

public class PlayerSpeakerScript : MonoBehaviour, ISpeakingCharacter
{
    private GameObject dialoguesAnchor;
    private ISpeakingCharacter helper;

    private void Start()
    {
        dialoguesAnchor = GameObject.Find("DialoguesAnchor");
        var helperAnchor = GameObject.Find("HelperAnchor");
        var helperGameObject = Resources.Load<GameObject>("Healper/Префаб/GreatCornEar");
        helper = Instantiate(helperGameObject, helperAnchor.transform).GetComponent<GreatCornEarScript>();
        CurrentGame.EnumToSpeaker = new Dictionary<SpeakersEnum, ISpeakingCharacter>
        {
            [SpeakersEnum.Player] = this,
            [SpeakersEnum.GreatCornEar] = helper,
            [SpeakersEnum.Comics] = GameObject.Find("ComicsAnchor").GetComponent<ComicsScript>()
        };
        Dialogues.Reset();
        StartCoroutine(Dialogues.DialoguesCoroutine());
    }
    public GameObject GetDialoguesAnchor() => dialoguesAnchor;

    public void ShowIfNeed()
    {
    }

    public void HideIfNeed()
    {
    }
}
