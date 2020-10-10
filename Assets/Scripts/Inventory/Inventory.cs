using Mirror;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private readonly SyncList<ItemScriptableObject> items = new SyncList<ItemScriptableObject>();

    public SyncList<ItemScriptableObject> Items => items; 
    public bool HasValuableItem { get; private set; } = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool AddItem(ItemScriptableObject item)
    {
        if (item.IsValuableItem)
        {
            if (this.HasValuableItem)
            {
                return false;
            }
            this.HasValuableItem = true;
        }
        this.Items.Add(item);
        return true;
    }
    public bool RemoveItem(ItemScriptableObject item)
    {
        this.Items.Remove(item);
        if (item.IsValuableItem)
        {
            this.HasValuableItem = false;
        }
        return true;
    }



}
