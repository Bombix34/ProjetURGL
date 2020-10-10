using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValuableItem : MonoBehaviour
{
    public ItemScriptableObject Item { get; set; }

    void OnTriggerStay2D(Collider2D col)
    {
        if (!col.CompareTag(Tags.THIEF_TAG))
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            //On ramasse l'item
        }
    }
}
