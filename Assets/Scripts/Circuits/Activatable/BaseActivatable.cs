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
        this.RpcOnActivateClient();
    }

    [Command(ignoreAuthority = true)]
    public virtual void CmdDeactivate()
    {
        this.OnDeactivate();
        this.RpcOnDeactivateClient();
    }

    public abstract void OnActivate();

    [ClientRpc]
    public virtual void RpcOnActivateClient() { return; }
    public abstract void OnDeactivate();
    [ClientRpc]
    public virtual void RpcOnDeactivateClient() { return; }
}
