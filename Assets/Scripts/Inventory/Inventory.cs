using Mirror;
using System.Linq;
using UnityEngine;

public class Inventory : NetworkBehaviour
{
    [SerializeField]
    private GameObject valuableItemPrefab;

    public static Inventory Instance { get; private set; }
    public SyncList<ItemScriptableObject> Items { get; } = new SyncList<ItemScriptableObject>();
    private bool HasValuableItem => Items.Any(q => q.IsValuableItem);
    public override void OnStartClient()
    {
        Instance = this;
    }

    [Server]
    public bool AddItem(ItemScriptableObject item)
    {
        if (item.IsValuableItem && this.HasValuableItem)
        {
            return false;
        }
        
        this.Items.Add(item);
        return true;
    }

    [Server]
    public void RemoveItem(ItemScriptableObject item)
    {
        this.Items.Remove(item);
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
        itemGameObject.GetComponent<ValuableItem>().Item = item;
        NetworkServer.Spawn(itemGameObject);
    }

    public bool HasItem(string ItemName)
    {
        return this.Items.Any(item => item.ItemName == ItemName);
    }

}
