using System.Collections;
using UnityEngine;


public static class TimeController
{
    private static int decelerationTime;

    public static IEnumerator TimeSlowerCoroutine()
    {
        yield return new WaitForFixedUpdate();
        while (true)
        {
            yield return null;
            if (decelerationTime == 0)
                continue;
            var t = decelerationTime;
            decelerationTime = 0;
            yield return new WaitForSeconds(t);
            Time.timeScale = 1;
            if (CurrentGame.Player != null && CurrentGame.Player.timeFreeze != null)
                CurrentGame.Player.timeFreeze.SetActive(false);
        }
    }

    public static void ChangeGameState(GameStates oldState, GameStates newState)
    {
        if (newState is GameStates.Dialogue or GameStates.Pause or GameStates.StartGameMenu)
            Time.timeScale = 0;
        else if (newState == GameStates.ActiveGame)
            Time.timeScale = 1;
        else
            Debug.Log($"Неизвестный для времени переход состояний игры {oldState} => {newState}");
        if (CurrentGame.Player != null && CurrentGame.Player.timeFreeze != null)
            CurrentGame.Player.timeFreeze.SetActive(false);
    }

    public static void ChangePlayerState(PlayerStates oldState, PlayerStates newState)
    {
        if (oldState != newState)
        {
            if (newState is PlayerStates.CrouchedToJump
                or PlayerStates.CrouchedToJumpFromLeftWall
                or PlayerStates.CrouchedToJumpFromRightWall)
            {
                if (CurrentGame.Player != null && CurrentGame.Player.timeFreeze != null)
                    CurrentGame.Player.timeFreeze.SetActive(true);
                Time.timeScale = 0.4f;
                decelerationTime = 1;
            }
            else
            {
                if (CurrentGame.Player != null && CurrentGame.Player.timeFreeze != null)
                    CurrentGame.Player.timeFreeze.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }

    public static void EnemyHasDetectedPlayerHandler()
    {
        if (CurrentGame.Player != null && CurrentGame.Player.timeFreeze != null)
            CurrentGame.Player.timeFreeze.SetActive(true);
        Time.timeScale = 0.3f;
        decelerationTime = 2;
    }
}