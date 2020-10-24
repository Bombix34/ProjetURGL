using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "URGL/PlayerSettings")]
public class PlayerSettings : ScriptableObject
{
    [Header("PLAYER TYPE")]
    public PlayerType PlayerType;
    [Header("PREFAB")]
    public GameObject PlayerPrefab;

    [Header("MOVEMENT SETTINGS")]
    [Range(0f, 15f)]
    public float MovementSpeed;

    [Range(0f, 1f)]
    public float MovementSpeedWithValuableItemMultiplier;
}
