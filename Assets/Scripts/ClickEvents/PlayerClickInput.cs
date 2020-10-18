using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.EventSystems;

public class PlayerClickInput : NetworkBehaviour
{
    [SerializeField]
    private LayerMask clickLayer;
    private PlayerManager manager;

    public ClickTrigger CurrentObjectOver { get; private set; } = null;
    public ClickTrigger CurrentObjectClicked { get; private set; } = null;

    private bool isObjectClickedInActionRange = false;

    public Event currentEvent;

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
        if(CurrentObjectClicked!=null)
        {
            if (CurrentObjectClicked == manager.GetComponentInChildren<ClickTrigger>())
            { 
                CurrentObjectClicked = null;
                return;
            }
            FieldOfView fieldViewManager = Camera.main.GetComponent<CameraManager>().FieldOfView;
            if (!fieldViewManager.IsObjectVisibleFromPlayer(manager.gameObject, CurrentObjectClicked.transform.parent.gameObject))
            {
                CurrentObjectClicked.OnMouseExitTrigger();
                CurrentObjectClicked = null;
            }
            else
            {
                isObjectClickedInActionRange = Vector2.Distance(manager.transform.position, CurrentObjectClicked.transform.parent.position) <= 1.4f;
                CurrentObjectClicked.PlayerInActionRange(isObjectClickedInActionRange);
            }
        }
        else if(CurrentObjectOver!=null)
        {
            FieldOfView fieldViewManager = Camera.main.GetComponent<CameraManager>().FieldOfView;
            if (!fieldViewManager.IsObjectVisibleFromPlayer(manager.gameObject, CurrentObjectOver.transform.parent.gameObject))
            {
                CurrentObjectOver.OnMouseExitTrigger();
                CurrentObjectOver = null;
            }
        }
    }

    private void ClickUpdate()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(CurrentObjectOver != null)
            {
                if (CurrentObjectOver == CurrentObjectClicked || CurrentObjectOver == manager.GetComponentInChildren<ClickTrigger>())
                    return;
                if(CurrentObjectClicked!=null)
                    CurrentObjectClicked.OnMouseExitTrigger();
                CurrentObjectClicked = CurrentObjectOver;
                CurrentObjectOver = null;
                CurrentObjectClicked.OnClickObject(manager);
            }
            else if(CurrentObjectClicked!=null)
            {
                CurrentObjectClicked.OnMouseExitTrigger();
                CurrentObjectClicked = null;
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
                if(CurrentObjectOver != null && CurrentObjectOver.gameObject != hit.collider.gameObject)
                    CurrentObjectOver.OnMouseExitTrigger();
                CurrentObjectOver = hit.collider.gameObject.GetComponent<ClickTrigger>();
                if(CurrentObjectClicked==null)
                    CurrentObjectOver.OnMouseOverTrigger(manager);
            }
            else
            {
                if (CurrentObjectOver != null )
                {
                    if(CurrentObjectOver != CurrentObjectClicked)
                        CurrentObjectOver.OnMouseExitTrigger();
                }
                CurrentObjectOver = null;
            }
        }
    }
}
