using UnityEngine;

public class DialogueTriggerScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))  // Проверяем, что столкнулись с объектом Player
            CurrentGame.Player.Dialogues.DialogueHandler(gameObject.name);
    }
}
