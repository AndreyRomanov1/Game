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

    private ISpeakingCharacter helper;

    private PlayerStates playerState = PlayerStates.Nothing;

    private MovementPlayer movement;
    private MovementStatePlayer movementState;
    private AnimationsPlayer animations;
    public LifePlayer Life;

    private GameObject triggerPrefab;

    private readonly Dictionary<Buttons, GameObject> buttonObjects = new()
    {
        { Buttons.F, null },
        { Buttons.Space, null }
    };

    public PlayerStates PlayerState
    {
        get => playerState;
        set
        {
            animations.PlayAnimation(playerState, value);
            TimeController.ChangePlayerState(playerState, value);
            playerState = value;
        }
    }

    public bool IsGrounded => groundCheckers
        .Any(groundCheck => Physics2D.OverlapCircle(groundCheck.position, 0.18f, groundMask));

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
        Dialogues.Reset();
        StartCoroutine(Dialogues.DialoguesCoroutine());
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

    public void SetGun(GameObject gun)
    {
        // Debug.Log(currentGun.name);
        currentGun.GetComponent<BaseWeaponScript>().SetMode(WeaponStateEnum.Nothing);
        Instantiate(triggerPrefab, transform).GetComponent<CollectionTriggerScript>().CreateTrigger(currentGun);
        while (gunPosition.transform.childCount > 0)
            Destroy(gunPosition.transform.GetChild(0));

        currentGun = gun;
        currentGun.transform.parent = gunPosition.transform;
        currentGun.transform.localPosition = Vector3.zero;
        currentGun.transform.localEulerAngles = Vector3.zero;
        currentGun.GetComponent<BaseWeaponScript>().SetMode(WeaponStateEnum.Player);
    }

    private void InitPlayerComponent()
    {
        physic = GetComponent<Rigidbody2D>();
        tools = transform.Find("Tools").gameObject;

        movement = new MovementPlayer(this);
        movementState = new MovementStatePlayer(this);
        animations = new AnimationsPlayer(this);
        Life = new LifePlayer(this);

        Instantiate(Resources.Load("Sound/Background Music"), transform);

        dialoguesAnchor = GameObject.Find("DialoguesAnchor");
        var helperAnchor = GameObject.Find("HelperAnchor");
        var helperGameObject = Resources.Load("Healper/Префаб/GreatCornEar");
        helper = Instantiate(helperGameObject, helperAnchor.transform).GetComponent<GreatCornEarScript>();
        CurrentGame.EnumToSpeaker = new Dictionary<SpeakersEnum, ISpeakingCharacter>
        {
            [SpeakersEnum.Player] = this,
            [SpeakersEnum.GreatCornEar] = helper
        };


        groundCheckers = GameObject.FindGameObjectsWithTag("GroundCheck")
            .Select(x => x.transform)
            .ToArray();
        leftWallCheck = GameObject.Find("LeftWallCheck").transform;
        rightWallCheck = GameObject.Find("RightWallCheck").transform;

        gunPosition = transform
            .Find("bone_1").Find("bone_9")
            .Find("Pivot")
            .Find("GG плечо").Find("bone_1").Find("GG локоть").Find("bone_1")
            .Find("Gun position")
            .gameObject;

        mainTarget = transform.Find("bone_1").Find("Target");
        targets = new List<Transform> { mainTarget };
        targets.AddRange(mainTarget.GetComponentsInChildren<Transform>());

        triggerPrefab = Resources.Load("Other Elements/CollectionTrigger").GameObject();

        currentGun = Instantiate(currentGun, gunPosition.transform);
        InitButtonDict();
    }

    public void FlipPlayerToDirection(Directions flipDirection)
    {
        var angle = 180 * (int)flipDirection;
        transform.localEulerAngles = new Vector3(0, angle, 0);
        tools.transform.localEulerAngles = new Vector3(0, angle, 0);
    }

    public void TakeDamage(float damage) => Life.TakeDamage(damage);
    public GameObject GetDialoguesAnchor() => dialoguesAnchor;

    public void ShowIfNeed()
    {
    }

    public void HideIfNeed()
    {
    }

    public void ShowButtonIcon(Buttons button) => buttonObjects[button].SetActive(true);

    public void HideButtonIcon(Buttons button) => buttonObjects[button].SetActive(false);

    private void InitButtonDict()
    {
        var buttonsFolder = transform.Find("Tools").Find("Buttons");
        foreach (var buttonName in buttonObjects.Keys.ToArray())
            buttonObjects[buttonName] = buttonsFolder.Find($"{ButtonsEnum.EnumToName[buttonName]}_button").gameObject;
    }
}