using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName="FafaTools/Scorer")]
public class ScoresScriptableObject : ScriptableObject
{
    public List<Score> scores = new List<Score>();
}