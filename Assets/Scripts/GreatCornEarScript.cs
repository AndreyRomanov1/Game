using UnityEngine;

public class GreatCornEarScript : MonoBehaviour, ISpeakingCharacter
{
    private GameObject dialoguesAnchor;

    private void Start()
    {
        dialoguesAnchor = GameObject.Find("GreatCornEar(Clone)/DialoguesAnchor");
    }

    public GameObject GetDialoguesAnchor() => dialoguesAnchor;

    public void ShowIfNeed() => gameObject.SetActive(true);

    public void HideIfNeed() => gameObject.SetActive(false);
}