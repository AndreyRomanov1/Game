using System.Collections;
using UnityEngine;

public static class Pause
{
    public  static IEnumerator PauseCoroutine()
    {
        yield return new WaitForFixedUpdate();
        while (true)
        {
            switch (Model.GameState)
            {
                case GameStates.ActiveGame when Input.GetKeyUp(KeyCode.Escape):
                    StartPause();
                    break;
                case GameStates.Pause when Input.GetKeyUp(KeyCode.Escape):
                    EndPause();
                    break;
            }

            yield return null;
        }
    }

    public static void StartPause()
    {
        Debug.Log($"P1 {Model.GameState} {Input.GetKeyUp(KeyCode.Escape)}");
        GameSounds.StartPause();
        Model.GameState = GameStates.Pause;
        UI.Show(UI.PauseGameObject);
        CurrentGame.PlayerCamera.gameObject.SetActive(false);
    }

    public static void EndPause()
    {
        Debug.Log($"P2 {Model.GameState} {Input.GetKeyUp(KeyCode.Escape)}");
        GameSounds.EndPause();
        Model.GameState = GameStates.ActiveGame;
        UI.HideAllCanvas();
        CurrentGame.PlayerCamera.gameObject.SetActive(true);
    }
}