using System.Collections.Generic;

public class EndEducationController : IController
{
    public void NextDialogues()
    {
        UI.Show(UI.WinEducationGameObject);
    }

    public HashSet<ISpeakingCharacter> GetDialogueParticipants() => new();
}