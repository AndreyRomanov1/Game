using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Dialogues
{
    private static int dialogueNumber;
    private static GameObject activeDialogue;
    private static IController activeController;
    private static readonly Dictionary<string, IController> DialogueController;

    static Dialogues()
    {
        DialogueController = new Dictionary<string, IController>
        {
            ["JumpEducation"] = new JumpEducationController(),
            ["ShootEducation"] = new ShootEducationController(),
        };
    }

    public static IEnumerator DialoguesCoroutine()
    {
        yield return new WaitForFixedUpdate();
        while (true)
        {
            if (activeDialogue != null && Input.GetKeyDown(KeyCode.Space))
                activeController.NextDialogues();
            yield return null;
        }
    }

    public static void DialogueHandler(string triggerName)
    {
        Debug.Log(triggerName);
        if (DialogueController.TryGetValue(triggerName, out activeController))
            activeController.NextDialogues();
        else
            Debug.Log("Не нашёл контроллер для такого триггера");
    }

    public static void SetDialogueCloud(DialogueCloud dialogueCloud)
    {
        ResetDialogueCloud();
        activeDialogue = Object.Instantiate(dialogueCloud.Cloud, dialogueCloud.Speaker.GetDialoguesAnchor().transform);
        Model.GameState = GameState.Dialogue;
    }

    public static void ResetDialogueCloud()
    {
        if (activeDialogue != null)
            Object.Destroy(activeDialogue);
        Model.GameState = GameState.ActiveGame;
    }

    public static DialogueCloud[] LoadSortedClouds(string path) =>
        Speakers.EnumToPath
            .Select(speakerAndPath =>
                (speakerAndPath.Key, GameScript.LoadAllByName("Clouds/" + path + "/" + speakerAndPath.Value)))
            .SelectMany(speakerAndClouds =>
                speakerAndClouds.Item2.Select(t2 =>
                    new DialogueCloud(CurrentGame.EnumToSpeaker[speakerAndClouds.Key], t2)))
            .OrderBy(cloud => int.Parse(cloud.Cloud.name))
            .ToArray();
}