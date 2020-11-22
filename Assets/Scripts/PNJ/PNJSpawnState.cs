using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PNJSpawnState : PNJState
{
    private Vector2 spawnPosition;

    public PNJSpawnState(ObjectManager curObject) : base(curObject)
    {
        stateName = "PNJ_SPAWN";
        manager = (PNJManager)curObject;
    }
    public PNJSpawnState(ObjectManager curObject, Vector2 position) : base(curObject)
    {
        stateName = "PNJ_SPAWN";
        manager = (PNJManager)curObject;
        spawnPosition = position;
    }
    #region STATE_METHODS

    public override void Enter()
    {
        manager.transform.position = spawnPosition;
        manager.Renderer.ActiveRenderer(true);
        manager.gameObject.SetActive(true);
        manager.Animator.SetTrigger("IDLE");
        manager.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        manager.Agent.ResetPath();
        manager.ChangeState(new PNJWaitState(manager));
    }

    public override void Execute()
    {
    }

    public override void Exit()
    {
    }
    #endregion
}