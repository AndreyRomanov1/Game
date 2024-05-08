using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class PlayerScript : MonoBehaviour, IDamageable
{
    private const float JumpBoost = 240f;
    private const float MaxJumpForce = 4;
    private const float MinJumpForce = 0.7f;
    private const float RiftBoost = 300f;
    private const float MaxRiftForce = 4;
    private const float MinRiftForce = 0.7f;
    private const float MaxHP = 200f;

    private float HP;
    private const float RiftDurationTime = 0.8f;

    private GameObject tools;
    private Rigidbody2D physic;
    private Animator animator;
    private Image HPBar;

    private Transform[] groundCheckers;
    private Transform leftWallCheck;
    private Transform rightWallCheck;
    public LayerMask groundMask;
    public GameObject currentGun;
    public GameObject dialoguesAnchor;

    private PlayerState playerState = PlayerState.Nothing;

    public PlayerState PlayerState
    {
        get => playerState;
        private set
        {
            PlayAnimation(playerState, value);
            playerState = value;
        }
    }

    private bool IsGrounded => groundCheckers
        .Any(groundCheck => Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundMask));

    private bool IsTouchedLeftWall =>
        Physics2D.OverlapCircle(leftWallCheck.position, 0.2f, groundMask);

    private bool IsTouchedRightWall =>
        Physics2D.OverlapCircle(rightWallCheck.position, 0.2f, groundMask);

    private TrajectoryRenderScript trajectory;
    private GameObject gunPosition;

    private void Start()
    {
        InitPlayerComponent();
        StartCoroutine(UpdateCoroutine());
        StartCoroutine(MovementStateController());
        
        SetDialogueCloud(Model.Clouds[0]);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            // Debug.Log("COLLISION");
        }
    }

    private void Print() =>
        Debug.Log(
            $"{PlayerState} {IsGrounded} {IsTouchedRightWall} {IsTouchedLeftWall} {Input.GetKeyDown(KeyCode.Space)} {Input.GetKeyUp(KeyCode.Space)} {Input.GetKey(KeyCode.Space)}");

    private IEnumerator UpdateCoroutine()
    {
        yield return new WaitForFixedUpdate();
        while (true)
        {
            MovementLogic();
            yield return null;
        }
    }

    private void SetGun(GameObject gun)
    {
        while (gunPosition.transform.childCount > 0)
        {
            Destroy(gunPosition.transform.GetChild(0));
        }

        Instantiate(gun, gunPosition.transform);
    }

    private void FlipPlayer()
    {
        var direction = transform.localEulerAngles.y == 0 ? Directions.Left : Directions.Right;
        FlipPlayerToDirection(direction);
    }

    private void FlipPlayerToDirection(Directions flipDirection)
    {
        // Debug.Log(flipDirection);
        var angle = 180 * (int)flipDirection;
        transform.localEulerAngles = new Vector3(0, angle, 0);
        tools.transform.localEulerAngles = new Vector3(0, angle, 0);
        // Debug.Log(playerPivot.transform.rotation.y);
    }

    private void PlayAnimation(PlayerState oldState, PlayerState newState)
    {
        switch (newState)
        {
            case PlayerState.Jump:
                switch (oldState)
                {
                    case PlayerState.CrouchedToJump:
                        animator.Play("jump");
                        break;
                    case PlayerState.CrouchedToJumpFromLeftWall:
                        animator.Play("jump from left wall");
                        break;
                    case PlayerState.CrouchedToJumpFromRightWall:
                        FlipPlayer();
                        animator.Play("jump from right wall");
                        break;
                    default:
                        Debug.Log("Прыжок не из состояний подготовки к прыжку");
                        break;
                }

                break;
            case PlayerState.Rift:
                if (oldState == PlayerState.CrouchedToJump)
                    animator.Play("rift");
                break;
            case PlayerState.CrouchedToJump:
                if (oldState == PlayerState.Nothing)
                    animator.Play("preparing for jump");
                break;
            case PlayerState.HangingOnLeftWall:
                if (oldState == PlayerState.Jump)
                    animator.Play("lending on left wall");
                break;
            case PlayerState.HangingOnRightWall:
                if (oldState == PlayerState.Jump)
                    animator.Play("landing on right wall");
                break;
            case PlayerState.Nothing:
                if (oldState == PlayerState.Jump)
                    animator.Play("lending on ground");
                break;
        }
    }

    private void InitPlayerComponent()
    {
        HP = MaxHP;
        
        physic = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        tools = transform.Find("Tools").gameObject;
        
        HPBar = tools.transform.Find("Main Camera").Find("StatesInspector").Find("HP bar").GetComponent<Image>();
        trajectory = GetComponentInChildren<TrajectoryRenderScript>();
        groundCheckers = GameObject.FindGameObjectsWithTag("GroundCheck")
            .Select(x => x.transform)
            .ToArray();
        leftWallCheck = GameObject.Find("LeftWallCheck").transform;
        rightWallCheck = GameObject.Find("RightWallCheck").transform;

        dialoguesAnchor = GameObject.Find("DialoguesAnchor");
        Debug.Log(dialoguesAnchor);

        gunPosition = transform.Find("bone_1").Find("bone_9")
            .Find("Pivot").Find("GG плечо").Find("bone_1").Find("GG локоть").Find("bone_1").Find("Gun position")
            .gameObject;

        //TODO:Вынести создание пушки из PlayerScript
        SetGun(currentGun);
    }

    // TODO: БАГ: иногда при прыжке в право игрок подпрыгивает на месте(только вверх).
    // TODO: похоже причина бага в неправильном занулении горизонтальной составляющей вектора скорости, персонаж может остановиться и в полете
    private void MovementLogic()
    {
        switch (PlayerState)
        {
            case PlayerState.Nothing:
            {
                if (Input.GetKeyDown(KeyCode.Space) && IsGrounded)
                {
                    animator.Play("preparing for jump");
                    PlayerState = PlayerState.CrouchedToJump;
                }

                break;
            }
            case PlayerState.CrouchedToJump:
            {
                var (state, vector) = GetMovementVector();

                if (Input.GetKey(KeyCode.Space))
                {
                    FlipPlayerToDirection(vector.x >= 0 ? Directions.Right : Directions.Left);
                    trajectory.ShowTrajectory(vector);
                }

                if (Input.GetKeyUp(KeyCode.Space))
                {
                    animator.Play("jump");
                    physic.AddForce(vector);
                    PlayerState = state;
                    trajectory.ClearTrajectory();
                }

                break;
            }
            case PlayerState.HangingOnRightWall or PlayerState.HangingOnLeftWall:
            {
                var velocity = physic.velocity;
                physic.velocity = new Vector2(velocity.x, velocity.y * 0.1f);

                if (Input.GetKeyDown(KeyCode.Space) && IsTouchedLeftWall)
                    PlayerState = PlayerState.CrouchedToJumpFromLeftWall;
                else if (Input.GetKeyDown(KeyCode.Space) && IsTouchedRightWall)
                    PlayerState = PlayerState.CrouchedToJumpFromRightWall;
                break;
            }
            case PlayerState.CrouchedToJumpFromLeftWall or PlayerState.CrouchedToJumpFromRightWall:
            {
                var velocity = physic.velocity;
                physic.velocity = new Vector2(velocity.x, velocity.y * 0.1f);

                var (state, vector) = GetWallMovementVector();
                if (Input.GetKey(KeyCode.Space))
                    trajectory.ShowTrajectory(vector);
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    physic.AddForce(vector);
                    PlayerState = state;
                    trajectory.ClearTrajectory();
                }

                break;
            }
        }
    }

    private (PlayerState state, Vector2 vector) GetWallMovementVector()
    {
        var vector = GetPositionDirectionVector();
        var vectorAngle = VectorAngle(vector);
        var state = PlayerState;
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
            if (PlayerState == PlayerState.Rift)
            {
                Print();

                yield return new WaitForSeconds(RiftDurationTime);
                if (!IsGrounded)
                    PlayerState = PlayerState.Jump;
                else
                {
                    physic.velocity = new Vector2(0, physic.velocity.y);
                    PlayerState = PlayerState.Nothing;
                }

                Print();
            }

            if (PlayerState is PlayerState.HangingOnLeftWall or PlayerState.HangingOnRightWall)
            {
                if (IsGrounded)
                    PlayerState = PlayerState.Nothing;
                else if (!IsTouchedLeftWall && !IsTouchedRightWall)
                    PlayerState = PlayerState.Jump;
            }

            if (PlayerState == PlayerState.Jump)
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
                    PlayerState = PlayerState.Nothing;
                else if (IsTouchedLeftWall)
                    PlayerState = PlayerState.HangingOnLeftWall;
                else if (IsTouchedRightWall)
                    PlayerState = PlayerState.HangingOnRightWall;
                Debug.Log("4 Конец прыжка");
                Print();
            }

            yield return null;
        }
    }

    private Vector2 GetPositionDirectionVector()
    {
        var cam = (Vector2)CurrentGame.PlayerCamera.ScreenToWorldPoint(Input.mousePosition);
        var position = (Vector2)transform.position;
        return cam - position;
    }

    private static float VectorAngle(Vector2 vector) => Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;

    public void SetDialogueCloud(GameObject cloud)
    {
        var c = Instantiate(cloud, dialoguesAnchor.transform);
        Debug.Log(c);
    }
    
    
    public void TakeDamage(float damage)
    {
        HP -= damage;
        if (HP <= 0)
            Die();
        HPBar.fillAmount = HP / MaxHP;
    }

    private void Die()
    {
        CurrentGame.KillGame();
        Model.StartGame();
    }
}