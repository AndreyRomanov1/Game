using UnityEngine;

public class ShootEducationController : IController
{
    private int cloudNumber;
    private readonly DialogueCloud[] clouds = Dialogues.LoadSortedClouds("ShootEducation");
    private int MaxCloudNumber => clouds.Length;

    public void NextDialogues()
    {
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