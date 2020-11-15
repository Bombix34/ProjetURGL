using Mirror;
using System;
using System.Collections.Generic;

public class PedestalManager : NetworkBehaviour
{
    private readonly List<Pedestal> pedestals = new List<Pedestal>();
    public static PedestalManager Instance;
    public Action onItemStolen;
    public Action onItemRetrieve;

    public override void OnStartServer()
    {
        Instance = this;
    }

    [Server]
    public void Register(Pedestal pedestal)
    {
        this.pedestals.Add(pedestal);
        pedestal.StolenStateChanged += this.RefreshPedestal;
    }

    [Server]
    private void RefreshPedestal(bool pedestalStolen)
    {
        if (pedestalStolen)
        {
            this.RpcOnItemStolen();
        }
        else
        {
            this.RpcOnItemRetrieve();
        }
    }

    [ClientRpc]
    private void RpcOnItemStolen()
    {
        onItemStolen?.Invoke();
    }

    [ClientRpc]
    private void RpcOnItemRetrieve()
    {
        onItemRetrieve?.Invoke();
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
