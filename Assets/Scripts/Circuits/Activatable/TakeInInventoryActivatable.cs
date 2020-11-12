using Mirror;
using System;
using UnityEngine;

public class TakeInInventoryActivatable : BaseActivatable, IItemContainer
{
    public override ActionTypes ActionType => ActionTypes.TAKE_ITEM;

    [SerializeField]
    [NotNull]
    private ItemScriptableObject item;
    [SerializeField]
    private ItemType itemType;
    [SyncVar(hook = nameof(ChangeItem))]
    private string itemName;

    public ItemScriptableObject Item
    {
        get => item; 
        set
        {
            item = value;
            this.itemName = this.item.ItemName;
        }
    }

    public override void OnStartServer()
    {
        this.item.InitialPosition = transform.position;
    }

    private void ChangeItem(string oldValue, string newValue)
    {
        string path;

        switch (this.itemType)
        {
            case ItemType.NORMAL_ITEM:
                path = $"Items/NormalItems/{newValue}";
                break;
            case ItemType.VALUABLE_ITEM:
                path = $"Items/ValuableItems/{newValue}";
                break;
            default:
                throw new NotImplementedException($"The item type {itemType} is not implemented");
        }

        this.item = Resources.Load<ItemScriptableObject>(path);

        if (item == null)
        {
            throw new ArgumentException($"No item found for path {path}");
        }

        this.SetSprite();
    }

    private void SetSprite()
    {
        this.GetComponent<SpriteRenderer>().sprite = item.Sprite;
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
