using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseActivatable : NetworkBehaviour
{
    [Command(ignoreAuthority = true)]
    public virtual void CmdActivate()
    {
        this.OnActivate();
        this.RpcActivateClient();
    }

    [Command(ignoreAuthority = true)]
    public virtual void CmdDeactivate()
    {
        this.OnDeactivate();
        this.RpcDeactivateClient();
    }

    internal abstract void OnActivate();
    internal abstract void OnDeactivate();

    [ClientRpc]
    public virtual void RpcActivateClient() { return; }
    [ClientRpc]
    public virtual void RpcDeactivateClient() { return; }
}
