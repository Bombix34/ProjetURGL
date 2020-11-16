using Mirror;
using System;
using UnityEngine;

public class TakeInInventoryActivatable : BaseActivatable
{
    public override ActionTypes ActionType => ActionTypes.TAKE_ITEM;

    private NetworkItem networkItem;

    public override void OnStartClient()
    {
        networkItem = GetComponent<NetworkItem>();
    }

    internal override void OnActivate(NetworkConnectionToClient sender)
    {
        var inventory = sender.identity.GetComponent<Inventory>();
        if (inventory.AddItem(networkItem.Item))
        {
            PedestalManager.Instance.GetPedestalForItem(networkItem.Item).OnItemDestroy();
            Destroy(this.gameObject);
        }
    }

    internal override void OnDeactivate(NetworkConnectionToClient sender)
    {
        throw new NotImplementedException();
    }
}
