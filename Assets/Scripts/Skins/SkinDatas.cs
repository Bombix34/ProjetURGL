using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skins datas")]
public class SkinDatas : ScriptableObject
{
    public List<Color> colors;
    public List<Sprite> heads;


    public Color agentColor;
    public Sprite agentHead;
}
