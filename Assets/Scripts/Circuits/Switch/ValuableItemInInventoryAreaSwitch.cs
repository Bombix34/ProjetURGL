using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValuableItemInInventoryAreaSwitch : AreaSwitch
{
    internal override bool PlayerRespectCondition(GameObject gameObject)
    {
        return gameObject.GetComponent<Inventory>()?.HasValuableItem ?? false;
    }
}
