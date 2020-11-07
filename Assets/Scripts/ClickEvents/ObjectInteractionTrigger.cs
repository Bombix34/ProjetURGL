using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteractionTrigger : MonoBehaviour
{
    private ClickTrigger clickTriggerManager;

    public bool PlayerIsInRange { get; set; } = false;

    private void Awake()
    {
        clickTriggerManager = GetComponentInParent<ClickTrigger>();
    }

    #region ON_TRIGGER_FUNCTIONS

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;
        if(clickTriggerManager.CurrentPlayerInteract!= null && obj == clickTriggerManager.CurrentPlayerInteract)
        {
            PlayerIsInRange = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;
        if (clickTriggerManager.CurrentPlayerInteract != null)
        {
            if(obj == clickTriggerManager.CurrentPlayerInteract)
                PlayerIsInRange = false;
        }
        else
        {
            PlayerIsInRange = false;
        }
    }

    #endregion
}
