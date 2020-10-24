using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New CameraConfig", menuName = "URGL/Camera/CameraConfig")]
public class CameraConfigScriptableObject : ScriptableObject
{
    [SerializeField]
    private float offsetZ;
    [SerializeField]
    private float smoothness;
    [SerializeField]
    private float introSmoothness;
    [SerializeField]
    private Vector2 introStartPosition;
    [SerializeField]
    private float introDurationOnValuableObject;

    public float OffsetZ { get => offsetZ; set => offsetZ = value; }
    public float Smoothness { get => smoothness; set => smoothness = value; }
    public float IntroSmoothness { get => introSmoothness; set => introSmoothness = value; }
    public Vector2 IntroStartPosition { get => introStartPosition; set => introStartPosition = value; }
    public float IntroDurationOnValuableObject { get => introDurationOnValuableObject; set => introDurationOnValuableObject = value; }
}
