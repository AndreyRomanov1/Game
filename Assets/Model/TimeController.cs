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
            if (decelerationTime == 0)
                yield return null;
            var t = decelerationTime;
            decelerationTime = 0;
            yield return new WaitForSeconds(t);
            Time.timeScale = 1;
        }
    }

    public static void ChangeGameState(GameStates oldStates, GameStates newStates)
    {
        if (newStates is GameStates.Dialogue or GameStates.Pause or GameStates.StartGameMenu)
            Time.timeScale = 0;
        else if (newStates == GameStates.ActiveGame)
            Time.timeScale = 1;
        else
            Debug.Log($"Неизвестный для времени переход состояний игры {oldStates} => {newStates}");
    }

    public static void ChangePlayerState(PlayerStates oldState, PlayerStates newState)
    {
        if (oldState != newState)
        {
            if (newState is PlayerStates.CrouchedToJump
                or PlayerStates.CrouchedToJumpFromLeftWall
                or PlayerStates.CrouchedToJumpFromRightWall)
            {
                Time.timeScale = 0.2f;
                decelerationTime = 1;
            }
            else
                Time.timeScale = 1;
        }
    }

    public static void EnemyHasDetectedPlayerHandler()
    {
        Time.timeScale = 0.1f;
        decelerationTime = 1;
    }
}