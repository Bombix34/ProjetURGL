using Mirror;
using System;
using UnityEngine;

public class TakeFromInventoryActivatable : BaseActivatable
{
    public override ActionTypes ActionType => ActionTypes.STEAL_ITEM;

    [SerializeField]
    private string itemName;

    private Inventory inventory;

    public override void OnStartClient()
    {
        inventory = GetComponent<Inventory>();
    }

    internal override void OnActivate(NetworkConnectionToClient sender)
    {
        var item = inventory.GetItemByName(itemName);

        if(item == null)
        {
            return;
        }

        var senderInventory = sender.identity.GetComponent<Inventory>();
        if (senderInventory.AddItem(item))
        {
            inventory.RemoveItem(item);
        }
    }

    internal override void OnDeactivate(NetworkConnectionToClient sender)
    {
        throw new NotImplementedException();
    }
}
