using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "URGL/Item/Item")]
[Serializable]
public class ItemScriptableObject : ScriptableObject
{
    [SerializeField]
    private string itemName = null;
    [SerializeField]
    private ItemType type = ItemType.NORMAL_ITEM;
    [SerializeField]
    [NotNull]
    private Sprite sprite = null;

    public string ItemName { get => itemName; }
    public ItemType Type { get => type; }
    public Sprite Sprite { get => sprite; }
    public bool IsValuableItem => this.Type == ItemType.VALUABLE_ITEM;
}
