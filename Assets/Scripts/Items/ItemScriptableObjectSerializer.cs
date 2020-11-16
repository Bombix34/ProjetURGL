using Mirror;
using System;
using System.IO;
using UnityEngine;

public static class ItemScriptableObjectSerializer
{
    private const string RESOURCE_NORMAL_ITEM_PATH = "Items/NormalItems/";
    private const string RESOURCE_VALUABLE_ITEM_PATH = "Items/ValuableItems/";
    public static void WriteItemScriptableObject(this NetworkWriter writer, ItemScriptableObject item)
    {
        string path;

        switch (item.Type)
        {
            case ItemType.NORMAL_ITEM:
                path = Path.Combine(RESOURCE_NORMAL_ITEM_PATH, item.name);
                break;
            case ItemType.VALUABLE_ITEM:
                path = Path.Combine(RESOURCE_VALUABLE_ITEM_PATH, item.name);
                break;
            default:
                throw new NotImplementedException($"The item type {item.Type} is not implemented");
        }
        writer.WriteString(path);
    }

    public static ItemScriptableObject ReadArmor(this NetworkReader reader)
    {
        var resource = Resources.Load<ItemScriptableObject>(reader.ReadString());
        return resource;
    }
}
