using Mirror;
using System;
using System.IO;
using UnityEngine;

public class NetworkItem : NetworkBehaviour, IItemContainer
{
    private const string RESOURCE_NORMAL_ITEM_PATH = "Items/NormalItems/";
    private const string RESOURCE_VALUABLE_ITEM_PATH = "Items/ValuableItems/";
    [SerializeField]
    [NotNull]
    protected ItemScriptableObject item;

    [SyncVar(hook = nameof(ChangeItem))]
    private string itemName;
    [SerializeField]
    [NotNull]
    private SpriteRenderer spriteRenderer = null;
    public ItemScriptableObject Item
    {
        get => item;
        set
        {
            item = value;
            this.itemName = this.item.ItemName;
        }
    }
    public virtual ItemType ItemType => ItemType.NORMAL_ITEM;

    public override void OnStartServer()
    {
        item.Pedestal?.SetNetworkItem(this);
    }

    private void ChangeItem(string oldValue, string newValue)
    {
        LoadResource(newValue);

        this.SetSprite();
    }

    private void LoadResource(string newValue)
    {
        string path;

        switch (this.ItemType)
        {
            case ItemType.NORMAL_ITEM:
                path = Path.Combine(RESOURCE_NORMAL_ITEM_PATH, newValue);
                break;
            case ItemType.VALUABLE_ITEM:
                path = Path.Combine(RESOURCE_VALUABLE_ITEM_PATH, newValue);
                break;
            default:
                throw new NotImplementedException($"The item type {ItemType} is not implemented");
        }

        this.item = Resources.Load<ItemScriptableObject>(path);

        if (item == null)
        {
            throw new ArgumentException($"No item found for path {path}");
        }
    }

    private void SetSprite()
    {
        spriteRenderer.sprite = item.Sprite;
    }
}
