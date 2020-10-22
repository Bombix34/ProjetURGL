using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorActivatable : BaseActivatable
{
    internal override void OnActivate()
    {
        Debug.Log("open");
    }

    internal override void OnDeactivate()
    {
        Debug.Log("close");
    }

    public override void RpcOnActivateClient()
    {
        Debug.Log("open");
    }

    public override void RpcOnDeactivateClient()
    {
        Debug.Log("close");
    }
}
