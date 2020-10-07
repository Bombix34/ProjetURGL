using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private Transform m_Target;
    public Vector3 m_CameraOffset;
    public float m_Smoothness = 0.3f;

    private void Update()
    {
        if (m_Target == null )
            return;
        Vector3 newPosition = m_Target.position + m_CameraOffset;
        this.transform.position = Vector3.Slerp(m_Target.transform.position, newPosition, m_Smoothness);
    }

    public void StartCameraFollow(Transform target)
    {
        m_Target = target;
    }
}
