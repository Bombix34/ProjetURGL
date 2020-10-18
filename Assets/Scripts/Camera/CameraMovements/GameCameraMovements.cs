using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCameraMovements : BaseCameraMovements
{
    private readonly Transform playerTransform;

    public GameCameraMovements(Transform transform, float smoothness, float offsetZ, Transform playerTransform) : base(transform, smoothness, offsetZ)
    {
        this.playerTransform = playerTransform;
    }

    public override void Move()
    {
        if (playerTransform)
        {
            this.MoveToPosition(playerTransform.position);
        }
    }
}
