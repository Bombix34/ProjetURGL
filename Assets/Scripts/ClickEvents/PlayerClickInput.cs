using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerClickInput : NetworkBehaviour
{
    [SerializeField]
    private LayerMask clickLayer;
    private PlayerManager manager;

    private ClickTrigger currentObjectOver = null;
    private ClickTrigger currentObjectClicked = null;

    private void Start()
    {
        manager = this.GetComponent<PlayerManager>();
    }

    private void Update()
    {
        if (!hasAuthority)
            return;
        DistanceTest();
        ClickUpdate();
    }

    private void FixedUpdate()
    {
        if (!hasAuthority)
            return;
        RaycastUpdate();
    }

    private void DistanceTest()
    {
        if(currentObjectClicked!=null)
        {
            FieldOfView fieldViewManager = Camera.main.GetComponent<CameraManager>().FieldOfView;
            if (!fieldViewManager.IsPlayerCanInteractWithObject(manager.gameObject, currentObjectClicked.transform.parent.gameObject))
            {
                currentObjectClicked.OnMouseExitTrigger();
                currentObjectClicked = null;
            }
        }
        else if(currentObjectOver!=null)
        {
            FieldOfView fieldViewManager = Camera.main.GetComponent<CameraManager>().FieldOfView;
            if (!fieldViewManager.IsPlayerCanInteractWithObject(manager.gameObject, currentObjectOver.transform.parent.gameObject))
            {
                currentObjectOver.OnMouseExitTrigger();
                currentObjectOver = null;
            }
        }
    }

    private void ClickUpdate()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(currentObjectOver != null)
            {
                if(currentObjectClicked!=null)
                    currentObjectClicked.OnMouseExitTrigger();
                currentObjectClicked = currentObjectOver;
                currentObjectOver = null;
                currentObjectClicked.OnClickObject(manager);
            }
            else if(currentObjectClicked!=null)
            {
                currentObjectClicked.OnMouseExitTrigger();
                currentObjectClicked = null;

            }
        }
    }

    private void RaycastUpdate()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(worldPosition.x, worldPosition.y), Vector2.zero, 0, clickLayer);
        {
            if (hit)
            {
                if(currentObjectOver != null && currentObjectOver.gameObject != hit.collider.gameObject)
                    currentObjectOver.OnMouseExitTrigger();
                currentObjectOver = hit.collider.gameObject.GetComponent<ClickTrigger>();
                if(currentObjectClicked==null)
                    currentObjectOver.OnMouseOverTrigger(manager);
            }
            else
            {
                if (currentObjectOver != null )
                {
                    if(currentObjectOver != currentObjectClicked)
                        currentObjectOver.OnMouseExitTrigger();
                }
                currentObjectOver = null;
            }
        }
    }
}
