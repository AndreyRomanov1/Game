using System.Collections.Generic;

public class EndEducationController : IController
{
    public void NextDialogues()
    {
        WinUIController.Show();
    }

    public HashSet<ISpeakingCharacter> GetDialogueParticipants() => new();
}