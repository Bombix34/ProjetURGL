using System.Collections.Generic;
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

    public string ItemName = "ItemName";
    public ItemType Type = ItemType.NORMAL_ITEM;
    public bool IsValuableItem => this.Type == ItemType.VALUABLE_ITEM;

    public override bool Equals(object obj)
    {
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        var item = (ItemScriptableObject)obj;

        return this.ItemName == item.ItemName;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
