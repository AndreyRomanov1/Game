using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 10f;
    public float jumpBoost = 900f;
    public float maxJumpForce = 10;
    public float minJumpForce = 2;

    public SpriteRenderer sprite;
    public Rigidbody2D physic;

    public Transform groundCheck1;
    public Transform groundCheck2;
    public LayerMask groundMask;

    public float groundRadius = 0.3f;

    private bool isReadyToJump;
    private Vector3 defaultCursorPosition;

    private bool IsGrounded => Physics2D.OverlapCircle(groundCheck1.position, groundRadius, groundMask)
                               || Physics2D.OverlapCircle(groundCheck2.position, groundRadius, groundMask);
    // private bool IsMoveBlocked => Physics2D.

    private MovementDirection direction = MovementDirection.Right;
    private TrajectoryRender Trajectory;

    private void Start()
    {
        InitPlayerComponent();
    }

    private void Update()
    {
        JumpToCursorLogic();
    }

    //TODO: Игра ломается при множественном нажатии пробела

    private void FixedUpdate()
    {
        // MovementLogic();
        // CharacterReversal();

        CharacterReversalForCursor();
        
        //MovementLogic();
        //CharacterReversal();
        
        // JumpLogic();
    }

    private void InitPlayerComponent()
    {
        sprite = GetComponent<SpriteRenderer>();
        physic = GetComponent<Rigidbody2D>();
        Trajectory = GetComponentInChildren<TrajectoryRender>();
    }
    

    private void JumpLogic()
    {
        if (Input.GetAxis("Jump") > 0 && IsGrounded)
            physic.AddForce(Vector3.up * jumpBoost);
    }

    //TODO: исправить проблему ускорения при приземлении
    private void JumpToCursorLogic()
    {
        if (!IsGrounded)
            return;

        if (Input.GetKeyDown(KeyCode.Space) && !isReadyToJump)
        {
            CurrentGame.isSlowGame = true;
            isReadyToJump = true;
            defaultCursorPosition = Input.mousePosition;
        }

        if (Input.GetKeyUp(KeyCode.Space) && isReadyToJump)
        if (Input.GetKey(KeyCode.Space))
        {
            JumpModel(Trajectory.ShowTrajectory);
        }

        if (Input.GetKeyUp(KeyCode.Space) && isTimeFrozen)
        {
            CurrentGame.isSlowGame = false;
            isReadyToJump = false;

            // var jumpVector = VectorFromPlayer();
            var jumpVector = VectorFromBaseCursorPosition();
            
            var vectorAngle = Mathf.Atan2(jumpVector.y, jumpVector.x) * Mathf.Rad2Deg;
            JumpModel(physic.AddForce);

            defaultCursorPosition = default;
            Trajectory.ClearTrajectory();
        }
    }


    private void JumpModel(Action<Vector2> function)
    {
        // var jumpVector = VectorFromPlayer();
        var jumpVector = VectorFromBaseCursorPosition();
        
        var vectorAngle = Mathf.Atan2(jumpVector.y, jumpVector.x) * Mathf.Rad2Deg;

        if (vectorAngle is > 20 and < 160)
            function(jumpVector * jumpBoost);
    }


    private Vector3 VectorFromPlayer()
    {
        var cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return (cursorPosition - transform.position).normalized;
    }

    private Vector3 VectorFromBaseCursorPosition()
    {
        if (defaultCursorPosition == default) return default;

        var jumpVector = Input.mousePosition - defaultCursorPosition;
        if (jumpVector.magnitude > maxJumpForce)
            jumpVector = jumpVector.normalized * maxJumpForce;
        if (jumpVector.magnitude < minJumpForce)
            jumpVector = jumpVector.normalized * minJumpForce;
        return jumpVector;
    }

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

    public void TakeDamage(float damage)
    {
        return;
    }
}