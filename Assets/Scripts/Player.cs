using System;
using DefaultNamespace;
using DefaultNamespace.Interfaces;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour, IDamageble
{
    public float speed = 10f;
    public float jumpForce = 900f;

    //TODO: Понять почему gameSpeed работает только при static и можно ли это изменить

    public SpriteRenderer sprite;
    public Rigidbody2D physic;

    public Transform groundCheck1;
    public Transform groundCheck2;
    public LayerMask groundMask;

    public float groundRadius = 0.3f;
    public GameObject playerTilemap;

    private bool isTimeFrozen = false;

    private bool IsGrounded => Physics2D.OverlapCircle(groundCheck1.position, groundRadius, groundMask) ||
                               Physics2D.OverlapCircle(groundCheck2.position, groundRadius, groundMask);
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

    private void FixedUpdate()
    {
        // JumpToCursorLogic();
        MovementLogic();
        CharacterReversal();
    }

    //TODO: исправить проблему ускорения при приземлении
    private void JumpToCursorLogic()
    {
        if (!IsGrounded)
            return;

        if (Input.GetKeyDown(KeyCode.Space) && !isTimeFrozen)
        {
            gameController.GameSpeed *= 0.01f;
            isTimeFrozen = true;
        }

        if (Input.GetKeyUp(KeyCode.Space) && isTimeFrozen)
        {
            gameController.GameSpeed /= 0.01f;
            isTimeFrozen = false;

            var cursorPosition = Camera.main.WorldToScreenPoint(transform.position);
            var jumpVector = (Input.mousePosition - cursorPosition).normalized;
            var vectorAngle = Mathf.Atan2(jumpVector.y, jumpVector.x) * Mathf.Rad2Deg;

            if (vectorAngle is > 20 and < 160)
                physic.AddForce(jumpVector * jumpForce);
        }
    }

    private void MovementLogic()
    {
        if (!IsGrounded) return;

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