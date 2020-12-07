using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : NetworkBehaviour
{
    [SerializeField]
    [NotNull]
    private GameObject normalItemPrefab = null;
    [SerializeField]
    [NotNull]
    private GameObject valuableItemPrefab = null;
    private readonly SyncList<ItemScriptableObject> _items = new SyncList<ItemScriptableObject>();
    [SerializeField]
    private List<ItemScriptableObject> defaultItemsInInventory = new List<ItemScriptableObject>();

    public static Inventory Instance { get; private set; }
    public bool HasValuableItem => _items.Any(q => q.IsValuableItem);

    public Action<ItemScriptableObject> OnItemAdded { get; set; }

    public Action<ItemScriptableObject> OnItemRemoved { get; set; }

    public List<ItemScriptableObject> Items => _items.ToList();

    public override void OnStartServer()
    {
        this._items.AddRange(defaultItemsInInventory);
    }
    public override void OnStartClient()
    {
        _items.Callback += this.OnInventoryUpdated;
    }

    public override void OnStartAuthority()
    {
        Instance = this;
    }

    public override void OnStopAuthority()
    {
        Instance = null;
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

    public void DropAllItems(bool deathDrop = false)
    {
        var itemListCopy = _items.ToList();
        foreach (var item in itemListCopy)
        {
            this.DropItem(item, deathDrop);
        }
    }


    [Server]
    public void DropItem(ItemScriptableObject item, bool deathDrop)
    {
        if (PedestalManager.Instance.ExistPedestalForItem(item) == false)
        {
            return;
        }
        GameObject prefabToInstantiate;
        switch (item.Type)
        {
            case ItemType.NORMAL_ITEM:
                prefabToInstantiate = normalItemPrefab;
                break;
            case ItemType.VALUABLE_ITEM:
                prefabToInstantiate = valuableItemPrefab;
                break;
            default:
                throw new NotImplementedException($"The item type {item.Type} is not implemented");
        }

        this.InstantiateItem(prefabToInstantiate, item, deathDrop);
        this.RemoveItem(item);
    }

    private void InstantiateItem(GameObject prefabToInstantiate, ItemScriptableObject item, bool deathDrop)
    {
        Vector3 position = transform.position;
        if (deathDrop)
        {
            position = PedestalManager.Instance.GetPedestalForItem(item).ItemInitialPosition;
        }

        var itemGameObject = Instantiate(prefabToInstantiate, position, transform.rotation);
        itemGameObject.GetComponent<IItemContainer>().Item = item;
        NetworkServer.Spawn(itemGameObject);
    }

    public bool HasItem(string itemName)
    {
        return this._items.Any(item => item.ItemName == itemName);
    }

    public ItemScriptableObject GetItemByName(string itemName)
    {
        return this._items.SingleOrDefault(item => item.ItemName == itemName);
    }

    public ItemScriptableObject GetValuableItem()
    {
        return this._items.SingleOrDefault(item => item.IsValuableItem);
    }

    void OnInventoryUpdated(SyncList<ItemScriptableObject>.Operation op, int index, ItemScriptableObject oldItem, ItemScriptableObject newItem)
    {
        switch (op)
        {
            case SyncList<ItemScriptableObject>.Operation.OP_ADD:
            case SyncList<ItemScriptableObject>.Operation.OP_INSERT:
                OnItemAdded?.Invoke(newItem);
                break;
            case SyncList<ItemScriptableObject>.Operation.OP_REMOVEAT:
                OnItemRemoved?.Invoke(oldItem);
                break;
        }
    }

}
