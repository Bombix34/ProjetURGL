using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CameraHelper {
    public static float GetRadius(Camera camera) {
        float cameraRadius = camera.orthographicSize;
        if (camera.pixelWidth > camera.pixelHeight) {
            cameraRadius *= (float)camera.pixelWidth / camera.pixelHeight;
        }
        cameraRadius = Mathf.Sqrt(cameraRadius * cameraRadius + cameraRadius * cameraRadius);

        return(cameraRadius);
    }

    static private Polygon2D polygon = null;
    static public Polygon2D GetPolygon() {
        if (polygon == null) {
            polygon = new Polygon2D();
            polygon.AddPoint(0, 0);
            polygon.AddPoint(0, 0);
            polygon.AddPoint(0, 0);
            polygon.AddPoint(0, 0);
        }

        return(polygon);
    }

    static public Polygon2D GetWorldPolyon(Camera camera) {
        float cameraSizeY = camera.orthographicSize;
        float cameraSizeX = cameraSizeY * (float)camera.pixelWidth / camera.pixelHeight;

        float sizeX = cameraSizeX * 2;
        float sizeY = cameraSizeY * 2;

        float x = -sizeX / 2;
        float y = -sizeY / 2;

        Polygon2D polygon = GetPolygon();

        polygon.pointsList[0].x = x;
        polygon.pointsList[0].y = y;

        polygon.pointsList[1].x = x + sizeX;
        polygon.pointsList[1].y = y;

        polygon.pointsList[2].x = x + sizeX;
        polygon.pointsList[2].y = y + sizeY;

        polygon.pointsList[3].x = x;
        polygon.pointsList[3].y = y + sizeY;

        polygon.ToRotationItself(camera.transform.rotation.eulerAngles.z * Mathf.Deg2Rad);
        polygon.ToOffsetItself(camera.transform.position);

        return(polygon);
    }

     public static Rect GetWorldRect(Camera camera) {
        Polygon2D polygon= GetWorldPolyon(camera);

        Rect rect = polygon.GetRect();
   
        return(rect);
    }
}
