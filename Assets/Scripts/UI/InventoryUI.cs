using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    private Inventory inventory;
    private RectTransform rectTransform;

    private readonly Dictionary<ItemScriptableObject, GameObject> _itemUIDict = new Dictionary<ItemScriptableObject, GameObject>();

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        StartCoroutine(nameof(Init));
    }

    IEnumerator Init()
    {
        yield return new WaitUntil(() => Inventory.Instance != null);
        inventory = Inventory.Instance;
        foreach (var item in inventory.Items)
        {
            this.AddItem(item);
        }
        inventory.OnItemAdded += AddItem;
        inventory.OnItemRemoved += RemoveItem;
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
