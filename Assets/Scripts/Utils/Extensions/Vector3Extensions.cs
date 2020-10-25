using UnityEngine;

public static class Vector3Extensions
{
    public static Vector3 ToVector3WithNewZ(this Vector3 vector3, float z = 0)
    {
        return new Vector3(vector3.x, vector3.y, z);
    }
}
