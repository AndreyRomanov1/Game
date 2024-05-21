using UnityEngine;

public interface ISpeakingCharacter
{
    GameObject GetDialoguesAnchor();
    void ShowIfNeed();
    void HideIfNeed();
}