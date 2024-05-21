using System.Linq;
using UnityEngine;

public class JumpEducationController : IController
{
    private int cloudNumber;
    private readonly DialogueCloud[] clouds = Dialogues.LoadSortedClouds("JumpEducation");
    private int MaxCloudNumber => clouds.Length;

    public void NextDialogues()
    {
        Debug.Log(string.Join(" ", clouds.Select(t => t.ToString())));
        if (cloudNumber >= MaxCloudNumber)
        {
            Debug.Log("Конец диалога");
            Dialogues.ResetDialogueCloud();
        }
        else
        {
            Dialogues.SetDialogueCloud(clouds[cloudNumber]);
            cloudNumber++;
        }
    }
}