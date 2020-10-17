using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IntroCameraMovements : BaseCameraMovements
{
    enum IntroCameraMovementsState
    {
        WAITING,
        MOVING,
        FINISHED
    }

    private const double DISTANCE_TO_VALIDE_CHECKPOINTS = 1;
    private readonly Stack<Vector2> _targetsPositions;
    private readonly float _waitingDuration;
    private Vector2 currentTarget;
    private IntroCameraMovementsState currentIntroCameraMovementsState;
    private float waited = 0f;
    private readonly Action _onFinish;

    public IntroCameraMovements(Transform transform, float smoothness, float offsetZ, IEnumerable<Vector2> targetsPositions, float waitingDuration, Action onFinish) : base(transform, smoothness, offsetZ)
    {
        this._targetsPositions = new Stack<Vector2>(targetsPositions);
        this.NextTarget();
        _waitingDuration = waitingDuration;
        _onFinish = onFinish;
    }

    public override void Move()
    {
        switch (currentIntroCameraMovementsState)
        {
            case IntroCameraMovementsState.WAITING:
                this.Waiting();
                break;
            case IntroCameraMovementsState.MOVING:
                this.MoveToPosition(currentTarget);
                this.CheckPosition();
                break;
            case IntroCameraMovementsState.FINISHED:
                break;
            default:
                throw new NotImplementedException($"The state {currentIntroCameraMovementsState} is not implemented");
        }
    }

    private void CheckPosition()
    {
        if (Vector2.Distance(this.Transform.position, this.currentTarget) <= DISTANCE_TO_VALIDE_CHECKPOINTS)
        {
            this.currentIntroCameraMovementsState = IntroCameraMovementsState.WAITING;
        }
    }

    private void Waiting()
    {
        this.waited += Time.deltaTime;

        if(waited >= _waitingDuration)
        {
            this.waited = 0f;
            this.NextTarget();
        }
    }

    private void NextTarget()
    {
        Debug.Log("nextTARGET");
        if (this._targetsPositions.Count == 0)
        {
            this.currentIntroCameraMovementsState = IntroCameraMovementsState.FINISHED;
            this._onFinish();
            return;
        }
        this.currentTarget = _targetsPositions.Pop();
        this.currentIntroCameraMovementsState = IntroCameraMovementsState.MOVING;
    }

}
