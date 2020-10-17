using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCameraMovements
{
    protected Transform Transform { get; }
    protected float Smoothness { get; }
    protected float OffsetZ { get; }
    public BaseCameraMovements(Transform transform, float smoothness, float offsetZ)
    {
        this.Transform = transform;
        this.Smoothness = smoothness;
        this.OffsetZ = offsetZ;
    }

    public abstract void Move();

    protected virtual void MoveToPosition(Vector2 vector2)
    {
        this.Transform.position = Vector2.Lerp(Transform.position, vector2, Smoothness).ToVector3(OffsetZ);
    }
}
