using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PNJDeadState : PNJState
{

    public PNJDeadState(ObjectManager curObject) : base(curObject)
    {
        stateName = "PNJ_DEAD";
        manager = (PNJManager)curObject;
    }

    #region STATE_METHODS

    public override void Enter()
    {
        manager.Renderer.ActiveRenderer(false);
        manager.gameObject.SetActive(false);
        manager.Animator.SetTrigger("IDLE");
        manager.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    public override void Execute()
    {
    }

    public override void Exit()
    {
    }
    #endregion
}