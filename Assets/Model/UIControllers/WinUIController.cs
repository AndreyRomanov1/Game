public static class WinUIController
{
    private static readonly ExitCurrentGameAndGameCanvasScript WinEducationCanvas = GameScript.CreateByGameObject(GameScript.LoadByName("UI/WinEducationCanvas"))
        .GetComponent<ExitCurrentGameAndGameCanvasScript>();

    public static void Show()
    {
        UI.HideAllCanvas();
        UI.UICamera.gameObject.SetActive(true);
        WinEducationCanvas.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        UI.UICamera.gameObject.SetActive(false);
        WinEducationCanvas.gameObject.SetActive(false);
    }
}