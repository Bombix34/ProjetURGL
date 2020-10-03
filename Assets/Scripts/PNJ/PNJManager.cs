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
        if (!isServer)
            return;
        Agent = GetComponent<NavMeshAgent>();
        Agent.updateRotation = false;
        Agent.updateUpAxis = false;
        PositionDatas = FindObjectOfType<PNJPositionDatas>();
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
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
}
