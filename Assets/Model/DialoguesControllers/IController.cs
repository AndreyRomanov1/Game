using System.Collections.Generic;

public interface IController
{
    void NextDialogues();
    HashSet<ISpeakingCharacter> GetDialogueParticipants();
}