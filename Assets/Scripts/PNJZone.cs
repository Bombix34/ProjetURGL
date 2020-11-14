using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PNJZone : MonoBehaviour
{
    [SerializeField]
    private ZoneType ZoneType;
    [SerializeField]
    private Vector2 rectangleSize;

    [SerializeField]
    private float circleRadius;


    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 0.4f, 0.8f, 0.15f);
        if(ZoneType==ZoneType.RECTANGLE)
            Gizmos.DrawCube(transform.position, new Vector3(rectangleSize.x, rectangleSize.y, 0.01f));
        else if (ZoneType == ZoneType.CIRCLE)
            Gizmos.DrawSphere(transform.position, circleRadius);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 0.2f, 1f, 0.5f);
        if (ZoneType == ZoneType.RECTANGLE)
            Gizmos.DrawCube(transform.position, new Vector3(rectangleSize.x, rectangleSize.y, 0.01f));
        else if (ZoneType == ZoneType.CIRCLE)
            Gizmos.DrawSphere(transform.position, circleRadius);
    }

}

public enum ZoneType
{
    CIRCLE,
    RECTANGLE
}
