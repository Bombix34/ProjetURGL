using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class PlayerManager : ObjectManager
{
    private const float FOG_OF_WAR_SIZE_COLLIDER_MULTIPLIER = 0.95f;
    [SerializeField]
    [NotNull]
    protected PlayerSettings settings;

    [SyncVar]
    private string playerName;

    public static PlayerManager localPlayer;
    public CharacterRenderer Renderer { get; private set; }
    public Animator Animator { get; private set; }
    public NetworkAnimator NetworkAnimator { get; private set; }
    protected Inventory inventory;

    private PlayerClickInput clickInteractionManager;
    private InteractionZone interactionTriggerZone;
    private CameraManager cameraManager;

    public Rigidbody2D Body { get; private set; }
    public string PlayerName { get => playerName; }
    public PlayerType PlayerType { get => settings.PlayerType; }

    public Vector2 MovementInput
    {
        get
        {
            if (!hasAuthority)
            {
                // exit from update if this is not the local player
                return Vector2.zero;
            }
            Vector2 inputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
            inputVector *= settings.MovementSpeed;
            if (inputVector.x < 0)
            {
                Renderer.IsRendererFlip = true;
            }
            else if (inputVector.x != 0)
            {
                Renderer.IsRendererFlip = false;
            }
            inputVector *= Time.fixedDeltaTime;

            //TO DO REMOVE?
            //if (this.inventory.HasValuableItem)
            //{
            //    inputVector *= this.settings.MovementSpeedWithValuableItemMultiplier;
            //}
            return inputVector;
        }
    }

    public void Init(string playerName)
    {
        this.playerName = playerName;
    }

    protected abstract void OnStart();

    protected void Start()
    {
        Renderer = GetComponentInChildren<CharacterRenderer>();
        inventory = GetComponent<Inventory>();
        this.OnStart();
        if (!hasAuthority)
            return;
        localPlayer = this;
        Body = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        NetworkAnimator = GetComponent<NetworkAnimator>();
        cameraManager = Camera.main.GetComponent<CameraManager>();
        clickInteractionManager = GetComponentInChildren<PlayerClickInput>();
        interactionTriggerZone = Camera.main.GetComponentInChildren<InteractionZone>();
        interactionTriggerZone.PlayerController = this.gameObject;
        Renderer.InitPlayerCharacterRenderer();
        cameraManager.Init(transform);
        cameraManager.StartIntro();
        ChangeFieldOfViewSize();
        ChangeState(new PlayerIdleState(this));
        RoomPlayerVivox.Instance.StartGame(settings.PlayerType);
    }

    protected override void Update()
    {
        if (!hasAuthority)
            return;
        if (currentState == null)
            return;
        currentState.Execute();

        //temporaire------------
        ChangeFieldOfViewSize();
        if (Input.GetKeyDown(KeyCode.A))
        {
            this.OnPressAlternateActionButton();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            clickInteractionManager.TryPerformInteraction();
        }
        //----------------------
    }

    /// <summary>
    /// change la taille du field of view pour correspondre aux settings
    /// : sur le field of view pour le rendu
    /// : sur area renderer pour le trigger
    /// </summary>
    private void ChangeFieldOfViewSize()
    {
        cameraManager.FieldOfViewManager.ViewSize = settings.FogOfWarSize;
        interactionTriggerZone.ColliderRadius = settings.FogOfWarSize * FOG_OF_WAR_SIZE_COLLIDER_MULTIPLIER;
    }

    protected virtual void OnPressAlternateActionButton() { }
}
