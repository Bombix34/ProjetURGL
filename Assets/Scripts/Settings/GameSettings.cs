using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "URGL/Game Settings")]
public class GameSettings : ScriptableObject
{
    [Header("REGLAGES PREFABS JOUEURS")]
    public PlayerSettings VoleurSettings;
    public PlayerSettings AgentSettings;
    public List<string> GameScenes;
    public float CameraDistance;
    public bool PlayIntroduction;
}
