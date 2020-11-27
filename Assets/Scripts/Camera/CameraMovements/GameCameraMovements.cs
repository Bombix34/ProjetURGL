﻿using UnityEngine;

public class GameCameraMovements : BaseCameraMovements
{
    protected Transform playerTransform;

    public GameCameraMovements(Transform transform, float smoothness, float offsetZ, Transform playerTransform) : base(transform, smoothness, offsetZ)
    {
        this.ChangePlayerTransform(playerTransform);
    }

    protected void ChangePlayerTransform(Transform playerTransform)
    {
        this.playerTransform = playerTransform;
        this.Transform.position = playerTransform.position;
    }

    protected GameCameraMovements(Transform transform, float smoothness, float offsetZ) : base(transform, smoothness, offsetZ)  {}

    public override void Move()
    {
        if (!playerTransform)
        {
            return;
        }

        this.MoveToPosition(playerTransform.position);
    }
}
