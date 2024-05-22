using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerScript : MonoBehaviour, IDamageable, ISpeakingCharacter
{
    public float riftDurationTime;

    public GameObject tools;
    public Rigidbody2D physic;

    private Transform[] groundCheckers;
    private Transform leftWallCheck;
    private Transform rightWallCheck;
    public LayerMask groundMask;
    public GameObject currentGun;
    public Transform mainTarget;
    public List<Transform> targets;
    private GameObject dialoguesAnchor;

    public ISpeakingCharacter Helper;

    private PlayerStates playerState = PlayerStates.Nothing;

    private MovementPlayer movement;
    private MovementStatePlayer movementState;
    private AnimationsPlayer animations;
    private LifePlayer life;

    public PlayerStates PlayerState
    {
        get => playerState;
        set
        {
            animations.PlayAnimation(playerState, value);
            playerState = value;
        }
    }

    public bool IsGrounded => groundCheckers
        .Any(groundCheck => Physics2D.OverlapCircle(groundCheck.position, 0.15f, groundMask));

    public bool IsTouchedLeftWall =>
        Physics2D.OverlapCircle(leftWallCheck.position, 0.2f, groundMask);

    public bool IsTouchedRightWall =>
        Physics2D.OverlapCircle(rightWallCheck.position, 0.2f, groundMask);

    private GameObject gunPosition;

    private void Start()
    {
        InitPlayerComponent();
        StartCoroutine(movement.MovementCoroutine());
        StartCoroutine(movementState.MovementStateCoroutine());
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            // Debug.Log("COLLISION");
        }
    }

    public void Print() =>
        Debug.Log(
            $"{PlayerState} {IsGrounded} {IsTouchedRightWall} {IsTouchedLeftWall} {Input.GetKeyDown(KeyCode.Space)} {Input.GetKeyUp(KeyCode.Space)} {Input.GetKey(KeyCode.Space)}");

    private void SetGun(GameObject gun)
    {
        while (gunPosition.transform.childCount > 0)
            Destroy(gunPosition.transform.GetChild(0));

        Instantiate(gun, gunPosition.transform);
    }

    private void InitPlayerComponent()
    {
        physic = GetComponent<Rigidbody2D>();
        tools = transform.Find("Tools").gameObject;

        movement = new MovementPlayer(this);
        movementState = new MovementStatePlayer(this);
        animations = new AnimationsPlayer(this);
        life = new LifePlayer(this);

        dialoguesAnchor = GameObject.Find("DialoguesAnchor");
        var helperAnchor = GameObject.Find("HelperAnchor");
        var helperGameObject = Resources.Load("Healper/Префаб/GreatCornEar");
        Helper = Instantiate(helperGameObject, helperAnchor.transform).GetComponent<GreatCornEarScript>();
        CurrentGame.EnumToSpeaker = new Dictionary<SpeakersEnum, ISpeakingCharacter>
        {
            [SpeakersEnum.Player] = this,
            [SpeakersEnum.GreatCornEar] = Helper
        };
        Model.Game.StartCoroutine(Dialogues.DialoguesCoroutine());

        groundCheckers = GameObject.FindGameObjectsWithTag("GroundCheck")
            .Select(x => x.transform)
            .ToArray();
        leftWallCheck = GameObject.Find("LeftWallCheck").transform;
        rightWallCheck = GameObject.Find("RightWallCheck").transform;

        gunPosition = transform.Find("bone_1").Find("bone_9")
            .Find("Pivot").Find("GG плечо").Find("bone_1").Find("GG локоть").Find("bone_1").Find("Gun position")
            .gameObject;

        mainTarget = transform.Find("bone_1").Find("Target");
        targets = new List<Transform> { mainTarget };
        targets.AddRange(mainTarget.GetComponentsInChildren<Transform>());

        SetGun(currentGun); //TODO:Вынести создание пушки из PlayerScript
    }

    public void FlipPlayerToDirection(Directions flipDirection)
    {
        var angle = 180 * (int)flipDirection;
        transform.localEulerAngles = new Vector3(0, angle, 0);
        tools.transform.localEulerAngles = new Vector3(0, angle, 0);
    }

    public void TakeDamage(float damage) => life.TakeDamage(damage);
    public GameObject GetDialoguesAnchor() => dialoguesAnchor;

    public void ShowIfNeed()
    {
    }

    public void HideIfNeed()
    {
    }
}