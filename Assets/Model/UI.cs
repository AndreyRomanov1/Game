using UnityEngine;

public static class UI
{
    private static readonly Camera UICamera = GameScript.CreateByGameObject(GameScript.LoadByName("Cameras/UICamera"))
        .GetComponent<Camera>();

    public static readonly MainCanvasScript MainCanvas = GameScript
        .CreateByGameObject(GameScript.LoadByName("UI/MainCanvas"))
        .GetComponent<MainCanvasScript>();

    public static readonly GameObject LevelSelectionGameObject = MainCanvas.transform.Find("LevelSelection").gameObject;
    public static readonly GameObject PauseGameObject = MainCanvas.transform.Find("Pause").gameObject;
    public static readonly GameObject LostGameGameObject = MainCanvas.transform.Find("LostGame").gameObject;
    public static readonly GameObject WinEducationGameObject = MainCanvas.transform.Find("WinEducation").gameObject;

    public static void Show(GameObject uiGameObject)
    {
        HideAllCanvas();
        UICamera.gameObject.SetActive(true);
        MainCanvas.gameObject.SetActive(true);
        uiGameObject.SetActive(true);
    }

    public static void ExitGame()
    {
        GameSounds.ButtonPress();
        Application.Quit();
    }

    public static void HideAllCanvas()
    {
        UICamera.gameObject.SetActive(false);
        LevelSelectionGameObject.SetActive(false);
        PauseGameObject.SetActive(false);
        LostGameGameObject.SetActive(false);
        WinEducationGameObject.SetActive(false);
        MainCanvas.gameObject.SetActive(false);
    }

    public static void StartNewGame(string pathToLevelBlocks)
    {
        HideAllCanvas();
        GameSounds.ButtonPress();
        Debug.Log(pathToLevelBlocks);
        Model.StartCurrentGame(pathToLevelBlocks);
    }

    public static void EndPause()
    {
        UI.HideAllCanvas();
        Pause.EndPause();
    }

    public static void ExitCurrentGame()
    {
        GameSounds.ButtonPress();
        UI.HideAllCanvas();
        CurrentGame.KillCurrentGame();
        UI.Show(UI.LevelSelectionGameObject);
    }
}