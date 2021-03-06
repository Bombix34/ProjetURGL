﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "URGL/Highlight Settings")]
public class HighlightSettings : ScriptableObject
{
    public float size;
    public Color onMouseOverColor;
    public Color onMouseClickColor;
    public Color onCanInteractColor;
}
