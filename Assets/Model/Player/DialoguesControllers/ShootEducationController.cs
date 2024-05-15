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
        clouds = DialoguesPlayer.LoadSortedClouds("ShootEducation");
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
            Debug.Log("Следующий кадр диалога");
            dialogues.SetDialogueCloud(clouds[cloudNumber]);
            cloudNumber++;
        }
    }
}