using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerManager : ObjectManager, IPlayerManager
{
    [SyncVar]
    private string playerName;
    [SyncVar]
    private PlayerType playerType;
    [SerializeField]
    protected PlayerSettings settings;
    public static PlayerManager localPlayer;
    public CharacterRenderer Renderer { get; private set; }
    public Animator Animator { get; private set; }
    public NetworkAnimator NetworkAnimator { get; private set; }
    private Inventory inventory;

    private PlayerClickInput clickInteractionManager;
    private InteractionZone interactionTriggerZone;

    public Rigidbody2D Body { get; private set; }
    public string PlayerName { get => playerName; }
    public PlayerType PlayerType { get => playerType; }

    protected void Start()
    {
        Renderer = GetComponentInChildren<CharacterRenderer>();
        if (!hasAuthority)
            return;
        localPlayer = this;
        Body = GetComponent<Rigidbody2D>();
        this.inventory = GetComponent<Inventory>();
        Animator = GetComponent<Animator>();
        NetworkAnimator = GetComponent<NetworkAnimator>();
        var cameraManager = Camera.main.GetComponent<CameraManager>();
        clickInteractionManager = GetComponentInChildren<PlayerClickInput>();
        interactionTriggerZone = Camera.main.GetComponentInChildren<InteractionZone>();
        interactionTriggerZone.PlayerController = this.gameObject;
        Renderer.InitPlayerCharacterRenderer();
        cameraManager?.Init(transform);
        cameraManager?.StartIntro();
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
            this.CmdDropItem();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            clickInteractionManager.TryPerformInteraction();
        }
        //----------------------
    }

    public void Init(string playerName, PlayerType playerType)
    {
        this.playerName = playerName;
        this.playerType = playerType;
    }

    [Command]
    private void CmdDropItem()
    {
        var item = inventory.GetValuableItem();
        if(item is null)
        {
            return;
        }
        inventory.DropItem(item);
    }

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

            if (this.inventory.HasValuableItem)
            {
                inputVector *= this.settings.MovementSpeedWithValuableItemMultiplier;
            }
            return inputVector;
        }
    }

    /// <summary>
    /// change la taille du field of view pour correspondre aux settings
    /// : sur le field of view pour le rendu
    /// : sur area renderer pour le trigger
    /// </summary>
    private void ChangeFieldOfViewSize()
    {
        var cameraManager = Camera.main.GetComponent<CameraManager>();
        cameraManager.FieldOfViewManager.ViewSize = settings.FogOfWarSize;
        interactionTriggerZone.ColliderRadius = settings.FogOfWarSize*0.95f;
    }

    [Server]
    public void GetCaught()
    {
        this.RpcDisable();
    }

    [ClientRpc]
    private void RpcDisable()
    {
        this.gameObject.SetActive(false);
        if (isLocalPlayer)
        {
            CameraManager.Instance.NextPlayer();
        }
    }
}
