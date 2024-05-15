using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialoguesPlayer
{
    private readonly PlayerScript player;
    private readonly GameObject dialoguesAnchor;
    private int dialogueNumber;
    private GameObject activeDialogue;
    private IController activeController;
    private readonly Dictionary<string, IController> dialogues;

    public DialoguesPlayer(PlayerScript player)
    {
        this.player = player;

        dialoguesAnchor = GameObject.Find("DialoguesAnchor");
        dialogues = new Dictionary<string, IController>
        {
            ["JumpEducation"] = new JumpEducationController(this),
            ["ShootEducation"] = new ShootEducationController(this),
        };
    }

    public IEnumerator DialoguesCoroutine()
    {
        yield return new WaitForFixedUpdate();
        while (true)
        {
            if (activeDialogue != null && Input.GetKeyDown(KeyCode.Space))
                activeController.NextDialogues();
            yield return null;
        }
    }

    public void DialogueHandler(string triggerName)
    {
        Debug.Log(triggerName);
        if (dialogues.TryGetValue(triggerName, out activeController))
            activeController.NextDialogues();
        else
            Debug.Log("Не нашёл контроллер для такого триггера");
    }

    public void SetDialogueCloud(GameObject cloud)
    {
        ResetDialogueCloud();
        activeDialogue = Object.Instantiate(cloud, dialoguesAnchor.transform);
        Model.GameState = GameState.Dialogue;
    }

    public void ResetDialogueCloud()
    {
        if (activeDialogue != null)
            Object.Destroy(activeDialogue);
        Model.GameState = GameState.ActiveGame;
    }

    public static GameObject[] LoadSortedClouds(string path) => GameScript
        .LoadAllByName($"Clouds/{path}")
        .OrderBy(t => t.name)
        .ToArray();
}