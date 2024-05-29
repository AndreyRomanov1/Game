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
                    Debug.Log($"P1 {Model.GameState} {Input.GetKeyUp(KeyCode.Escape)}");

                    Model.GameState = GameStates.Pause;
                    PauseUIController.Show();
                    CurrentGame.PlayerCamera.gameObject.SetActive(false);
                    break;
                case GameStates.Pause when Input.GetKeyUp(KeyCode.Escape):
                    Debug.Log($"P2 {Model.GameState} {Input.GetKeyUp(KeyCode.Escape)}");
                    Model.GameState = GameStates.ActiveGame;
                    PauseUIController.Hide();
                    CurrentGame.PlayerCamera.gameObject.SetActive(true);
                    break;
            }

            yield return null;
        }
    }
}