public static class LoseUIController
{
    private static readonly ExitCurrentGameAndGameCanvasScript LostGameCanvas = GameScript.CreateByGameObject(GameScript.LoadByName("UI/LostGameCanvas"))
        .GetComponent<ExitCurrentGameAndGameCanvasScript>();

    public static void Show()
    {
        UI.HideAllCanvas();
        UI.UICamera.gameObject.SetActive(true);
        LostGameCanvas.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        UI.UICamera.gameObject.SetActive(false);
        LostGameCanvas.gameObject.SetActive(false);
    }
}