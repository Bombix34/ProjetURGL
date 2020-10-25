using Mirror;
using UnityEngine;

public class TestColorSpriteActivatable : BaseActivatable
{
    internal override void OnActivate(NetworkConnectionToClient sender)
    {
        GetComponent<SpriteRenderer>().color = Color.green;
    }

    internal override void OnDeactivate(NetworkConnectionToClient sender)
    {
        GetComponent<SpriteRenderer>().color = Color.red;
    }

    [ClientRpc]
    public override void RpcActivateClient()
    {
        GetComponent<SpriteRenderer>().color = Color.green;
    }

    [ClientRpc]
    public override void RpcDeactivateClient()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
    }
}
