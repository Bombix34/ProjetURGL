using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private Transform m_Target;
    public float m_CameraOffsetZ;
    public float m_Smoothness = 0.3f;

    private void Update()
    {
        if (m_Target == null )
            return;
        Vector3 newPosition = new Vector3(m_Target.position.x, m_Target.position.y, m_CameraOffsetZ);
       // this.transform.position = Vector3.Slerp(m_Target.transform.position, newPosition, m_Smoothness);
        this.transform.position = Vector2.Lerp(m_Target.transform.position, newPosition, m_Smoothness);
        this.transform.position = new Vector3(transform.position.x, transform.position.y, m_CameraOffsetZ);
    }

    public void StartCameraFollow(Transform target)
    {
        m_Target = target;
    }
}
