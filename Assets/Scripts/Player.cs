using System;
using System.Collections;
using Unity.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 10f;
    public float jumpBoost = 900f;
    public float riftBoost = 50f;
    public float maxJumpForce = 10;
    public float minJumpForce = 2;

    public SpriteRenderer sprite;
    public Rigidbody2D physic;

    public Transform groundCheck1;
    public Transform groundCheck2;
    public LayerMask groundMask;

    private float currentTime = 0f;
    private const float JumpDurationTime = 1f;
    private const float RiftDurationTime = 1.5f;


    private bool IsReadyToJump =>
        playerState is not (PlayerState.CrouchedToJump or PlayerState.Jump or PlayerState.Rift);

    private Vector3 defaultCursorPosition;

    private PlayerState playerState = PlayerState.Nothing;

    private bool IsGrounded => Physics2D.OverlapCircle(groundCheck1.position,0.1f, groundMask)
                                || Physics2D.OverlapCircle(groundCheck2.position, 0.1f, groundMask);
    // private bool IsMoveBlocked => Physics2D.

    private MovementDirection direction = MovementDirection.Right;
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
            currentTime += Time.deltaTime;
            if (playerState == PlayerState.Jump && (currentTime >= JumpDurationTime || physic.velocity.magnitude < 0.01f))
                playerState = PlayerState.Nothing;
            if (playerState == PlayerState.Rift && (currentTime >= RiftDurationTime || physic.velocity.magnitude < 0.01f))
                playerState = PlayerState.Nothing;

            if (IsGrounded && IsReadyToJump)
                physic.velocity = Vector2.zero;
            JumpToCursorLogic();
            CharacterReversalForCursor();

            yield return null;
        }
    }
    
    // private void Update()
    // {
    //     currentTime += Time.deltaTime;
    //     if (playerState == PlayerState.Jump && (currentTime >= JumpDurationTime || physic.velocity.magnitude < 0.01f))
    //         playerState = PlayerState.Nothing;
    //     if (playerState == PlayerState.Rift && (currentTime >= RiftDurationTime || physic.velocity.magnitude < 0.01f))
    //         playerState = PlayerState.Nothing;
    //
    //     if (IsGrounded && IsReadyToJump)
    //         physic.velocity = Vector2.zero;
    //     JumpToCursorLogic();
    //     CharacterReversalForCursor();
    //     
    // }

    //TODO: Игра ломается при множественном нажатии пробела

    private void InitPlayerComponent()
    {
        sprite = GetComponent<SpriteRenderer>();
        physic = GetComponent<Rigidbody2D>();
        trajectory = GetComponentInChildren<TrajectoryRender>();
    }

    //TODO: исправить проблему ускорения при приземлении
    private void JumpToCursorLogic()
    {
        if (!IsGrounded)
            return;

        if (Input.GetKeyDown(KeyCode.Space) && IsReadyToJump)
        {
            CurrentGame.isSlowGame = true;
            playerState = PlayerState.CrouchedToJump;
            defaultCursorPosition = Input.mousePosition;
        }

        if (Input.GetKey(KeyCode.Space) && playerState == PlayerState.CrouchedToJump)
            trajectory.ShowTrajectory(GetJumpVector().vector * jumpBoost);

        if (Input.GetKeyUp(KeyCode.Space) && playerState == PlayerState.CrouchedToJump)
        {
            var (state, vector) = GetJumpVector();
            physic.AddForce(vector * jumpBoost);
            playerState = state;
            currentTime = 0f;
            CurrentGame.isSlowGame = false;
            defaultCursorPosition = default;
            trajectory.ClearTrajectory();
        }
    }

    private (PlayerState state, Vector3 vector) GetJumpVector()
    {
        var jumpVector = VectorFromBaseCursorPosition();
        var vectorAngle = Mathf.Atan2(jumpVector.y, jumpVector.x) * Mathf.Rad2Deg;
        return vectorAngle switch
        {
            > 20 and < 160 => (PlayerState.Jump, jumpVector),
            >= 160 or < -170 => (PlayerState.Rift, new Vector2(-1, 0).normalized * riftBoost),
            <= 20 and > -10 => (PlayerState.Rift, new Vector2(1, 0).normalized * riftBoost),
            _ => (PlayerState.Nothing, Vector3.zero)
        };
    }

    private void JumpModel(Action<Vector2> jumpAction)
    {
        // var jumpVector = VectorFromPlayer();
        var jumpVector = VectorFromBaseCursorPosition();
        var vectorAngle = Mathf.Atan2(jumpVector.y, jumpVector.x) * Mathf.Rad2Deg;

        switch (vectorAngle)
        {
            case < -10 and > -170:
                break;
            case > 20 and < 160:
                jumpAction(jumpVector * jumpBoost);
                break;
            case >= 160:
            case <= 20:
                jumpAction(new Vector2(vectorAngle is > 90 or < -90 ? -1 : 1, 0).normalized * (jumpBoost * 50));
                break;
        }
    }

    private Vector3 VectorFromBaseCursorPosition()
    {
        if (defaultCursorPosition == default)
            return default;

        var jumpVector = Input.mousePosition - defaultCursorPosition;
        jumpVector = jumpVector.normalized * Mathf.Pow(jumpVector.magnitude, 0.7f);
        if (jumpVector.magnitude > maxJumpForce)
            jumpVector = jumpVector.normalized * maxJumpForce;
        if (jumpVector.magnitude < minJumpForce)
            jumpVector = jumpVector.normalized * minJumpForce;
        return jumpVector;
    }

    private void CharacterReversalForCursor()
    {
        var directionVector = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        sprite.flipX = directionVector.x switch
        {
            < 0 => true,
            > 0 => false,
            _ => sprite.flipX
        };
    }

    // Old movement logic:
    private void MovementLogic()
    {
        if (!IsGrounded)
            return;

        var moveHorizontal = Input.GetAxis("Horizontal");

        direction = moveHorizontal switch
        {
            > 0 => MovementDirection.Right,
            < 0 => MovementDirection.Left,
            _ => direction
        };

        var movement = new Vector3((int)direction, 0);
        transform.Translate(movement * (CurrentGame.GameSpeed * speed * Time.fixedDeltaTime));
    }

    private void CharacterReversal()
    {
        if (!IsGrounded)
            return;

        var direction = Input.GetAxis("Horizontal");
        sprite.flipX = direction switch
        {
            < 0 => true,
            > 0 => false,
            _ => sprite.flipX
        };
    }

    private void JumpLogic()
    {
        if (Input.GetAxis("Jump") > 0 && IsGrounded)
            physic.AddForce(Vector3.up * jumpBoost);
    }

    private Vector3 VectorFromPlayer()
    {
        var cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return (cursorPosition - transform.position).normalized;
    }
}