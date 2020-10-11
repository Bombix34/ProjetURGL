using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInInventoryAreaSwitch : AreaSwitch
{
    [SerializeField]
    private string itemNameToActivate;

    public string ItemNameToActivate => itemNameToActivate;

    internal override bool PlayerRespectCondition(GameObject gameObject)
    {
        return gameObject.GetComponent<Inventory>()?.HasItem(this.ItemNameToActivate) ?? false;
    }
}
