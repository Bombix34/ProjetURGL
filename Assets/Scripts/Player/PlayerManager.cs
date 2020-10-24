using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerManager : ObjectManager, IPlayerManager
{
    public bool IsVigil = false;
    [SerializeField]
    protected PlayerSettings settings;
    public CharacterRenderer Renderer { get; private set; }
    public Animator Animator { get; private set; }
    private Inventory inventory;

    private PlayerClickInput clickInteractionManager;

    protected void Start()
    {
        Renderer = GetComponentInChildren<CharacterRenderer>();
        if (!hasAuthority)
            return;
        this.inventory = GetComponent<Inventory>();
        Animator = GetComponent<Animator>();
        var cameraManager = Camera.main.GetComponent<CameraManager>();
        clickInteractionManager = GetComponentInChildren<PlayerClickInput>();

        cameraManager.Init(transform);
        cameraManager.StartIntro();
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

        //temporaire
        if (Input.GetKeyDown(KeyCode.A))
        {
            this.CmdDropItem();
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            clickInteractionManager.TryPerformInteraction();
        }
    }

    [Command]
    private void CmdDropItem()
    {
        //to change
        var inventory = GetComponent<Inventory>();
        var item = inventory.Items.First();
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
            inputVector *= Time.deltaTime;

            if (this.inventory.HasValuableItem)
            {
                inputVector *= this.settings.MovementSpeedWithValuableItemMultiplier;
            }

            return inputVector;
        }
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
