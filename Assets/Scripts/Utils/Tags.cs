using System;
using UnityEngine;

public static class Tags
{
    [Serializable]
    public enum TagSelection
    {
        THIEF,
        VIGIL,
        PNJ,
        ITEM
    }
    public const string THIEF_TAG = "Thief";
    public const string VIGIL_TAG = "Vigil";
    public const string PNJ_TAG = "PNJ";
    public const string ITEM_TAG = "Item";

    public static bool IsTagValid(this Collider2D collider2D, TagSelection tagSelection)
    {
        switch (tagSelection)
        {
            case TagSelection.THIEF:
                return collider2D.CompareTag(THIEF_TAG);
            case TagSelection.VIGIL:
                return collider2D.CompareTag(VIGIL_TAG);
            case TagSelection.PNJ:
                return collider2D.CompareTag(PNJ_TAG);
            case TagSelection.ITEM:
                return collider2D.CompareTag(ITEM_TAG);
            default:
                throw new NotImplementedException($"No implementation for tag {tagSelection}");
        }
    }
}
