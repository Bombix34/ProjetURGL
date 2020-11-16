using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;

public class PedestalManager : NetworkBehaviour
{
    private readonly List<Pedestal> pedestals = new List<Pedestal>();
    public static PedestalManager Instance;
    public Action<ItemScriptableObject> onItemStolen;
    public Action<ItemScriptableObject> onItemRetrieve;

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
    private void RefreshPedestal(Pedestal pedestal)
    {
        if (pedestal.Stolen)
        {
            this.RpcOnItemStolen(pedestal.Item);
        }
        else
        {
            this.RpcOnItemRetrieve(pedestal.Item);
        }
    }

    [ClientRpc]
    private void RpcOnItemStolen(ItemScriptableObject item)
    {
        onItemStolen?.Invoke(item);
    }

    [ClientRpc]
    private void RpcOnItemRetrieve(ItemScriptableObject item)
    {
        onItemRetrieve?.Invoke(item);
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public Pedestal GetPedestalForItem(ItemScriptableObject item)
    {
        return this.pedestals.SingleOrDefault(p => p.Item == item);
    }

    public bool ExistPedestalForItem(ItemScriptableObject item)
    {
        return this.pedestals.Any(p => p.Item == item);
    }
}
