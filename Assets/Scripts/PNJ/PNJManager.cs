using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PNJManager : ObjectManager
{
    public NavMeshAgent Agent { get; private set; }
    public PNJPositionDatas PositionDatas { get; private set; }

    private void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        Agent.updateRotation = false;
        Agent.updateUpAxis = false;
        if (!isServer)
            return;
        PositionDatas = FindObjectOfType<PNJPositionDatas>();
        ChangeState(new PNJMoveState(this, PositionDatas.GetPosition()));
    }

    protected override void Update()
    {
        if (!isServer)
            return;
        if (currentState == null)
            return;
        currentState.Execute();
    }

    public override void ChangeState(State newState = null)
    {
        base.ChangeState(newState);
    }
}
