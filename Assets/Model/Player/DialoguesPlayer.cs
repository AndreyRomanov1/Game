using System.Collections;
using System.Linq;
using UnityEngine;

public class DialoguesPlayer
{
    private readonly PlayerScript player;
    private readonly GameObject dialoguesAnchor;
    private int dialogueNumber;
    private GameObject activeDialogue;

    public DialoguesPlayer(PlayerScript player)
    {
        this.player = player;

        dialoguesAnchor = GameObject.Find("DialoguesAnchor");
    }

    public IEnumerator DialoguesCoroutine()
    {
        yield return new WaitForFixedUpdate();
        while (true)
        {
            if (activeDialogue != null && Input.GetKey(KeyCode.Space))
                ResetDialogueCloud();
            yield return null;
        }
    }

    public void DialogueHandler(string triggerName)
    {
        switch (triggerName)
        {
            case "1":
                break;
            default:
                Debug.Log(triggerName);
                SetDialogueCloud(Model.Clouds[0]); // Тестовое облако

                break;
        }
    }

    private void SetDialogueCloud(GameObject cloud)
    {
        ResetDialogueCloud();
        activeDialogue = Object.Instantiate(cloud, dialoguesAnchor.transform);
        Model.GameState = GameState.Dialogue;
    }
    private void ResetDialogueCloud()
    {
        if (activeDialogue != null)
            Object.Destroy(activeDialogue);
        Model.GameState = GameState.ActiveGame;
    }
}