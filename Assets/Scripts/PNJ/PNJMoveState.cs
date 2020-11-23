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
        target = new Vector3(targetPosition.x, targetPosition.y ,manager.transform.position.z);
    }

    #region STATE_METHODS

    public override void Enter()
    {
        manager.Agent.SetDestination(target);
        manager.Animator.SetTrigger("MOVE");
    }

    public override void Execute()
    {
        if (Vector3.Distance(manager.transform.position, target) < 0.25f)
        {
            manager.ChangeState(new PNJWaitState(manager));
        }
        else
        {
            if (manager.Agent.velocity == Vector3.zero)
                return;
            float magnitude = manager.Agent.desiredVelocity.magnitude;
            Vector2 newDirVector = SnapTo(manager.Agent.desiredVelocity.normalized);
           //Debug.Log(magnitude);
            manager.Agent.velocity = newDirVector*magnitude;
        }
    }

    public override void Exit()
    {
    }
    #endregion

    private Vector2 SnapTo(Vector2 v3)
    {
        float x = 0f;
        float y = 0;
        if(Mathf.Abs(v3.x) > Mathf.Abs(v3.y))
        {
            if(Mathf.Abs(v3.y) < 0.2f)
            {
                x = v3.x > 0 ? 1f : -1f;
                y = 0f;
            }
            else
            {
                x = v3.x > 0 ? 0.7f : -0.7f;
                y = v3.y > 0 ? 0.7f : -0.7f;
            }
        }
        else
        {
            if (Mathf.Abs(v3.x) < 0.2f)
            {
                x = 0f;
                y = v3.y > 0 ? 2f : -2f;
            }
            else
            {
                x = v3.x > 0 ? 0.7f : -0.7f;
                y = v3.y > 0 ? 0.7f : -0.7f;
            }
        }
        /*
        float snapX = Mathf.Round(v3.x*2) / 2;
        float snapY = Mathf.Round(v3.y*2) / 2;
        return new Vector3(snapX, snapY, 0f);
        */
        return new Vector2(x, y);
    }
}
