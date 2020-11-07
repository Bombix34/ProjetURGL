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
        UpdateVisibleObjects();
        TriggerEnterZone(obj);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;
        if (obj == PlayerController || visibleObjects.Contains(obj))
            return;
        UpdateVisibleObjects();
        TriggerEnterZone(obj);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;
        if (obj == PlayerController)
            return;
        UpdateVisibleObjects();
        TriggerExitZone(obj);
    }

    #endregion

    private void TriggerEnterZone(GameObject obj)
    {
        if (obj.CompareTag("Item"))
        {
            if (!fieldViewManager.IsObjectVisibleFromPlayer(PlayerController, obj))
                return;
            visibleObjects.Add(obj);
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
    }

    private void TriggerExitZone(GameObject obj)
    {
        if (obj.CompareTag("Item"))
        {
            visibleObjects.Remove(obj);
        }
        else if (obj.CompareTag("PNJ") || obj.CompareTag("Thief"))
        {
            visibleObjects.Remove(obj);
            obj.GetComponentInChildren<CharacterRenderer>().SwitchVisibility(false);
        }
        else if (obj.CompareTag("Vigil"))
        {
            visibleObjects.Remove(obj);
            obj.GetComponentInChildren<CharacterRenderer>().SwitchVisibility(false);
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
