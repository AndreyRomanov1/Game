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

    private bool isTimeFrozen = false;
    private Vector3 defaultCursorPosition = default;

    private bool IsGrounded => Physics2D.OverlapCircle(groundCheck1.position, groundRadius, groundMask) 
                               || Physics2D.OverlapCircle(groundCheck2.position, groundRadius, groundMask);
    // private bool IsMoveBlocked => Physics2D.

    private MovementDirection direction = MovementDirection.Right;
    private GameController gameController;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        physic = GetComponent<Rigidbody2D>();
        gameController = FindObjectOfType<GameController>();
    }

    //TODO: Понять, нормально ли управлять прыжком из Update и можно ли это изменить
    private void Update()
    {
        JumpToCursorLogic();
    }

    //TODO: Игра ломается при множественном нажатии пробела

    private void FixedUpdate()
    {
        MovementLogic();
        CharacterReversal();

        // JumpLogic();
    }

    private void JumpLogic()
    {
        if (Input.GetAxis("Jump") > 0 && IsGrounded)
            physic.AddForce(Vector3.up * jumpBoost);
    }

    //TODO: исправить проблему ускорения при приземлении
    private void JumpToCursorLogic()
    {

        Debug.Log("jump");
        if (!IsGrounded)
            return;
        Debug.Log("ground");

        if (Input.GetKeyDown(KeyCode.Space) && !isTimeFrozen)
        {
            gameController.GameSpeed *= 0.01f;
            isTimeFrozen = true;
            defaultCursorPosition = Input.mousePosition;
        }

        if (Input.GetKeyUp(KeyCode.Space) && isTimeFrozen)
        {
            gameController.GameSpeed /= 0.01f;
            isTimeFrozen = false;

            // var jumpVector = VectorFromPlayer();
            var jumpVector = VectorFromBaseCursorPosition();


            var vectorAngle = Mathf.Atan2(jumpVector.y, jumpVector.x) * Mathf.Rad2Deg;

            if (vectorAngle is > 20 and < 160)
                physic.AddForce(jumpVector * jumpBoost);

            defaultCursorPosition = default;
        }
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
        // что бы скорость была стабильной в любом случае
        // и учитывая что мы вызываем из FixedUpdate мы умножаем на fixedDeltaTime

        transform.Translate(movement * (gameController.GameSpeed * speed * Time.fixedDeltaTime));
    }

    private void CharacterReversal()
    {
        if (!IsGrounded) return;

        var direction = Input.GetAxis("Horizontal");
        sprite.flipX = direction switch
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