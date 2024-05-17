using System.Linq;
using UnityEngine;


public class ShootEducationController : IController
{
    private readonly DialoguesPlayer dialogues;
    private int cloudNumber;
    private readonly GameObject[] clouds;
    private int MaxCloudNumber => clouds.Length;


    public ShootEducationController(DialoguesPlayer dialogues)
    {
        this.dialogues = dialogues;
        clouds = DialoguesPlayer.LoadSortedClouds("ShootEducation").OrderBy(t => int.Parse(t.name)).ToArray();
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