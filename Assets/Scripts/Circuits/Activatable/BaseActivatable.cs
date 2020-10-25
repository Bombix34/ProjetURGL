using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseActivatable : NetworkBehaviour
{
    protected ActionTypes type = ActionTypes.NOT_SET;

    public ActionTypes ActionType { get => type; }

    [Command(ignoreAuthority = true)]
    public virtual void CmdActivate(NetworkConnectionToClient sender = null)
    {
        this.OnActivate(sender);
        this.RpcActivateClient();
    }

    [Command(ignoreAuthority = true)]
    public virtual void CmdDeactivate(NetworkConnectionToClient sender = null)
    {
        this.OnDeactivate(sender);
        this.RpcDeactivateClient();
    }

    internal abstract void OnActivate(NetworkConnectionToClient sender);
    internal abstract void OnDeactivate(NetworkConnectionToClient sender);

    [ClientRpc]
    public virtual void RpcActivateClient() { return; }
    [ClientRpc]
    public virtual void RpcDeactivateClient() { return; }
}
