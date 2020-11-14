using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionZone : MonoBehaviour
{
    public GameObject PlayerController { get; set; }
    private FieldOfView fieldViewManager;
    private CircleCollider2D collider2d;

    private List<GameObject> visibleObjects;

    public bool HasAutority { get; set; } = false;

    private void Awake()
    {
        visibleObjects = new List<GameObject>();
        fieldViewManager = GetComponent<FieldOfView>();
        collider2d = GetComponent<CircleCollider2D>();
    }

    #region ON_TRIGGER_FUNCTIONS

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;
        if (obj == PlayerController)
            return;
        TriggerEnterZone(obj);
        UpdateVisibleObjects();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;
        if (obj == PlayerController || visibleObjects.Contains(obj))
            return;
        TriggerEnterZone(obj);
        UpdateVisibleObjects();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;
        if (obj == PlayerController)
            return;
        TriggerExitZone(obj);
        UpdateVisibleObjects();
    }

    #endregion

    private void TriggerEnterZone(GameObject obj)
    {
        if (obj.CompareTag("Item"))
        {
            if (!fieldViewManager.IsObjectVisibleFromPlayer(PlayerController, obj))
                return;
            visibleObjects.Add(obj);
            obj.GetComponentInChildren<ClickTrigger>().SetupFogShader(false);
        }
        else if (obj.CompareTag("PNJ") || obj.CompareTag("Thief"))
        {
            if (!fieldViewManager.IsObjectVisibleFromPlayer(PlayerController, obj))
                return;
            visibleObjects.Add(obj);
            obj.GetComponentInChildren<CharacterRenderer>().SwitchVisibility(true);
        }
        else if (obj.CompareTag("Vigil"))
        {
            if (!fieldViewManager.IsObjectVisibleFromPlayer(PlayerController, obj))
                return;
            visibleObjects.Add(obj);
            obj.GetComponentInChildren<CharacterRenderer>().SwitchVisibility(true);
        }
        else if(obj.CompareTag("Door"))
        {
            if (!fieldViewManager.IsObjectVisibleFromPlayer(PlayerController, obj))
                return;
            visibleObjects.Add(obj);
            obj.GetComponentInParent<FogVisibilityRenderer>().SwitchVisibility(true);
        }
    }

    private void TriggerExitZone(GameObject obj)
    {
        if (obj.CompareTag("Item"))
        {
            visibleObjects.Remove(obj);
            obj.GetComponentInChildren<ClickTrigger>().SetupFogShader(true);
        }
        else if (obj.CompareTag("PNJ") || obj.CompareTag("Thief"))
        {
            visibleObjects.Remove(obj);
            if (obj.GetComponentInChildren<CharacterRenderer>() != null)
                obj.GetComponentInChildren<CharacterRenderer>().SwitchVisibility(false);
        }
        else if (obj.CompareTag("Vigil"))
        {
            visibleObjects.Remove(obj);
            if (obj.GetComponentInChildren<CharacterRenderer>() != null)
                obj.GetComponentInChildren<CharacterRenderer>().SwitchVisibility(false);
        }
        else if (obj.CompareTag("Door"))
        {
            visibleObjects.Remove(obj);
            obj.GetComponentInParent<FogVisibilityRenderer>().SwitchVisibility(false);
        }
    }

    public bool IsObjectVisible(GameObject obj)
    {
        return visibleObjects.Contains(obj);
    }

    private void UpdateVisibleObjects()
    {
        List<GameObject> toRemove = new List<GameObject>();
        foreach(var obj in visibleObjects)
        {
            if (!fieldViewManager.IsObjectVisibleFromPlayer(PlayerController, obj))
            {
                toRemove.Add(obj);
            }
        }
        foreach(var old in toRemove)
        {
            TriggerExitZone(old);
        }
        UpdateThiefWithValuableItemInRange();
    }

    //detection si le joueur est un vigile qu'un voleur qui a volé est visible
    private void UpdateThiefWithValuableItemInRange()
    {
        if (!PlayerController.CompareTag("Vigil"))
            return;
        VigilManager vigilManager = PlayerController.GetComponent<VigilManager>();
        foreach (var obj in visibleObjects)
        {
            if(obj.CompareTag("Thief"))
            {
                Inventory thiefInventory = obj.GetComponent<Inventory>();
                //voleur en vue
                if (thiefInventory != null && thiefInventory.HasValuableItem)
                {
                    //verification de si le voleur était en vue aussi a la frame précédente
                    if (vigilManager.WarningFeedback == null)
                    {
                        GameObject feedback = Instantiate(Resources.Load("FX/WarningFeedback", typeof(GameObject)) as GameObject, PlayerController.transform);
                        feedback.transform.parent = PlayerController.transform;
                        vigilManager.WarningFeedback = feedback;
                        return;
                    }
                    else
                        return;
                }
            }
        }
        //voleur était en vue mais ne l'est plus
        if (vigilManager.WarningFeedback != null)
        {
            vigilManager.WarningFeedback.GetComponent<WarningFeedback>().DestroyFeedback();
            vigilManager.WarningFeedback = null;
        }
    }

    public float ColliderRadius
    {
        set
        {
            if (collider2d == null)
                collider2d = GetComponent<CircleCollider2D>();
            collider2d.radius = value;
        }
    }

    public int ObjectsCount
    {
        get => visibleObjects.Count;
    }


}
