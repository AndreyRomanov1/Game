public static class PauseUIController
{
    
    private static readonly PauseCanvasScript PauseCanvas = GameScript.CreateByGameObject(GameScript.LoadByName("UI/PauseCanvas"))
        .GetComponent<PauseCanvasScript>();
    public static void Show()
    {
        UI.HideAllCanvas();
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
        UI.HideAllCanvas();
        Pause.EndPause();
    }

    public static void ExitCurrentGame()
    {
        UI.HideAllCanvas();
        CurrentGame.KillCurrentGame();
        LevelSelectionUIController.Show();
    }
}