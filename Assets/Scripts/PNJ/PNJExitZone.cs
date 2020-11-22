using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PNJExitZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.IsTagValid(TagSelection.PNJ))
        {
            collision.GetComponent<PNJManager>().GetCaught();
        }
    }
}
