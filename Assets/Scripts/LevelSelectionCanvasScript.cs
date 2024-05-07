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
        ShowCanvas();
    }

    private void InitButtons()
    {
        startLevelButton = GameObject.Find("StartLevelButton").GetComponent<Button>();
        startLevelButton.onClick.AddListener(() => OnClick("StartBlocks"));
        buttons.Add(startLevelButton);
        infinityLevelButton = GameObject.Find("InfinityLevelButton").GetComponent<Button>();
        infinityLevelButton.onClick.AddListener(() => OnClick("InfinityBlocks"));
        buttons.Add(infinityLevelButton);
    }

    private void OnClick(string pathToLevelBlocks)
    {
        Debug.Log(pathToLevelBlocks);
        HideCanvas();
        Model.StartNewGame(pathToLevelBlocks);
    }

    private void HideCanvas() => canvas.gameObject.SetActive(false);
    private void ShowCanvas() => canvas.gameObject.SetActive(true);
}