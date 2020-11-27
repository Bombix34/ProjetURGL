using Mirror;
using UnityEngine;
using UnityEngine.AI;

public class PNJManager : ObjectManager, ICaughtable
{
    private PlayerSettings Settings = null;
    public NavMeshAgent Agent { get; private set; }
    public PNJPositionDatas PositionDatas { get; private set; }
    public Animator Animator { get; private set; }
    private CharacterRenderer characterRenderer;

    private float previousPosX;

    public CharacterRenderer Renderer
    {
        get => characterRenderer;
    }

    public bool IsDead
    {
        get
        {
            if (currentState == null)
                return false;
            return currentState.stateName == "PNJ_DEAD";
        }
    }

    public override void OnStartClient()
    {
        characterRenderer = GetComponentInChildren<CharacterRenderer>();
        previousPosX = transform.position.x;
        Agent = GetComponent<NavMeshAgent>();
        Agent.updateRotation = false;
        Agent.updateUpAxis = false;
    }

    protected override void Update()
    {
        if (!isServer)
            return;

        UpdatePNJRotation();
        if (currentState == null)
        {
            ChangeState(new PNJMoveState(this, PositionDatas.GetPosition()));
        }
        currentState.Execute();
    }

    private void SetLocalData()
    {
        characterRenderer = GetComponentInChildren<CharacterRenderer>();
        previousPosX = transform.position.x;
        Agent = GetComponent<NavMeshAgent>();
        Agent.updateRotation = false;
        Agent.updateUpAxis = false;
    }

    [Server]
    public void Init()
    {
        this.SetLocalData();
        Settings = RoomSettings.Instance.Settings.VoleurSettings;
        Agent.speed = Settings.MovementSpeed * Settings.pnjSpeedMultiplicator;
        Animator = GetComponent<Animator>();
        PositionDatas = FindObjectOfType<PNJPositionDatas>();
        ChangeState(new PNJDeadState(this));
    }

    public override void ChangeState(State newState = null)
    {
        base.ChangeState(newState);
    }

    private void UpdatePNJRotation()
    {
        if (Agent.velocity.x==0 || Mathf.Abs(Agent.velocity.x) < 0.07f)
            return;
        characterRenderer.IsRendererFlip = Agent.velocity.x < 0;
    }

    [Server]
    public void GetCaught()
    {
        ChangeState(new PNJDeadState(this));
    }

    [Server]
    public void Enable()
    {
        Renderer.ActiveRenderer(true);
        gameObject.SetActive(true);
        this.RpcEnable();
    }

    [ClientRpc]
    private void RpcEnable()
    {
        Renderer.ActiveRenderer(true);
        gameObject.SetActive(true);
    }

    [Server]
    public void Disable()
    {
        Renderer.ActiveRenderer(false);
        gameObject.SetActive(false);
        this.RpcDisable();
    }

    [ClientRpc]
    private void RpcDisable()
    {
        Renderer.ActiveRenderer(false);
        gameObject.SetActive(false);
    }

}
