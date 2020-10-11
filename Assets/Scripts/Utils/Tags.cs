using System;
using UnityEngine;

public static class Tags
{
    [Serializable]
    public enum TagSelection
    {
        THIEF_AND_VIGIL,
        THIEF,
        VIGIL,
    }
    public const string THIEF_TAG = "Thief";
    public const string VIGIL_TAG = "Vigil";

    public static bool IsTagValid(this Collider2D collider2D, TagSelection tagSelection)
    {
        switch (tagSelection)
        {
            case TagSelection.THIEF:
                return collider2D.CompareTag(THIEF_TAG);
            case TagSelection.VIGIL:
                return collider2D.CompareTag(VIGIL_TAG);
            case TagSelection.THIEF_AND_VIGIL:
                return collider2D.CompareTag(THIEF_TAG) || collider2D.CompareTag(VIGIL_TAG);
            default:
                throw new NotImplementedException($"No implementation for tag {tagSelection}");
        }
    }
}
