using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "URGL/Skins datas")]
public class SkinDatas : ScriptableObject
{
    public List<Color> colors;

    public List<RuntimeAnimatorController> animatorController;
}
