using UnityEngine;

public static class LevelSelectionUIController
{
    private static readonly LevelSelectionCanvasScript LevelSelectionCanvas = GameScript.CreateByGameObject(GameScript.LoadByName("UI/LevelSelectionCanvas"))
        .GetComponent<LevelSelectionCanvasScript>();

    public static void Show()
    {
        UI.HideAllCanvas();
        UI.UICamera.gameObject.SetActive(true);
        LevelSelectionCanvas.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        UI.UICamera.gameObject.SetActive(false);
        LevelSelectionCanvas.gameObject.SetActive(false);
    }

    public static void StartNewGame(string pathToLevelBlocks)
    {
        UI.HideAllCanvas();
        Debug.Log(pathToLevelBlocks);
        CurrentGame.StartCurrentGame(pathToLevelBlocks);
    }
}