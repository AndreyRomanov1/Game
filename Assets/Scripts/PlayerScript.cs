using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerScript : MonoBehaviour, IDamageable, ISpeakingCharacter
{
    public float RiftDurationTime { get; set; }
    public GameObject Tools { get; private set; }
    public Rigidbody2D Physic { get; private set;  }
    public Transform MainTarget { get; private set; }
    public List<Transform> Targets { get; private set; }

    private Transform[] groundCheckers;
    private Transform leftWallCheck;
    private Transform rightWallCheck;
    public LayerMask groundMask;
    private GameObject currentGun;
    
    private GameObject dialoguesAnchor;

    private ISpeakingCharacter helper;

    private PlayerStates playerState = PlayerStates.Nothing;

    private MovementPlayer movement;
    private MovementStatePlayer movementState;
    private AnimationsPlayer animations;
    public LifePlayer Life;
    public SoundPlayer sound;

    public GameObject TimeFreeze;

    private GameObject triggerPrefab;

    private readonly Dictionary<ButtonsEnum, GameObject> buttonObjects = new()
    {
        { ButtonsEnum.F, null },
        { ButtonsEnum.Space, null }
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
        .Any(groundCheck => Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundMask))
    || CheckCollisionWithTag("Block") && IsGrounded2(1f);
    
    public bool IsGrounded2(float r) =>groundCheckers
        .Any(groundCheck => Physics2D.OverlapCircle(groundCheck.position, r, groundMask));

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
    public bool CheckCollisionWithTag(string tagToCheck)
    {
        var colliders = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0f);

        return colliders.Any(collider => collider.CompareTag(tagToCheck));
    }

    public void Print() =>
        Debug.Log(
            $"{PlayerState} {Model.GameState} {CheckCollisionWithTag("Block")} Time:{Time.timeScale} Касание:{IsGrounded} {IsTouchedRightWall} {IsTouchedLeftWall} Пробел: {Input.GetKeyDown(KeyCode.Space)} {Input.GetKeyUp(KeyCode.Space)} {Input.GetKey(KeyCode.Space)}");

    public void SetGun(GameObject gun)
    {
        // Debug.Log(currentGun.name);
        if (currentGun is not null)
        {
            currentGun.GetComponent<BaseGunScript>().SetMode(WeaponStateEnum.Nothing);
            Instantiate(triggerPrefab, CurrentGame.CurrentGameObject.transform).GetComponent<CollectionTriggerScript>()
                .CreateTrigger(currentGun);
        }
        while (gunPosition.transform.childCount > 0)
            Destroy(gunPosition.transform.GetChild(0));

        currentGun = gun;
        currentGun.transform.parent = gunPosition.transform;
        currentGun.transform.localPosition = Vector3.zero;
        currentGun.transform.localEulerAngles = Vector3.zero;
        currentGun.GetComponent<BaseGunScript>().SetMode(WeaponStateEnum.Player);
    }

    private void InitPlayerComponent()
    {
        Physic = GetComponent<Rigidbody2D>();
        Tools = transform.Find("Tools").gameObject;

        movement = new MovementPlayer(this);
        movementState = new MovementStatePlayer(this);
        animations = new AnimationsPlayer(this);
        Life = new LifePlayer(this);
        sound = GetComponent<SoundPlayer>();

        Instantiate(Resources.Load("Sound/Background Music"), transform);

        dialoguesAnchor = GameObject.Find("DialoguesAnchor");
        var helperAnchor = GameObject.Find("HelperAnchor");
        var helperGameObject = Resources.Load("Healper/Префаб/GreatCornEar");
        helper = Instantiate(helperGameObject, helperAnchor.transform).GetComponent<GreatCornEarScript>();
        CurrentGame.EnumToSpeaker = new Dictionary<SpeakersEnum, ISpeakingCharacter>
        {
            [SpeakersEnum.Player] = this,
            [SpeakersEnum.GreatCornEar] = helper,
            [SpeakersEnum.Comics] = GameObject.Find("ComicsAnchor").GetComponent<ComicsScript>()
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

        MainTarget = transform.Find("bone_1").Find("Target");
        Targets = new List<Transform> { MainTarget };
        Targets.AddRange(MainTarget.GetComponentsInChildren<Transform>());

        triggerPrefab = Resources.Load("Other Elements/CollectionTrigger").GameObject();

        TimeFreeze = GameObject.Find("TimeFreeze");
        TimeFreeze.SetActive(false);
        // currentGun = Instantiate(currentGun, gunPosition.transform);
        InitButtonDict();
        
        if (Model.IsEducation)
            ShowButtonIcon(ButtonsEnum.Space);
    }

    public void FlipPlayerToDirection(Directions flipDirection)
    {
        var angle = 180 * (int)flipDirection;
        transform.localEulerAngles = new Vector3(0, angle, 0);
        Tools.transform.localEulerAngles = new Vector3(0, angle, 0);
    }

    public void TakeDamage(float damage) => Life.TakeDamage(damage);
    public GameObject GetDialoguesAnchor() => dialoguesAnchor;

    public void ShowIfNeed()
    {
    }

    public void HideIfNeed()
    {
    }

    public void ShowButtonIcon(ButtonsEnum buttonEnum) => buttonObjects[buttonEnum].SetActive(true);

    public void HideButtonIcon(ButtonsEnum buttonEnum) => buttonObjects[buttonEnum].SetActive(false);

    private void InitButtonDict()
    {
        var buttonsFolder = transform.Find("Tools").Find("Buttons");
        foreach (var buttonName in buttonObjects.Keys.ToArray())
            buttonObjects[buttonName] = buttonsFolder.Find($"{Buttons.EnumToName[buttonName]}_button").gameObject;
    }
}