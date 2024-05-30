using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Dialogues
{
    private static int dialogueNumber;
    private static GameObject activeDialogue;
    private static IController activeController;

    private static Dictionary<string, IController> DialogueController;

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

    public static void Reset()
    {
        Debug.Log("Reset Dialogues");
        DialogueController = new Dictionary<string, IController>
        {
            ["JumpEducation"] = new JumpEducationController(),
            ["ShootEducation"] = new ShootEducationController(),
        };
    }

    public static void DialogueHandler(string triggerName)
    {
        Debug.Log(triggerName);
        if (DialogueController.TryGetValue(triggerName, out activeController))
        {
            StartDialogue();
            activeController.NextDialogues();
        }
        else
            Debug.Log("Не нашёл контроллер для такого триггера");
    }

    public static void SetDialogueCloud(DialogueCloud dialogueCloud)
    {
        ResetDialogueCloud();
        activeDialogue = Object.Instantiate(dialogueCloud.Cloud, dialogueCloud.Speaker.GetDialoguesAnchor().transform);
    }

    private static void ResetDialogueCloud()
    {
        if (activeDialogue != null)
            Object.Destroy(activeDialogue);
    }

    public static DialogueCloud[] LoadSortedClouds(string path) =>
        Speakers.EnumToPath
            .Select(speakerAndPath =>
                (speakerAndPath.Key, GameScript.LoadAllByName("Clouds/" + path + "/" + speakerAndPath.Value)))
            .SelectMany(speakerAndClouds =>
                speakerAndClouds.Item2.Select(speakerAndCloud =>
                    new DialogueCloud(CurrentGame.EnumToSpeaker[speakerAndClouds.Key], speakerAndCloud)))
            .OrderBy(cloud => int.Parse(cloud.Cloud.name))
            .ToArray();

    public static void StartDialogue()
    {
        Model.GameState = GameStates.Dialogue;
        Debug.Log("Старт диалога");
        foreach (var speakingCharacter in activeController.GetDialogueParticipants())
            speakingCharacter.ShowIfNeed();
    }

    public static void EndDialogue()
    {
        Debug.Log("Конец диалога");
        foreach (var speakingCharacter in activeController.GetDialogueParticipants())
            speakingCharacter.HideIfNeed();
        Model.GameState = GameStates.ActiveGame;
        ResetDialogueCloud();
    }
}