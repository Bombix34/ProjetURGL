using Mirror;
using UnityEngine;

public class TakeInInventoryActivatable : BaseActivatable, IItemContainer
{
    public override ActionTypes ActionType => ActionTypes.TAKE_ITEM;

    [SerializeField]
    [NotNull]
    private ItemScriptableObject item;

    public ItemScriptableObject Item { get => item; set => item = value; }

    internal override void OnActivate(NetworkConnectionToClient sender)
    {
        var inventory = sender.identity.GetComponent<Inventory>();
        if (inventory.AddItem(Item))
        {
            Destroy(this.gameObject);
        }
    }

    internal override void OnDeactivate(NetworkConnectionToClient sender)
    {
        throw new System.NotImplementedException();
    }
}
