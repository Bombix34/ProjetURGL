using UnityEngine;

[CreateAssetMenu(menuName = "URGL/PlayerSettings")]
public class PlayerSettings : ScriptableObject
{
    [Header("PLAYER TYPE")]
    public PlayerType PlayerType;
    [Header("PREFAB")]
    public GameObject PlayerPrefab;

    [Header("MOVEMENT SETTINGS")]
    [Range(0f, 500f)]
    public float MovementSpeed;

    [Header("MOVEMENT PNJ SETTINGS")]
    public float pnjSpeedMultiplicator;


    [Header("FOG OF WAR SETTINGS")]
    [Range(0f, 50f)]
    public float FogOfWarSize;
}
