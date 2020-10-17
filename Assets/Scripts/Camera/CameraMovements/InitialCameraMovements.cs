using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialCameraMovements : BaseCameraMovements
{
    private readonly Vector2 _initialPosition;

    public InitialCameraMovements(Transform transform, float smoothness, float offsetZ, Vector2 initialPosition) : base(transform, smoothness, offsetZ)
    {
        this._initialPosition = initialPosition;
    }
    public override void Move()
    {
        this.MoveToPosition(_initialPosition);
    }

    protected override void MoveToPosition(Vector2 vector2)
    {
        this.Transform.position = vector2.ToVector3(OffsetZ);
    }
}
