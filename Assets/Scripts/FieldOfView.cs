using UnityEngine;
using System.Collections.Generic;

public class FieldOfView : MonoBehaviour
{
    private float viewSize;
    [Range(0, 360)]
    public float viewAngle;

    public LayerMask obstacleMask;

    private LightingSource2D[] lightingSources;

    private void Awake()
    {
        lightingSources = GetComponentsInChildren<LightingSource2D>();
    }

    public bool IsObjectVisibleFromPlayer(GameObject player, GameObject target)
    {
        float distancePlayerTarget = Vector2.Distance(player.transform.position, target.transform.position);
        Vector2 dir = (target.transform.position - player.transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(player.transform.position, dir, viewSize, obstacleMask);
        if (hit && hit.collider.gameObject != target && hit.collider.gameObject != player)
        {
            float distancePlayerHit = Vector2.Distance(player.transform.position, hit.point);
            if (distancePlayerHit < distancePlayerTarget)
            {
                return false;
            }
            else
                return true;
        }
        else
            return true;

    }

    private bool IsRendererVisibleFromCameraView(GameObject target)
    {
        if (target.GetComponentInChildren<SpriteRenderer>() != null)
        {
            if (!target.GetComponentInChildren<SpriteRenderer>().isVisible)
                return false;
        }
        else if (target.GetComponentInChildren<Transform>().GetComponentInChildren<SpriteRenderer>() != null)
        {
            if (!target.GetComponentInChildren<Transform>().GetComponentInChildren<SpriteRenderer>().isVisible)
                return false;
        }
        return true;
    }

    public float ViewSize
    {
        set
        {
            viewSize = value;
            foreach (var light in lightingSources)
                light.size = viewSize;
        }
        get => viewSize;
    }
}