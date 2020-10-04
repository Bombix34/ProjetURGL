using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PNJMoveState : PNJState
{
    private Vector2 target;

    public PNJMoveState(ObjectManager curObject) : base(curObject)
    {
        stateName = "PNJ_MOVE";
    }

    public PNJMoveState(ObjectManager curObject, Vector2 targetPosition) : base(curObject)
    {
        stateName = "PNJ_MOVE";
        manager = (PNJManager)curObject;
        target = new Vector3(targetPosition.x + Random.Range(-1f,1f),targetPosition.y+Random.Range(-1f,1f),manager.transform.position.z);
    }

    #region STATE_METHODS

    public override void Enter()
    {
        manager.Agent.SetDestination(target);
    }

    public override void Execute()
    {
        if (Vector3.Distance(manager.transform.position, target) < 0.3f)
        {
            manager.ChangeState(new PNJWaitState(manager));
        }
    }

    public override void Exit()
    {
    }
    #endregion
}
