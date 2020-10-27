using UnityEngine;
using System.Collections.Generic;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public LayerMask obstacleMask;


    public bool IsObjectVisibleFromPlayer(GameObject player, GameObject target)
    {
        float distancePlayerTarget = Vector2.Distance(player.transform.position, target.transform.position);
        if (distancePlayerTarget > viewRadius || !IsRendererVisibleFromCameraView(target))
            return false;
        else
        {
            Vector2 dir = (target.transform.position - player.transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(player.transform.position, dir, viewRadius, obstacleMask);
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


    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dst;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
        {
            hit = _hit;
            point = _point;
            dst = _dst;
            angle = _angle;
        }
    }
}