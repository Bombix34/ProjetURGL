using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerSettings")]
public class PlayerSettings : ScriptableObject
{
    [Header("PREFAB")]
    public GameObject PlayerPrefab;

    [Header("MOVEMENT SETTINGS")]
    [Range(0f,15f)]
    public float MovementSpeed;
}
