using Mirror;
using UnityEngine;

public class NetworkItem : NetworkBehaviour, IItemContainer
{
    private const string RESOURCE_NORMAL_ITEM_PATH = "Items/NormalItems/";
    private const string RESOURCE_VALUABLE_ITEM_PATH = "Items/ValuableItems/";
    [SerializeField]
    [NotNull]
    [SyncVar(hook = nameof(ChangeItem))]
    protected ItemScriptableObject item;

    [SerializeField]
    [NotNull]
    private SpriteRenderer spriteRenderer = null;
    public ItemScriptableObject Item
    {
        get => item;
        set
        {
            item = value;
        }
    }
    public virtual ItemType ItemType => ItemType.NORMAL_ITEM;

    public override void OnStartServer()
    {
        if (PedestalManager.Instance)
        {
            PedestalManager.Instance.GetPedestalForItem(item)?.SetNetworkItem(this);
        }

    }

    private void ChangeItem(ItemScriptableObject oldValue, ItemScriptableObject newValue)
    {
        this.SetSprite();
    }

    private void SetSprite()
    {
        spriteRenderer.sprite = item.Sprite;
    }
}
