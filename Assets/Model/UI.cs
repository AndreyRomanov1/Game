using UnityEngine;

public static class UI
{
    public static readonly Camera UICamera = GameScript.CreateByGameObject(GameScript.LoadByName("Cameras/UICamera"))
            .GetComponent<Camera>();

    public static void ExitGame()
    {
        Application.Quit();
    }

    public static void HideAllCanvas()
    {
        LevelSelectionUIController.Hide();
        LoseUIController.Hide();
        PauseUIController.Hide();
        WinUIController.Hide();
    }
}