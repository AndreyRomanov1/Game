using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: БАГ: иногда при прыжке в право игрок подпрыгивает на месте(только вверх).
// TODO: похоже причина бага в неправильном занулении горизонтальной составляющей вектора скорости, персонаж может остановиться и в полете

public class MovementPlayer
{
    private const float JumpBoost = 240f;
    private const float MaxJumpForce = 5;
    private const float MinJumpForce = 0.7f;
    private const float RiftBoost = 300f;
    private const float MaxRiftForce = 4;
    private const float MinRiftForce = 0.7f;

    private const float MaxRiftDurationTime = 1.2f;

    // private float riftDurationTime = MaxRiftDurationTime;
    private readonly Dictionary<PlayerStates, Action> movementStateActions;
    private readonly PlayerScript player;

    private readonly TrajectoryRenderScript trajectory;

    public MovementPlayer(PlayerScript player)
    {
        this.player = player;
        trajectory = player.GetComponentInChildren<TrajectoryRenderScript>();

        movementStateActions = new Dictionary<PlayerStates, Action>()
        {
            [PlayerStates.Nothing] = NothingMovementLogic,
            [PlayerStates.CrouchedToJump] = CrouchedToJumpMovementLogic,
            [PlayerStates.HangingOnRightWall] = HangingOnWallMovementLogic,
            [PlayerStates.HangingOnLeftWall] = HangingOnWallMovementLogic,
            [PlayerStates.CrouchedToJumpFromRightWall] = CrouchedToJumpFromWallMovementLogic,
            [PlayerStates.CrouchedToJumpFromLeftWall] = CrouchedToJumpFromWallMovementLogic,
        };
    }

    public IEnumerator MovementCoroutine()
    {
        yield return new WaitForFixedUpdate();
        while (true)
        {
            MovementLogic();
            yield return null;
        }
    }

    private void MovementLogic()
    {
        if (Model.GameState == GameState.ActiveGame
            && movementStateActions.TryGetValue(player.PlayerState, out var action))
            action();
    }

    private void CrouchedToJumpFromWallMovementLogic()
    {
        var velocity = player.physic.velocity;
        player.physic.velocity = new Vector2(velocity.x, velocity.y * 0.1f);

        var (state, vector) = GetWallMovementVector();
        if (Input.GetKey(KeyCode.Space))
            trajectory.ShowTrajectory(vector);
        if (Input.GetKeyUp(KeyCode.Space))
        {
            player.physic.AddForce(vector);
            player.PlayerState = state;
            trajectory.ClearTrajectory();
        }
    }

    private void HangingOnWallMovementLogic()
    {
        var velocity = player.physic.velocity;
        player.physic.velocity = new Vector2(velocity.x, velocity.y * 0.1f);

        if (Input.GetKeyDown(KeyCode.Space) && player.IsTouchedLeftWall)
            player.PlayerState = PlayerStates.CrouchedToJumpFromLeftWall;
        else if (Input.GetKeyDown(KeyCode.Space) && player.IsTouchedRightWall)
            player.PlayerState = PlayerStates.CrouchedToJumpFromRightWall;
    }

    private void CrouchedToJumpMovementLogic()
    {
        var (state, vector) = GetMovementVector();

        if (Input.GetKey(KeyCode.Space))
        {
            player.FlipPlayerToDirection(vector.x >= 0 ? Directions.Right : Directions.Left);
            trajectory.ShowTrajectory(vector);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            player.physic.AddForce(vector);
            player.PlayerState = state;
            trajectory.ClearTrajectory();
        }
    }

    private void NothingMovementLogic()
    {
        if (Input.GetKeyDown(KeyCode.Space) && player.IsGrounded)
            player.PlayerState = PlayerStates.CrouchedToJump;
    }

    private (PlayerStates state, Vector2 vector) GetWallMovementVector()
    {
        var vector = GetPositionDirectionVector();
        vector = GetJumpVector(vector);
        var state = PlayerStates.Jump;

        if (vector.magnitude < 1e-3)
            state = PlayerStates.Nothing;
        return (state, vector);
    }

    private (PlayerStates state, Vector2 vector) GetMovementVector()
    {
        var vector = GetPositionDirectionVector();
        var vectorAngle = VectorAngle(vector);
        PlayerStates states;
        switch (vectorAngle)
        {
            case > 20 and < 160:
                vector = GetJumpVector(vector);
                states = PlayerStates.Jump;
                break;
            case >= 160 or < -150:
            case <= 20 and > -30:
                vector = GetRiftVector(vector);
                states = PlayerStates.Rift;
                break;
            default:
                vector = Vector2.zero;
                states = PlayerStates.Nothing;
                break;
        }

        if (vector.magnitude < 1e-3)
            states = PlayerStates.Nothing;

        return (states, vector);
    }

    private static Vector2 GetJumpVector(Vector2 vector)
    {
        if (vector.magnitude > MaxJumpForce)
            vector = vector.normalized * MaxJumpForce;
        else if (vector.magnitude < MinJumpForce)
            vector = Vector2.zero;
        return vector * JumpBoost;
    }

    private Vector2 GetRiftVector(Vector2 vector)
    {
        if (vector.magnitude > MaxRiftForce)
            vector = vector.normalized * MaxRiftForce;
        else if (vector.magnitude < MinRiftForce)
            vector = Vector2.zero;
        player.riftDurationTime = MaxRiftDurationTime * (vector.magnitude / MaxRiftForce);
        return vector * RiftBoost;
    }

    private Vector2 GetPositionDirectionVector()
    {
        var cam = (Vector2)CurrentGame.PlayerCamera.ScreenToWorldPoint(Input.mousePosition);
        var position = (Vector2)player.transform.position;
        return cam - position;
    }

    private static float VectorAngle(Vector2 vector) => Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
}