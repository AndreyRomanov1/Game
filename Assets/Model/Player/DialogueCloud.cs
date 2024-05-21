using UnityEngine;

public class DialogueCloud
{
    public readonly GameObject Cloud;
    public readonly ISpeakingCharacter Speaker;

    public DialogueCloud(ISpeakingCharacter speaker, GameObject cloud)
    {
        Speaker = speaker;
        Cloud = cloud;
    }

    public override string ToString() => $"{Cloud.name} {Speaker}";
}