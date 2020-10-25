using Mirror;
using UnityEngine;

public class TakeInInventoryActivatable : BaseActivatable
{
    [SerializeField]
    private ItemScriptableObject item;

    public ItemScriptableObject Item { get => item; set => item = value; }

    private void Start()
    {
        this.type = ActionTypes.TAKE_ITEM;
    }
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
