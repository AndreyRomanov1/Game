using System.Linq;
using UnityEngine;

public class JumpEducationController : IController
{
    private readonly DialoguesPlayer dialogues;
    private int cloudNumber;
    private readonly GameObject[] clouds;
    private int MaxCloudNumber => clouds.Length;

    public JumpEducationController(DialoguesPlayer dialogues)
    {
        this.dialogues = dialogues;
        clouds = DialoguesPlayer.LoadSortedClouds("JumpEducation").OrderBy(t => int.Parse(t.name)).ToArray();
    }

    public void NextDialogues()
    {
        if (cloudNumber >= MaxCloudNumber)
        {
            Debug.Log("Конец диалога");
            dialogues.ResetDialogueCloud();
        }
        else
        {
            dialogues.SetDialogueCloud(clouds[cloudNumber]);
            cloudNumber++;
        }
    }
}