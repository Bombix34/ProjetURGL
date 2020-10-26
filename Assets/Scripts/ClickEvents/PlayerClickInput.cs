using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.EventSystems;

public class PlayerClickInput : NetworkBehaviour
{
    private const float CLICK_DISTANCE = 1.4f;
    [SerializeField]
    private LayerMask clickLayer;
    [SerializeField]
    private ActionsConfigurationScriptableObject actionsConfiguration = null;
    private ActionUI interactionUI;

    private PlayerManager manager;
    private ClickTrigger managerClickTrigger;

    private CursorManager cursor;

    public ClickTrigger CurrentObjectOver { get; private set; } = null;
    public ClickTrigger CurrentObjectClicked { get; private set; } = null;

    private bool isObjectClickedInActionRange = false;

    public Event currentEvent;

    private void Start()
    {
        manager = this.GetComponent<PlayerManager>();
        this.managerClickTrigger = manager.GetComponentInChildren<ClickTrigger>();
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

    public void TryPerformInteraction()
    {
        if(CurrentObjectClicked == null || this.CanDoAction() == false)
        {
            return;
        }
        this.CanDoAction();
        StartCoroutine(this.actionsConfiguration.GetActionWaitCoroutine(CurrentObjectClicked.CurrentInteractionAvailable.Activatable.ActionType).Wait());
        CurrentObjectClicked.PlayerInteract();
    }

    private bool CanDoAction()
    {
        if(CurrentObjectClicked.CurrentInteractionAvailable == null)
        {
            CurrentObjectClicked = null;
            return false;
        }
        isObjectClickedInActionRange = Vector2.Distance(manager.transform.position, CurrentObjectClicked.transform.parent.position) <= CLICK_DISTANCE;
        bool canDoAction = this.actionsConfiguration.CanDoAction(CurrentObjectClicked.CurrentInteractionAvailable.Activatable.ActionType);
        return isObjectClickedInActionRange && canDoAction;
    }

    private void DistanceTest()
    {
        if (CurrentObjectClicked != null)
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
                interactionUI?.EnableButton(this.CanDoAction());
                CurrentObjectClicked?.PlayerInActionRange(isObjectClickedInActionRange);
            }
        }
        else if (CurrentObjectOver != null)
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
        if (Input.GetMouseButtonDown(0))
        {
            this.OnMouseClick();
        }
    }

    private void OnMouseClick()
    {
        if (CurrentObjectOver != null)
        {
            this.ClickHoverGameObject();
        }
        else if (CurrentObjectClicked != null)
        {
            CurrentObjectClicked.OnMouseExitTrigger();
            ActivateUIFeedback(false);
            CurrentObjectClicked = null;
        }
    }

    private void ClickHoverGameObject()
    {
        if (CurrentObjectOver == CurrentObjectClicked || CurrentObjectOver == managerClickTrigger)
        {
            return;
        }

        if (CurrentObjectClicked != null)
        {
            CurrentObjectClicked.OnMouseExitTrigger();
        }

        CurrentObjectClicked = CurrentObjectOver;
        CurrentObjectOver = null;

        bool canInteract = CurrentObjectClicked.OnClickObject(manager);
        if (canInteract)
        {
            ActivateUIFeedback(true);
        }
        else
        {
            CurrentObjectClicked = null;
            ActivateUIFeedback(false);
        }
    }

    #region RaycastUpdate

    private void RaycastUpdate()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(worldPosition.x, worldPosition.y), Vector2.zero, 0, clickLayer);
        {
            if (hit)
            {
                OnRaycastHit(hit.collider.gameObject);
            }
            else
            {
                OnRaycastNotHit();
            }
        }
    }

    private void OnRaycastHit(GameObject hitGameObject)
    {
        CheckHitAnotherGameObject(hitGameObject);
        CurrentObjectOver = hitGameObject.GetComponent<ClickTrigger>();

        if (CurrentObjectClicked != null)
        {
            return;
        }

        if (!CurrentObjectOver.OnMouseOverTrigger(manager))
        {
            CurrentObjectOver = null;
        }
        else
        {
            cursor.SwitchCursor(CursorManager.CursorType.ON_INTERACT);
        }
    }

    private void CheckHitAnotherGameObject(GameObject hitGameObject)
    {
        bool hasHitAnotherGameObject = CurrentObjectOver != null && CurrentObjectOver.gameObject != hitGameObject;
        if (hasHitAnotherGameObject)
        {
            CurrentObjectOver.OnMouseExitTrigger();
        }
    }

    private void OnRaycastNotHit()
    {
        if (CurrentObjectOver != null && CurrentObjectOver != CurrentObjectClicked)
        {
            CurrentObjectOver.OnMouseExitTrigger();
        }
        CurrentObjectOver = null;
        cursor.SwitchCursor(CursorManager.CursorType.BASIC);
    }

    #endregion RaycastUpdate

    private void ActivateUIFeedback(bool isActive)
    {
        interactionUI?.gameObject.SetActive(false);
        return;
        if (isActive)
        {
            if (interactionUI == null)
            {
                interactionUI = Instantiate(Resources.Load<GameObject>("ToInstantiate/ActionCanvas")).GetComponent<ActionUI>();
                interactionUI.Init(this);
            }

            interactionUI.gameObject.SetActive(true);
        }
        else
        {
            interactionUI?.gameObject.SetActive(false);
        }
    }
}
