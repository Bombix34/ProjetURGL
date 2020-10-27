using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LightEventListener : MonoBehaviour {
    public bool useDistance = false;
    public float visability = 0;
    
    public LightCollision2D collision2DInfo = new LightCollision2D();

    private LightingCollider2D lightCollider;

    void OnEnable() {
        lightCollider = GetComponent<LightingCollider2D>();
        lightCollider.AddEvent(collisionEvent);
    }

    void collisionEvent(LightCollision2D collision) {
        if (collision.points != null) {
            if (collision2DInfo.state == LightingEventState.None) {
                collision2DInfo = collision;

            } else {
                if (collision2DInfo.points != null) { //?
                    if (collision.points.Count >= collision2DInfo.points.Count) {
                        collision2DInfo = collision;
                    } else if (collision2DInfo.light == collision.light) {
                        collision2DInfo = collision;
                    }
                }
            }

        } else {
            collision2DInfo.state = LightingEventState.None;
        }
    }

    void Update() {
        visability = 0;

        if (collision2DInfo.state == LightingEventState.None) {
            return;
        }

        if (collision2DInfo.points != null) {
            Polygon2D polygon = lightCollider.mainShape.GetPolygonsLocal()[0];

            int pointsCount = polygon.pointsList.Count;
            int pointsInView = collision2DInfo.points.Count;

            visability = (((float)pointsInView / pointsCount));

            if (useDistance) {
                if (collision2DInfo.points.Count > 0) {
                    float multiplier = 0;

                    foreach(Vector2 point in collision2DInfo.points) {
                        float distance = Vector2.Distance(Vector2.zero, point);
                        float pointMultipler = ( 1 - (distance / collision2DInfo.light.size) ) * 2;

                        if (pointMultipler > 1) {
                            pointMultipler = 1;
                        }

                        if (pointMultipler < 0) {
                            pointMultipler = 0;
                        }

                        multiplier += pointMultipler;
                    }

                    multiplier /= collision2DInfo.points.Count;

                    visability *= multiplier;
                }
            }
        }
    
        collision2DInfo.state = LightingEventState.None;
    }
}
