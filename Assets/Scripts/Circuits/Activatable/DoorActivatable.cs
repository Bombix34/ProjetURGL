using Mirror;
using UnityEngine;

public class DoorActivatable : BaseActivatable
{
    internal override void OnActivate(NetworkConnectionToClient sender)
    {
        Debug.Log("open");
    }

    internal override void OnDeactivate(NetworkConnectionToClient sender)
    {
        Debug.Log("close");
    }

    public override void RpcActivateClient()
    {
        Debug.Log("open");
    }

    public override void RpcDeactivateClient()
    {
        Debug.Log("close");
    }
}
