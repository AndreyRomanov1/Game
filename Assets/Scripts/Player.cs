using System;
using System.Collections;
using Unity.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    private const float JumpBoost = 240f;
    private const float MaxJumpForce = 4;
    private const float MinJumpForce = 1;
    private const float RiftBoost = 200f;
    private const float MaxRiftForce = 4;
    private const float MinRiftForce = 1;

    public SpriteRenderer sprite;
    public Rigidbody2D physic;

    public Transform groundCheck1;
    public Transform groundCheck2;
    public LayerMask groundMask;

    private const float JumpDurationTime = 0.001f;
    private const float RiftDurationTime = 0.4f;


    private bool IsReadyToMovement =>
        playerState is not (PlayerState.CrouchedToJump or PlayerState.Jump or PlayerState.Rift);

    // private Vector3 defaultCursorPosition;

    private PlayerState playerState = PlayerState.Nothing;

    private bool IsGrounded => Physics2D.OverlapCircle(groundCheck1.position, 0.2f, groundMask)
                               || Physics2D.OverlapCircle(groundCheck2.position, 0.2f, groundMask);
    // private bool IsMoveBlocked => Physics2D.

    private TrajectoryRender trajectory;

    private void Start()
    {
        InitPlayerComponent();
        StartCoroutine(UpdateCoroutine());
    }

    private IEnumerator UpdateCoroutine()
    {
        while (true)
        {
            MovementLogic();
            yield return null;
        }
    }

    // TODO: Игра ломается при множественном нажатии пробела
    private void InitPlayerComponent()
    {
        sprite = GetComponent<SpriteRenderer>();
        physic = GetComponent<Rigidbody2D>();
        trajectory = GetComponentInChildren<TrajectoryRender>();
    }

    // TODO: исправить проблему ускорения при приземлении
    // TODO: БАГ: иногда при прыжке в право игрок подпрыгивает на месте(только вверх). Влево такое не замечал, но тоже возможно
    private void MovementLogic()
    {
        if (!IsGrounded)
            return;

        if (Input.GetKeyDown(KeyCode.Space) && IsReadyToMovement)
        {
            CurrentGame.isSlowGame = true;
            playerState = PlayerState.CrouchedToJump;
            // defaultCursorPosition = Input.mousePosition;
        }

        if (Input.GetKey(KeyCode.Space) && playerState == PlayerState.CrouchedToJump)
            trajectory.ShowTrajectory(GetMovementVector().vector);

        if (Input.GetKeyUp(KeyCode.Space) && playerState == PlayerState.CrouchedToJump)
        {
            var (state, vector) = GetMovementVector();
            physic.AddForce(vector);
            playerState = state;
            StartCoroutine(StopJumpOrRift());
            CurrentGame.isSlowGame = false;
            // defaultCursorPosition = default;
            trajectory.ClearTrajectory();
        }
    }
    
    private (PlayerState state, Vector3 vector) GetMovementVector()
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
                state = PlayerState.Nothing;
                vector = Vector3.zero;
                break;
        }

        sprite.flipX = vector.x <= 0;
        return (state, vector);
    }

    private static Vector3 GetJumpVector(Vector3 vector)
    {
        // vector = vector.normalized * Mathf.Pow(vector.magnitude, 0.7f);
        if (vector.magnitude > MaxJumpForce)
            vector = vector.normalized * MaxJumpForce;
        return vector * JumpBoost;
    }

    private static Vector3 GetRiftVector(Vector3 vector)
    {
        // vector = vector.normalized * Mathf.Pow(vector.magnitude, 0.7f);
        if (vector.magnitude > MaxRiftForce)
            vector = vector.normalized * MaxRiftForce;
        return vector * RiftBoost;
    }

    private IEnumerator StopJumpOrRift()
    {
        if (playerState == PlayerState.Rift)
        {
            yield return new WaitForSeconds(RiftDurationTime);
            physic.velocity= new Vector2(0, physic.velocity.y); 
        }
        if (playerState == PlayerState.Jump)
        {
            // TODO: Нужно как-то чётко отслеживать, прыгает ли игрок или уже приземлился. Просто смотреть на землю плохо работает
        }
        playerState = PlayerState.Nothing;
    }


    private Vector3 GetPositionDirectionVector()
    {
        var c = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var position = transform.position;
        return new Vector2(c.x - position.x, c.y - position.y);
    }

    private static float VectorAngle(Vector2 vector) => Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;

    // // переписать как GetPositionDirectionVector, если будешь использовать!!!
    // private Vector3 GetMouseDirectionVector() =>
    //     defaultCursorPosition == default
    //         ? default
    //         : Camera.main.ScreenToWorldPoint(Input.mousePosition) - defaultCursorPosition;

    // private (PlayerState state, Vector3 vector) GetJumpVector()
    // {
    //     // var jumpVector = VectorFromPlayer();
    //     var jumpVector = VectorFromBaseCursorPosition();
    //     var vectorAngle = Mathf.Atan2(jumpVector.y, jumpVector.x) * Mathf.Rad2Deg;
    //     return vectorAngle switch
    //     {
    //         > 20 and < 160 => (PlayerState.Jump, jumpVector),
    //         >= 160 or < -150 => (PlayerState.Rift, new Vector2(-1, 0).normalized * RiftBoost),
    //         <= 20 and > -30 => (PlayerState.Rift, new Vector2(1, 0).normalized * RiftBoost),
    //         _ => (PlayerState.Nothing, Vector3.zero)
    //     };
    // }
    //
    // private Vector3 VectorFromBaseCursorPosition()
    // {
    //     if (defaultCursorPosition == default)
    //         return default;
    //
    //     var vector = GetMouseDirectionVector();
    //
    //     vector = vector.normalized * Mathf.Pow(vector.magnitude, 0.7f);
    //     if (vector.magnitude > MaxJumpForce)
    //         vector = vector.normalized * MaxJumpForce;
    //     if (vector.magnitude < MinJumpForce)
    //         vector = vector.normalized * MinJumpForce;
    //     return vector;
    // }
    //
    // private void CharacterReversalForCursor()
    // {
    //     var directionVector = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
    //     sprite.flipX = directionVector.x switch
    //     {
    //         < 0 => true,
    //         > 0 => false,
    //         _ => sprite.flipX
    //     };
    // }

    // Old movement logic:
    // private void OldMovementLogic()
    // {
    //     if (!IsGrounded)
    //         return;
    //
    //     var moveHorizontal = Input.GetAxis("Horizontal");
    //
    //     direction = moveHorizontal switch
    //     {
    //         > 0 => MovementDirection.Right,
    //         < 0 => MovementDirection.Left,
    //         _ => direction
    //     };
    //
    //     var movement = new Vector3((int)direction, 0);
    //     transform.Translate(movement * (CurrentGame.GameSpeed * speed * Time.fixedDeltaTime));
    // }
    //
    // private void CharacterReversal()
    // {
    //     if (!IsGrounded)
    //         return;
    //
    //     var direction = Input.GetAxis("Horizontal");
    //     sprite.flipX = direction switch
    //     {
    //         < 0 => true,
    //         > 0 => false,
    //         _ => sprite.flipX
    //     };
    // }
    //
    // private void JumpLogic()
    // {
    //     if (Input.GetAxis("Jump") > 0 && IsGrounded)
    //         physic.AddForce(Vector3.up * JumpBoost);
    // }
    //
    // private Vector3 VectorFromPlayer()
    // {
    //     var cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //     return (cursorPosition - transform.position).normalized;
    // }
}