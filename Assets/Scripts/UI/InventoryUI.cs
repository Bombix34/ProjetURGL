using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;

    private Inventory inventory;
    private RectTransform rectTransform;

    private readonly Dictionary<ItemScriptableObject, GameObject> _itemUIDict = new Dictionary<ItemScriptableObject, GameObject>();

    private void Start()
    {
        Instance = this;
        rectTransform = GetComponent<RectTransform>();
        StartCoroutine(nameof(Init));
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    IEnumerator Init()
    {
        yield return new WaitUntil(() => Inventory.Instance != null);
        this.ChangeInventory(Inventory.Instance);
    }

    public void ChangeInventory(Inventory newInventory)
    {
        if(this.inventory != null)
        {
            this.inventory.OnItemAdded -= AddItem;
            this.inventory.OnItemRemoved -= RemoveItem;

            var itemUIKeys = _itemUIDict.Keys;
            foreach (var key in itemUIKeys)
            {
                this.RemoveItem(key);
            }
        }

        this.inventory = newInventory;

        foreach (var item in this.inventory.Items)
        {
            this.AddItem(item);
        }
        this.inventory.OnItemAdded += AddItem;
        this.inventory.OnItemRemoved += RemoveItem;
    } 

    private void AddItem(ItemScriptableObject item)
    {
        var inventoryUIPrefab = Resources.Load<GameObject>("UI/Item/ItemPanel");
        var inventoryUIGameObject = Instantiate(inventoryUIPrefab, transform);
        var itemUI = inventoryUIGameObject.GetComponent<ItemUI>();
        itemUI.Init(item);
        this._itemUIDict.Add(item, inventoryUIGameObject);
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
    }
    private void RemoveItem(ItemScriptableObject item)
    {
        Destroy(_itemUIDict[item]);
        _itemUIDict.Remove(item);
    }
}
