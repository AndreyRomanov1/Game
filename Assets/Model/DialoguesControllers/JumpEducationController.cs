using System.Collections.Generic;
using System.Linq;

public class JumpEducationController : IController
{
    private int cloudNumber;
    private readonly DialogueCloud[] clouds = Dialogues.LoadSortedClouds("JumpEducation");
    private int MaxCloudNumber => clouds.Length;

    public void NextDialogues()
    {
        if (cloudNumber >= MaxCloudNumber)
            Dialogues.EndDialogue();
        else
        {
            Dialogues.SetDialogueCloud(clouds[cloudNumber]);
            cloudNumber++;
        }
    }

    public HashSet<ISpeakingCharacter> GetDialogueParticipants()
        => clouds.Select(t => t.Speaker).ToHashSet();
}