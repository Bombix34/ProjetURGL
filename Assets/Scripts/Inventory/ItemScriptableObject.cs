using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Item")]
public class ItemScriptableObject : ScriptableObject
{
    [System.Serializable]
    public enum ItemType
    {
        NORMAL_ITEM,
        VALUABLE_ITEM
    }

    [SerializeField]
    private string itemName = "ItemName";
    [SerializeField]
    private ItemType type = ItemType.NORMAL_ITEM;

    public string ItemName { get => itemName; }
    public ItemType Type { get => type; }
    public bool IsValuableItem { get => this.type == ItemType.VALUABLE_ITEM; }
}
