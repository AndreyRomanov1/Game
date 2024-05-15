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
        clouds = DialoguesPlayer.LoadSortedClouds("JumpEducation");
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