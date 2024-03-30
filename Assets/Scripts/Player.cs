using DefaultNamespace;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 10f;
    public float jumpForce = 900f;

    public SpriteRenderer sprite;
    public Rigidbody2D physic;

    public Transform groundCheck;
    public LayerMask groundMask;

    public float groundRadius = 0.3f;
    public GameObject playerTilemap;
    private bool IsGrounded => Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundMask);
    // private bool IsMoveBlocked => Physics2D.

    private MovementDirection direction = MovementDirection.Right;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        physic = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        MovementLogic();
        JumpLogic();
        CharacterReversal();
    }

    private void MovementLogic()
    {
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
        transform.Translate(movement * (speed * Time.fixedDeltaTime));
    }

    private void JumpLogic()
    {
        if (Input.GetAxis("Jump") > 0 && IsGrounded)
            physic.AddForce(Vector3.up * jumpForce);

        // Обратите внимание что я делаю на основе Vector3.up 
        // а не на основе transform.up. Если персонаж упал или 
        // если персонаж -- шар, то его личный "верх" может 
        // любое направление. Влево, вправо, вниз...
        // Но нам нужен скачек только в абсолютный вверх, 
        // потому и Vector3.up
    }

    private void CharacterReversal()
    {
        var direction = Input.GetAxis("Horizontal");
        if (direction < 0)
            sprite.flipX = true;
        else if (direction > 0)
            sprite.flipX = false;
    }
}