using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName="FafaTools/Achievements")]
public class AchievementsScriptableObject : ScriptableObject
{
    public List<Achievement> achievements = new List<Achievement>();
}