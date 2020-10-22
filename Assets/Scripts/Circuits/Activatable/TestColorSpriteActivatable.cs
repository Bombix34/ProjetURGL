using Mirror;
using UnityEngine;

public class TestColorSpriteActivatable : BaseActivatable
{
    internal override void OnActivate()
    {
        GetComponent<SpriteRenderer>().color = Color.green;
    }

    internal override void OnDeactivate()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
    }

    [ClientRpc]
    public override void RpcOnActivateClient()
    {
        GetComponent<SpriteRenderer>().color = Color.green;
    }

    [ClientRpc]
    public override void RpcOnDeactivateClient()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
    }
}
