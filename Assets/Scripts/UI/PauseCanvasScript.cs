using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PauseCanvasScript : MonoBehaviour
{
    public Button continueGameButton;
    public Button exitCurrentGameButton;
    public Button exitButton;

    private void Start()
    {
        InitButtons();
    }

    private void InitButtons()
    {
        continueGameButton = GameObject.Find("ContinueGameButton").GetComponent<Button>();
        continueGameButton.onClick.AddListener(PauseUIController.EndPause);
        exitCurrentGameButton = GameObject.Find("ExitCurrentGameButton").GetComponent<Button>();
        exitCurrentGameButton.onClick.AddListener(PauseUIController.ExitCurrentGame);
        exitButton = GameObject.Find("ExitGameButton").GetComponent<Button>();
        exitButton.onClick.AddListener(UI.ExitGame);
    }
}