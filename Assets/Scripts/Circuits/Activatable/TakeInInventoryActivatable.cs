using Mirror;
using System;
using UnityEngine;

public class TakeInInventoryActivatable : BaseActivatable
{
    public override ActionTypes ActionType => ActionTypes.TAKE_ITEM;

    [SerializeField]
    [NotNull]
    private GameObject ItemGameObject;

    private NetworkItem networkItem;

    public override void OnStartServer()
    {
        networkItem = GetComponent<NetworkItem>();
        this.networkItem.Item.InitialPosition = transform.position;
    }

    public override void OnStartClient()
    {
        networkItem = GetComponent<NetworkItem>();
    }

    internal override void OnActivate(NetworkConnectionToClient sender)
    {
        var inventory = sender.identity.GetComponent<Inventory>();
        if (inventory.AddItem(networkItem.Item))
        {
            Destroy(this.gameObject);
        }
    }

    internal override void OnDeactivate(NetworkConnectionToClient sender)
    {
        throw new System.NotImplementedException();
    }
}
