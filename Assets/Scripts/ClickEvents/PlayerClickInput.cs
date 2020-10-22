using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.EventSystems;

public class PlayerClickInput : NetworkBehaviour
{
    [SerializeField]
    private LayerMask clickLayer;
    private ActionUI interactionUI;

    private PlayerManager manager;

    private CursorManager cursor;

    public ClickTrigger CurrentObjectOver { get; private set; } = null;
    public ClickTrigger CurrentObjectClicked { get; private set; } = null;

    private bool isObjectClickedInActionRange = false;

    public Event currentEvent;

    private void Start()
    {
        manager = this.GetComponent<PlayerManager>();
        cursor = Camera.main.GetComponent<CursorManager>();
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
                ActivateUIFeedback(false);
                CurrentObjectClicked = null;
            }
            else
            {
                isObjectClickedInActionRange = Vector2.Distance(manager.transform.position, CurrentObjectClicked.transform.parent.position) <= 1.4f;
                interactionUI.EnableButton(isObjectClickedInActionRange);
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
                if (!CurrentObjectClicked.OnClickObject(manager))
                {
                    CurrentObjectClicked = null;
                    ActivateUIFeedback(false);
                }
                else
                {
                    ActivateUIFeedback(true);
                }
            }
            else if(CurrentObjectClicked!=null)
            {
                CurrentObjectClicked.OnMouseExitTrigger();
                ActivateUIFeedback(false);
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
                {
                    if (!CurrentObjectOver.OnMouseOverTrigger(manager))
                        CurrentObjectOver = null;
                    else
                        cursor.SwitchCursor(CursorManager.CursorType.ON_INTERACT);
                }
            }
            else
            {
                if (CurrentObjectOver != null )
                {
                    if(CurrentObjectOver != CurrentObjectClicked)
                        CurrentObjectOver.OnMouseExitTrigger();
                }
                CurrentObjectOver = null;
                cursor.SwitchCursor(CursorManager.CursorType.BASIC);
            }
        }
    }

    private void ActivateUIFeedback(bool isActive)
    {
        if(isActive)
        {
            if (interactionUI == null)
                interactionUI = Instantiate(Resources.Load<GameObject>("ToInstantiate/ActionCanvas")).GetComponent<ActionUI>();
            interactionUI.gameObject.SetActive(true);
        }
        else
        {
            interactionUI?.gameObject.SetActive(false);
        }
    }
}
