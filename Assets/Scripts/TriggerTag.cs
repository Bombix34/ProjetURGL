using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTag : MonoBehaviour
{
    [SerializeField]
    private TriggerTagType tagType;

    public bool IsTagCorresponding(TriggerTagType tag)
    {
        return tagType == tag;
    }

    public TriggerTagType TagType
    {
        get => tagType;
    }
}

public enum TriggerTagType
{
    THIEF,
    VIGIL,
    PNJ,
    ACCESS_CARD
}
