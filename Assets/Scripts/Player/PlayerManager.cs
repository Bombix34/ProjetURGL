using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerManager : ObjectManager
{
    public bool IsVigil = false;
    [SerializeField]
    protected PlayerSettings settings;
    public CharacterRenderer Renderer{ get; private set; }
    public Animator Animator { get; private set; }

    protected void Start()
    {
        Renderer = GetComponentInChildren<CharacterRenderer>();
        if (!hasAuthority)
            return;
        Animator = GetComponent<Animator>();
        Camera.main.GetComponent<CameraManager>().StartCameraFollow(this.transform);
        ChangeState(new PlayerIdleState(this));
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
    }

    [Command]
    private void CmdDropItem()
    {
        var inventory = GetComponent<Inventory>();
        var item = inventory.Items.First();
        print(inventory.Items.Count);
        inventory.DropItem(item);
    }

    public void Init()
    {
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

            float inputX = Input.GetAxis("Horizontal");
            float inputY = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(settings.MovementSpeed * inputX, settings.MovementSpeed * inputY, 0);
            if (inputX < 0)
            {
                Renderer.IsRendererFlip = true;
            }
            else if (inputX != 0)
            {
                Renderer.IsRendererFlip = false;
            }
            movement *= Time.deltaTime;
            return movement;
        }
    }
}
