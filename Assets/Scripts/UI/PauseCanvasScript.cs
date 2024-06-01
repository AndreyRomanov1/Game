using UnityEngine;
using UnityEngine.UI;

public class PauseCanvasScript : MonoBehaviour
{
    public Button continueGameButton;
    private void Start()
    {
        InitButtons();
    }

    private void InitButtons()
    {
        continueGameButton = GameObject.Find("ContinueGameButton").GetComponent<Button>();
        continueGameButton.onClick.AddListener(PauseUIController.EndPause);
    }
}