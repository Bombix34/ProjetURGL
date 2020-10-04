using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerSettings")]
public class PlayerSettings : ScriptableObject
{
    [Range(0f,15f)]
    public float MovementSpeed;
}
