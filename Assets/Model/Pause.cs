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
                case GameState.ActiveGame when Input.GetKeyUp(KeyCode.Escape):
                    Debug.Log($"P1 {Model.GameState} {Input.GetKeyUp(KeyCode.Escape)}");

                    Model.GameState = GameState.Pause;
                    PauseUIController.Show();
                    CurrentGame.PlayerCamera.gameObject.SetActive(false);
                    Time.timeScale = 0;
                    break;
                case GameState.Pause when Input.GetKeyUp(KeyCode.Escape):
                    Debug.Log($"P2 {Model.GameState} {Input.GetKeyUp(KeyCode.Escape)}");
                    Model.GameState = GameState.ActiveGame;
                    PauseUIController.Hide();
                    CurrentGame.PlayerCamera.gameObject.SetActive(true);
                    Time.timeScale = 1;
                    break;
            }

            yield return null;
        }
    }
}