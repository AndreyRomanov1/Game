using UnityEngine;

public static class PauseUIController
{
    
    private static readonly PauseCanvasScript PauseCanvas = GameScript.CreateByGameObject(GameScript.LoadByName("UI/PauseCanvas"))
        .GetComponent<PauseCanvasScript>();
    public static void Show()
    {
        UI.UICamera.gameObject.SetActive(true);
        PauseCanvas.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        UI.UICamera.gameObject.SetActive(false);
        PauseCanvas.gameObject.SetActive(false);
    }

    public static void EndPause()
    {
        Hide();
        Pause.EndPause();
    }

    public static void ExitCurrentGame()
    {
        Hide();
        CurrentGame.KillCurrentGame();
        LevelSelectionUIController.Show();
    }
}