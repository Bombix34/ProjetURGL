using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState : State
{
    protected PlayerManager manager;

    public PlayerState(ObjectManager curObject) : base(curObject)
    {
        stateName = "PLAYER_IDLE";
        manager = (PlayerManager)curObject;
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
