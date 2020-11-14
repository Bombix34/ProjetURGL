using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "URGL/Item/Item")]
[System.Serializable]
public class ItemScriptableObject : ScriptableObject
{
    [SerializeField]
    private string itemName = null;
    [SerializeField]
    private ItemType type = ItemType.NORMAL_ITEM;
    [SerializeField]
    [NotNull]
    private Sprite sprite = null;
    private Pedestal pedestal;

    public string ItemName { get => itemName; }
    public ItemType Type { get => type; }
    public Sprite Sprite { get => sprite; }
    public bool IsValuableItem => this.Type == ItemType.VALUABLE_ITEM;
    public bool CanBeDrop => this.Pedestal != null;
    public Pedestal Pedestal { get => pedestal; set => pedestal = value; }

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
