using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionCanvasScript : MonoBehaviour
{
    public Button startLevelButton;
    public Button infinityLevelButton;
    public Button exitButton;

    private void Start()
    {
        InitButtons();
    }

    private void InitButtons()
    {
        startLevelButton = GameObject.Find("StartLevelButton").GetComponent<Button>();
        startLevelButton.onClick.AddListener(() => LevelSelectionUIController.StartNewGame("StartBlocks"));
        infinityLevelButton = GameObject.Find("InfinityLevelButton").GetComponent<Button>();
        infinityLevelButton.onClick.AddListener(() => LevelSelectionUIController.StartNewGame("InfinityBlocks"));
        exitButton = GameObject.Find("ExitGameButton").GetComponent<Button>();
        exitButton.onClick.AddListener(UI.ExitGame);
    }
}