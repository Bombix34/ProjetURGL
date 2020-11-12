using Mirror;
using System;
using UnityEngine;

public class NetworkItem : NetworkBehaviour, IItemContainer
{
    [SerializeField]
    [NotNull]
    protected ItemScriptableObject item;

    [SyncVar(hook = nameof(ChangeItem))]
    private string itemName;
    [SerializeField]
    [NotNull]
    private SpriteRenderer spriteRenderer;
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
                path = $"Items/NormalItems/{newValue}";
                break;
            case ItemType.VALUABLE_ITEM:
                path = $"Items/ValuableItems/{newValue}";
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
