using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PNJManager : ObjectManager
{
    public NavMeshAgent Agent { get; private set; }
    public PNJPositionDatas PositionDatas { get; private set; }
    public Animator Animator { get; private set; }
    [SerializeField]
    private GameObject rendererContainer;

    private void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        Agent.updateRotation = false;
        Agent.updateUpAxis = false;
        if (!isServer)
            return;
        Animator = GetComponent<Animator>();
        PositionDatas = FindObjectOfType<PNJPositionDatas>();
        ChangeState(new PNJMoveState(this, PositionDatas.GetPosition()));
    }

    protected override void Update()
    {
        if (!isServer)
            return;
        UpdatePNJRotation();
        if (currentState == null)
            return;
        currentState.Execute();
    }

    public override void ChangeState(State newState = null)
    {
        base.ChangeState(newState);
    }

    private void UpdatePNJRotation()
    {
        if (Agent.destination.x < transform.position.x)
            rendererContainer.transform.localScale = new Vector3(-1f, 1f, 1f);
        else if(Agent.destination.x > transform.position.x)
            rendererContainer.transform.localScale = new Vector3(1f, 1f, 1f);
    }
}
