using UnityEngine;

public class PNJWaitState : PNJState
{
    private float waitTime;

    public PNJWaitState(ObjectManager curObject) : base(curObject)
    {
        stateName = "PNJ_WAIT";
        manager = (PNJManager)curObject;
    }

    #region STATE_METHODS

    public override void Enter()
    {
        manager.Animator.SetTrigger("IDLE");
        manager.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        waitTime = Random.Range(2f, 6f);
        manager.Agent.velocity = Vector3.zero;
        manager.Agent.ResetPath();
    }

    public override void Execute()
    {
        waitTime -= Time.deltaTime;
        if (waitTime <= 0f)
            manager.ChangeState(new PNJMoveState(manager, manager.PositionDatas.GetPosition()));
    }

    public override void Exit()
    {
    }
    #endregion
}

