using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerState
{

    public PlayerMoveState(ObjectManager curObject) : base(curObject)
    {
        stateName = "PLAYER_MOVE";
    }

    #region STATE_METHODS

    public override void Enter()
    {
        manager.NetworkAnimator.SetTrigger("MOVE");
    }

    public override void Execute()
    {
        if (manager.MovementInput != Vector2.zero)
        {
           // manager.transform.Translate(manager.MovementInput);
           manager.Body.velocity = manager.MovementInput;
        }
        else
        {
            manager.ChangeState(new PlayerIdleState(manager));
        }
    }

    public override void Exit()
    {
    }
    #endregion
}
