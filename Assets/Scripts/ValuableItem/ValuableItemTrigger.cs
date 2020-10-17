using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValuableItemTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag(Tags.THIEF_TAG))
        {
            return;
        }
        if (col.GetComponent<Inventory>().HasValuableItem)
        {
            VictoryScreen.Instance.Init(VictoryScreen.VictoryType.THIEFS_VICTORY);
        }
    }
}
