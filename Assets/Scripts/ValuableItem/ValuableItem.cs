using Mirror;
using System;
using UnityEngine;

public class ValuableItem : NetworkBehaviour
{
    [SerializeField]
    private ItemScriptableObject item;

    public ItemScriptableObject Item { get => item; set => item = value; }

    void OnTriggerStay2D(Collider2D col)
    {
        if (!col.CompareTag(Tags.THIEF_TAG))
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            this.CmdTakeValuableItem();
        }
    }

    [Command(ignoreAuthority = true)]
    public void CmdTakeValuableItem(NetworkConnectionToClient sender = null)
    {
        var inventory = sender.identity.GetComponent<Inventory>();
        if (inventory.AddItem(Item))
        {
            Destroy(this.gameObject);
        }
    }
}
