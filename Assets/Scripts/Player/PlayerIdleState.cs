﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerState
{

    public PlayerIdleState(ObjectManager curObject) : base(curObject)
    {
        stateName = "PLAYER_IDLE";
    }

    #region STATE_METHODS

    public override void Enter()
    {
        manager.NetworkAnimator.SetTrigger("IDLE");
        manager.Body.velocity = Vector2.zero;
    }

    public override void Execute()
    {
        if(manager.MovementInput!=Vector2.zero)
        {
            manager.ChangeState(new PlayerMoveState(manager));
        }
    }

    public override void Exit()
    {
    }
    #endregion
}