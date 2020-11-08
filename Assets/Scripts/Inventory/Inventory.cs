using Mirror;
using System.Linq;
using UnityEngine;

public class Inventory : NetworkBehaviour
{
    [SerializeField]
    private GameObject valuableItemPrefab = null;
    private readonly SyncList<ItemScriptableObject> _items = new SyncList<ItemScriptableObject>();

    public static Inventory Instance { get; private set; }
    public bool HasValuableItem => _items.Any(q => q.IsValuableItem);
    public override void OnStartClient()
    {
        Instance = this;
    }

    [Server]
    public bool AddItem(ItemScriptableObject item)
    {
        if (_items.Contains(item))
        {
            return false;
        }
        if (item.IsValuableItem && this.HasValuableItem)
        {
            return false;
        }

        this._items.Add(item);
        return true;
    }

    [Server]
    public void RemoveItem(ItemScriptableObject item)
    {
        this._items.Remove(item);
    }


    [Server]
    public void DropItem(ItemScriptableObject item)
    {
        switch (item.Type)
        {
            case ItemScriptableObject.ItemType.NORMAL_ITEM:
                break;
            case ItemScriptableObject.ItemType.VALUABLE_ITEM:
                this.InstantiateValuableItem(item);
                this.RemoveItem(item);
                break;
            default:
                break;
        }
    }

    [Server]
    public void InstantiateValuableItem(ItemScriptableObject item)
    {
        var itemGameObject = Instantiate(valuableItemPrefab, transform.position, transform.rotation);
        itemGameObject.GetComponent<IItemContainer>().Item = item;
        NetworkServer.Spawn(itemGameObject);
    }

    public bool HasItem(string ItemName)
    {
        return this._items.Any(item => item.ItemName == ItemName);
    }

    public ItemScriptableObject GetValuableItem()
    {
        return this._items.SingleOrDefault(item => item.IsValuableItem);
    }

}
