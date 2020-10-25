using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PNJPositionDatas : NetworkBehaviour
{
    private List<Transform> targetPositionsDatas;

    private void Awake()
    {
        targetPositionsDatas = new List<Transform>();
        for(int i = 0; i < transform.childCount; ++i)
        {
            targetPositionsDatas.Add(transform.GetChild(i));
        }
    }

    public Vector2 GetPosition()
    {
        return targetPositionsDatas[Random.Range(0, targetPositionsDatas.Count)].position;
    }
}
