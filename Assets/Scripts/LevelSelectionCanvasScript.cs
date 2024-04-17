using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionCanvasScript : MonoBehaviour
{
    public Canvas canvas;
    public Button startLevelButton;
    public Button infinityLevelButton;

    private readonly Dictionary<string, string> buttonToLevel = new()
    {
        ["StartLevelButton"] = "StartBlocks",
        ["InfinityLevelButton"] = "InfinityBlocks"
    };

    private readonly List<Button> buttons = new();

    private void Start()
    {
        canvas = GetComponent<Canvas>();
        InitButtons();
    }

    private void InitButtons()
    {
        startLevelButton = GameObject.Find("StartLevelButton").GetComponent<Button>();
        startLevelButton.onClick.AddListener(StartLevelButtonOnClick);
        infinityLevelButton = GameObject.Find("InfinityLevelButton").GetComponent<Button>();
        infinityLevelButton.onClick.AddListener(InfinityLevelButtonOnClick);
    }

    private void StartLevelButtonOnClick()
    {
        HideCanvas();
        Debug.Log("StartLevelButtonOnClick");
        Model.StartNewGame("StartBlocks");
    }

    private void InfinityLevelButtonOnClick()
    {
        HideCanvas();
        Debug.Log("InfinityLevelButtonOnClick");
        Model.StartNewGame("InfinityBlocks");
    }

    public void HideCanvas() => canvas.enabled = false;
    public void ShowCanvas() => canvas.enabled = true;
}