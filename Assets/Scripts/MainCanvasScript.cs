using UnityEngine;
using UnityEngine.UI;

public class MainCanvasScript : MonoBehaviour
{
    public void Init()
    {
        var continueGameButton = GameObject.Find("Pause/ContinueGameButton").GetComponent<Button>();
        continueGameButton.onClick.AddListener(UI.EndPause);

        foreach (var t in GameObject.FindGameObjectsWithTag("ExitGameButton"))
            t.GetComponent<Button>().onClick.AddListener(UI.ExitGame);
        foreach (var t in GameObject.FindGameObjectsWithTag("ExitCurrentGameButton"))
            t.GetComponent<Button>().onClick.AddListener(UI.ExitCurrentGame);
        
        var startLevelButton = GameObject.Find("LevelSelection/StartLevelButton").GetComponent<Button>();
        startLevelButton.onClick.AddListener(() => UI.StartNewGame("StartBlocks"));
        var infinityLevelButton = GameObject.Find("LevelSelection/InfinityLevelButton").GetComponent<Button>();
        infinityLevelButton.onClick.AddListener(() => UI.StartNewGame("InfinityBlocks"));
    }
}