using System.Collections;
using System.Linq;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private const float JumpBoost = 240f;
    private const float MaxJumpForce = 4;
    private const float MinJumpForce = 0.7f;
    private const float RiftBoost = 300f;
    private const float MaxRiftForce = 4;
    private const float MinRiftForce = 0.7f;

    private SpriteRenderer sprite;
    private Rigidbody2D physic;

    private Transform[] groundCheckers;
    private Transform leftWallCheck;
    private Transform rightWallCheck;
    public LayerMask groundMask;

    private const float RiftDurationTime = 0.8f;
    public PlayerState playerState = PlayerState.Nothing;

    private bool IsGrounded => groundCheckers
        .Any(groundCheck => Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundMask));

    private bool IsTouchedLeftWall =>
        Physics2D.OverlapCircle(leftWallCheck.position, 0.2f, groundMask);

    private bool IsTouchedRightWall =>
        Physics2D.OverlapCircle(rightWallCheck.position, 0.2f, groundMask);

    private TrajectoryRenderScript trajectory;

    private void Start()
    {
        InitPlayerComponent();
        StartCoroutine(UpdateCoroutine());
        StartCoroutine(MovementStateController());
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            Debug.Log("COLLISION");
        }
    }

    private void Print() =>
        Debug.Log(
            $"{playerState} {IsGrounded} {IsTouchedRightWall} {IsTouchedLeftWall} {Input.GetKeyDown(KeyCode.Space)} {Input.GetKeyUp(KeyCode.Space)} {Input.GetKey(KeyCode.Space)}");

    private IEnumerator UpdateCoroutine()
    {
        yield return new WaitForFixedUpdate();
        while (true)
        {
            MovementLogic();
            yield return null;
        }
    }

    private void InitPlayerComponent()
    {
        sprite = GetComponent<SpriteRenderer>();
        physic = GetComponent<Rigidbody2D>();
        trajectory = GetComponentInChildren<TrajectoryRenderScript>();
        groundCheckers = GameObject.FindGameObjectsWithTag("GroundCheck")
            .Select(x => x.transform)
            .ToArray();
        leftWallCheck = GameObject.Find("LeftWallCheck").transform;
        rightWallCheck = GameObject.Find("RightWallCheck").transform;
    }

    // TODO: БАГ: иногда при прыжке в право игрок подпрыгивает на месте(только вверх). Влево такое не замечал, но тоже возможно
    // TODO: похоже причина бага в неправильном занулении горизонтальной составляющей вектора скорости, персонаж может остановиться и в полете
    private void MovementLogic()
    {
        if (playerState == PlayerState.Nothing)
        {
            if (Input.GetKeyDown(KeyCode.Space) && IsGrounded)
                playerState = PlayerState.CrouchedToJump;
        }
        else if (playerState == PlayerState.CrouchedToJump)
        {
            var (state, vector) = GetMovementVector();
            sprite.flipX = vector.x <= 0;

            if (Input.GetKey(KeyCode.Space))
                trajectory.ShowTrajectory(vector);

            if (Input.GetKeyUp(KeyCode.Space))
            {
                physic.AddForce(vector);
                playerState = state;
                trajectory.ClearTrajectory();
            }
        }
        else if (playerState is PlayerState.HangingOnRightWall or PlayerState.HangingOnLeftWall)
        {
            var velocity = physic.velocity;
            physic.velocity = new Vector2(velocity.x, velocity.y * 0.1f);

            if (Input.GetKeyDown(KeyCode.Space) && (IsTouchedLeftWall || IsTouchedRightWall))
                playerState = PlayerState.CrouchedToJumpFromWall;
        }
        else if (playerState == PlayerState.CrouchedToJumpFromWall)
        {
            var velocity = physic.velocity;
            physic.velocity = new Vector2(velocity.x, velocity.y * 0.1f);

            var (state, vector) = GetWallMovementVector();
            sprite.flipX = vector.x <= 0;

            if (Input.GetKey(KeyCode.Space))
                trajectory.ShowTrajectory(vector);

            if (Input.GetKeyUp(KeyCode.Space))
            {
                physic.AddForce(vector);
                playerState = state;
                trajectory.ClearTrajectory();
            }
        }
    }
    // TODO: оптимизировать функции прыжка с земли и со стены, убрав повторяющейся код 

    private (PlayerState state, Vector2 vector) GetWallMovementVector()
    {
        var vector = GetPositionDirectionVector();
        var vectorAngle = VectorAngle(vector);
        var state = playerState;
        if (vectorAngle is > -70 and < 70 or < -110 or > 110)
        {
            vector = GetJumpVector(vector);
            state = PlayerState.Jump;
        }
        else
            vector = Vector2.zero;

        if (vector.magnitude < 1e-3)
            state = PlayerState.Nothing;
        return (state, vector);
    }

    private (PlayerState state, Vector2 vector) GetMovementVector()
    {
        var vector = GetPositionDirectionVector();
        var vectorAngle = VectorAngle(vector);
        PlayerState state;
        switch (vectorAngle)
        {
            case > 20 and < 160:
                vector = GetJumpVector(vector);
                state = PlayerState.Jump;
                break;
            case >= 160 or < -150:
            case <= 20 and > -30:
                vector = GetRiftVector(vector);
                state = PlayerState.Rift;
                break;
            default:
                vector = Vector2.zero;
                state = PlayerState.Nothing;
                break;
        }

        if (vector.magnitude < 1e-3)
            state = PlayerState.Nothing;

        return (state, vector);
    }

    private static Vector2 GetJumpVector(Vector2 vector)
    {
        if (vector.magnitude > MaxJumpForce)
            vector = vector.normalized * MaxJumpForce;
        else if (vector.magnitude < MinJumpForce)
            vector = Vector2.zero;
        return vector * JumpBoost;
    }

    private static Vector2 GetRiftVector(Vector2 vector)
    {
        if (vector.magnitude > MaxRiftForce)
            vector = vector.normalized * MaxRiftForce;
        else if (vector.magnitude < MinRiftForce)
            vector = Vector2.zero;
        return vector * RiftBoost;
    }

    private IEnumerator MovementStateController()
    {
        yield return new WaitForFixedUpdate();
        while (true)
        {
            if (playerState == PlayerState.Rift)
            {
                Print();

                yield return new WaitForSeconds(RiftDurationTime);
                if (!IsGrounded)
                    playerState = PlayerState.Jump;
                else
                {
                    physic.velocity = new Vector2(0, physic.velocity.y);
                    playerState = PlayerState.Nothing;
                }

                Print();
            }

            if (playerState is PlayerState.HangingOnLeftWall or PlayerState.HangingOnRightWall)
            {
                if (IsGrounded)
                    playerState = PlayerState.Nothing;
                else if (!IsTouchedLeftWall && !IsTouchedRightWall)
                    playerState = PlayerState.Jump;
            }

            if (playerState == PlayerState.Jump)
            {
                Print();

                Debug.Log("1 Начало прыжка");
                while (IsGrounded || IsTouchedLeftWall || IsTouchedRightWall)
                    yield return null; // Ждём, пока достаточно далеко отлетим от стены
                Debug.Log("2 Оторвал ноги от земли или стены");
                while (!IsGrounded && !IsTouchedLeftWall && !IsTouchedRightWall)
                    yield return null; // Когда коснёмся поверхности, обнуляем X у скорости
                Debug.Log("3 Коснулся земли или стены");
                physic.velocity = new Vector2(0, physic.velocity.y);
                if (IsGrounded)
                    playerState = PlayerState.Nothing;
                else if (IsTouchedLeftWall)
                    playerState = PlayerState.HangingOnLeftWall;
                else if (IsTouchedRightWall)
                    playerState = PlayerState.HangingOnRightWall;
                Debug.Log("4 Конец прыжка");
                Print();
            }

            yield return null;
        }
    }

    private Vector2 GetPositionDirectionVector()
    {
        var cam = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var position = (Vector2)transform.position;
        return cam - position;
    }

    private static float VectorAngle(Vector2 vector) => Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
}