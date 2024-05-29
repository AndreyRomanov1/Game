using UnityEngine;

public class GreatCornEarScript : MonoBehaviour, ISpeakingCharacter
{
    private GameObject dialoguesAnchor;
    private GameObject sprite;

    private void Start()
    {
        dialoguesAnchor = GameObject.Find("GreatCornEar(Clone)/DialoguesAnchor");
        sprite = GameObject.Find("Прапорчаток");
        HideIfNeed();
    }

    public GameObject GetDialoguesAnchor() => dialoguesAnchor;

    public void ShowIfNeed() => sprite.SetActive(true);

    public void HideIfNeed() => sprite.SetActive(false);
}