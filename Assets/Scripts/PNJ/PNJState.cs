using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PNJState : State
{
    protected PNJManager manager;

    public PNJState(ObjectManager curObject) : base(curObject)
    {
        stateName = "PNJ_STATE";
        manager = (PNJManager)curObject;
    }

    #region STATE_METHODS

    public override void Enter()
    {
    }

    public override void Execute()
    {
    }

    public override void Exit()
    {
    }
    #endregion
}
