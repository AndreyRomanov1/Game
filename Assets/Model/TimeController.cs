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

    }

    public static void ChangePlayerState(PlayerStates oldState, PlayerStates newState)
    {
        if (oldState != newState)
        {
            if (newState is PlayerStates.CrouchedToJump
                or PlayerStates.CrouchedToJumpFromLeftWall
                or PlayerStates.CrouchedToJumpFromRightWall)
            {
                Time.timeScale = 0.4f;
                decelerationTime = 1;
            }
            else
                Time.timeScale = 1;
        }
    }

    public static void EnemyHasDetectedPlayerHandler()
    {
        Time.timeScale = 0.4f;
        decelerationTime = 1;
    }
}