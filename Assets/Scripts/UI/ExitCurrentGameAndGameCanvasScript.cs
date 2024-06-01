using UnityEngine;
using UnityEngine.UI;

public class ExitCurrentGameAndGameCanvasScript : MonoBehaviour
{
    public Button exitCurrentGameButton;
    public Button exitButton;

    private void Start()
    {
        InitButtons();
    }

    private void InitButtons()
    {
        exitCurrentGameButton = GameObject.Find("ExitCurrentGameButton").GetComponent<Button>();
        exitCurrentGameButton.onClick.AddListener(PauseUIController.ExitCurrentGame);
        exitButton = GameObject.Find("ExitGameButton").GetComponent<Button>();
        exitButton.onClick.AddListener(UI.ExitGame);
    }
}
