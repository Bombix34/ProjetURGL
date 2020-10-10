using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Settings")]
public class GameSettings : ScriptableObject
{
    [Header("REGLAGES PREFABS JOUEURS")]
    public PlayerSettings VoleurSettings;
      public PlayerSettings AgentSettings;
    [Header("REGLAGES PARTIE")]
    public int PlayerNumber = 1;
    public int AgentNumber = 1;
}
