using System.Collections;
using UnityEngine;


public static class TimeController
{
    private static float decelerationTime;
    private static float needToScaleTo = 1;
    private static bool letsJump = false;

    public static IEnumerator TimeSlowerCoroutine()
    {
        yield return new WaitForFixedUpdate();
        while (true)
        {
            yield return null;
            if (decelerationTime == 0)
                continue;

            while (Time.timeScale > needToScaleTo)
            {
                yield return new WaitForFixedUpdate();
                if (letsJump)
                {
                    break;
                    letsJump = false;
                }
                Debug.Log($"{Time.timeScale} - {(Time.fixedDeltaTime / 0.5f) * (1 - needToScaleTo)} = {Time.timeScale - (Time.fixedDeltaTime / 0.5f) * (1 - needToScaleTo)}");
                Time.timeScale -= (Time.fixedDeltaTime / 0.5f) * (1 - needToScaleTo);
            }
            
            var t = decelerationTime;
            decelerationTime = 0;
            yield return new WaitForSeconds(t);
            
            while (Time.timeScale < 1)
            {
                yield return new WaitForFixedUpdate();
                Time.timeScale += (0.3f / Time.fixedTime) * (1 - needToScaleTo);
            }
            Time.timeScale = 1;
            needToScaleTo = 1;
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
        if (oldState != newState && needToScaleTo > 0.4f && decelerationTime == 0)
        {
            if (newState is PlayerStates.CrouchedToJump
                or PlayerStates.CrouchedToJumpFromLeftWall
                or PlayerStates.CrouchedToJumpFromRightWall)
            {
                needToScaleTo = 0.4f;
                decelerationTime = 0.5f;
            }
            else
            {
                Time.timeScale = 1;
                letsJump = true;
            }
        }
    }
    
    public static void EnemyHasDetectedPlayerHandler()
    {
        needToScaleTo = 0.3f;
        decelerationTime = 2;
    }
}