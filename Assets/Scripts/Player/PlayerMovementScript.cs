using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// TODO: БАГ: иногда при прыжке в право игрок подпрыгивает на месте(только вверх).
// TODO: похоже причина бага в неправильном занулении горизонтальной составляющей вектора скорости, персонаж может остановиться и в полете

public class PlayerMovementScript : MonoBehaviour
{
    private const float JumpBoost = 120f;
    private const float MaxJumpForce = 10;
    private const float MinJumpForce = 0.7f;
    private const float RiftBoost = 150f;
    private const float MaxRiftForce = 8;
    private const float MinRiftForce = 0.7f;
    private const float MaxRiftDurationTime = 1.2f;

    private PlayerStates playerState = PlayerStates.Nothing;

    public PlayerStates PlayerState
    {
        get => playerState;
        set
        {
            PlayAnimation(playerState, value);
            TimeController.ChangePlayerState(playerState, value);
            playerState = value;
        }
    }

    private Transform[] groundCheckers;
    private Transform leftWallCheck;

    private Transform rightWallCheck;

    private float riftDurationTime = MaxRiftDurationTime;
    private Rigidbody2D Physic { get; set; }
    public LayerMask groundMask;
    private GameObject Tools { get; set; }

    private bool IsGrounded => groundCheckers
                                   .Any(groundCheck => Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundMask))
                               || CheckCollisionWithTag("Block") && IsGrounded2(1f);

    private bool IsGrounded2(float r) => groundCheckers
        .Any(groundCheck => Physics2D.OverlapCircle(groundCheck.position, r, groundMask));

    private bool IsTouchedLeftWall =>
        Physics2D.OverlapCircle(leftWallCheck.position, 0.2f, groundMask);

    private bool IsTouchedRightWall =>
        Physics2D.OverlapCircle(rightWallCheck.position, 0.2f, groundMask);

    private Dictionary<PlayerStates, Action> movementStateActions;

    private TrajectoryRenderScript trajectory;

    public void Start()
    {
        trajectory = GetComponentInChildren<TrajectoryRenderScript>();

        movementStateActions = new Dictionary<PlayerStates, Action>()
        {
            [PlayerStates.Nothing] = NothingMovementLogic,
            [PlayerStates.CrouchedToJump] = CrouchedToJumpMovementLogic,
            [PlayerStates.HangingOnRightWall] = HangingOnWallMovementLogic,
            [PlayerStates.HangingOnLeftWall] = HangingOnWallMovementLogic,
            [PlayerStates.CrouchedToJumpFromRightWall] = CrouchedToJumpFromWallMovementLogic,
            [PlayerStates.CrouchedToJumpFromLeftWall] = CrouchedToJumpFromWallMovementLogic,
        };


        groundCheckers = GameObject.FindGameObjectsWithTag("GroundCheck")
            .Select(x => x.transform)
            .ToArray();
        leftWallCheck = GameObject.Find("LeftWallCheck").transform;
        rightWallCheck = GameObject.Find("RightWallCheck").transform;
        Physic = GetComponent<Rigidbody2D>();
        Tools = transform.Find("Tools").gameObject;

        StartCoroutine(MovementCoroutine());
        StartCoroutine(MovementStateCoroutine());
    }

    private IEnumerator MovementCoroutine()
    {
        var h = 0;
        yield return new WaitForFixedUpdate();
        while (true)
        {
            if (Model.IsActiveGame)
                MovementLogic();

            if (Input.GetKeyUp(KeyCode.LeftShift))
                Print();
            if (Input.GetKeyUp(KeyCode.K) && h < 5) // Костыль когда застрял в блоке
            {
                Physic.AddForce(new Vector2(300, 400));
                h = 100;
            }

            h--;
            if (h < -100)
                h = 0;
            yield return null;
        }
    }

    private void Print() => Debug.Log(
        $"{PlayerState} {Model.GameState} {CheckCollisionWithTag("Block")} Time:{Time.timeScale} Касание:{IsGrounded} {IsTouchedRightWall} {IsTouchedLeftWall} Пробел: {Input.GetKeyDown(KeyCode.Space)} {Input.GetKeyUp(KeyCode.Space)} {Input.GetKey(KeyCode.Space)}");

    private bool CheckCollisionWithTag(string tagToCheck)
    {
        var colliders = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0f);

        return colliders.Any(collider => collider.CompareTag(tagToCheck));
    }

    private void MovementLogic()
    {
        if (Model.GameState == GameStates.ActiveGame
            && movementStateActions.TryGetValue(PlayerState, out var action))
            action();
    }

    private void CrouchedToJumpFromWallMovementLogic()
    {
        var velocity = Physic.velocity;
        Physic.velocity = new Vector2(velocity.x, velocity.y * 0.1f);

        var (state, vector) = GetWallMovementVector();
        if (Input.GetKey(KeyCode.Space))
            trajectory.ShowTrajectory(vector);
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Physic.AddForce(vector);
            PlayerState = state;
            trajectory.ClearTrajectory();
        }
    }

    private void HangingOnWallMovementLogic()
    {
        var velocity = Physic.velocity;
        Physic.velocity = new Vector2(velocity.x, velocity.y * 0.1f);

        if (Input.GetKeyDown(KeyCode.Space) && IsTouchedLeftWall)
            PlayerState = PlayerStates.CrouchedToJumpFromLeftWall;
        else if (Input.GetKeyDown(KeyCode.Space) && IsTouchedRightWall)
            PlayerState = PlayerStates.CrouchedToJumpFromRightWall;
    }

    private void CrouchedToJumpMovementLogic()
    {
        var (state, vector) = GetMovementVector();

        if (Input.GetKey(KeyCode.Space))
        {
            FlipPlayerToDirection(vector.x >= 0 ? Directions.Right : Directions.Left);
            trajectory.ShowTrajectory(vector);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            Debug.Log(vector);
            Physic.AddForce(vector);
            PlayerState = state;
            trajectory.ClearTrajectory();
        }
    }

    private void NothingMovementLogic()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded)
            PlayerState = PlayerStates.CrouchedToJump;
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
        riftDurationTime = MaxRiftDurationTime * (vector.magnitude / MaxRiftForce);
        return vector * RiftBoost;
    }

    private Vector2 GetPositionDirectionVector()
    {
        var cam = (Vector2)CurrentGame.PlayerCamera.ScreenToWorldPoint(Input.mousePosition);
        var position = (Vector2)transform.position;
        return cam - position;
    }

    private static float VectorAngle(Vector2 vector) => Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;

    private void FlipPlayerToDirection(Directions flipDirection)
    {
        var angle = 180 * (int)flipDirection;
        transform.localEulerAngles = new Vector3(0, angle, 0);
        Tools.transform.localEulerAngles = new Vector3(0, angle, 0);
    }

    private IEnumerator MovementStateCoroutine()
    {
        yield return new WaitForFixedUpdate();
        while (true)
        {
            for (var k0 = 0; PlayerState == PlayerStates.Nothing && k0 < 10; k0++)
            {
                if (!IsGrounded)
                    if (IsTouchedLeftWall)
                        PlayerState = PlayerStates.HangingOnLeftWall;
                    else if (IsTouchedRightWall)
                        PlayerState = PlayerStates.HangingOnRightWall;
                yield return null;
            }

            for (var k0 = 0;
                 PlayerState is PlayerStates.HangingOnLeftWall or PlayerStates.HangingOnRightWall &&
                 k0 < 10;
                 k0++)
            {
                if (IsGrounded)
                    PlayerState = PlayerStates.Nothing;
                yield return null;
            }

            if (PlayerState == PlayerStates.Rift)
            {
                Print();
                yield return new WaitForSeconds(riftDurationTime);
                if (!IsGrounded)
                    PlayerState = PlayerStates.Jump;
                else
                {
                    Physic.velocity = new Vector2(0, Physic.velocity.y);
                    PlayerState = PlayerStates.Nothing;
                }

                Print();
            }

            if (PlayerState is PlayerStates.HangingOnLeftWall or PlayerStates.HangingOnRightWall)
            {
                if (IsGrounded)
                    PlayerState = PlayerStates.Nothing;
                else if (!IsTouchedLeftWall && !IsTouchedRightWall)
                    PlayerState = PlayerStates.Jump;
            }

            if (PlayerState == PlayerStates.Jump)
            {
                Print();

                Debug.Log("1 Начало прыжка");
                var k = 0;
                for (; k < 15 && (IsGrounded || IsTouchedLeftWall || IsTouchedRightWall); k++)
                    yield return null; // Ждём, пока достаточно далеко отлетим от стены
                Debug.Log($"2 Оторвал ноги от земли или стены. Был возле стены {k} тиков");
                while (!IsGrounded && !IsTouchedLeftWall && !IsTouchedRightWall)
                    yield return null; // Когда коснёмся поверхности, обнуляем X у скорости
                Debug.Log("3 Коснулся земли или стены");
                Physic.velocity = new Vector2(0, Physic.velocity.y);
                if (IsGrounded)
                    PlayerState = PlayerStates.Nothing;
                else if (IsTouchedLeftWall)
                    PlayerState = PlayerStates.HangingOnLeftWall;
                else if (IsTouchedRightWall)
                    PlayerState = PlayerStates.HangingOnRightWall;
                Debug.Log("4 Конец прыжка");
                Print();
            }

            yield return null;
        }
    }
}