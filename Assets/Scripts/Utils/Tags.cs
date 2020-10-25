using System;
using System.Collections.Generic;
using UnityEngine;
public static class Tags
{
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

    public static bool IsTagValid(this GameObject gameObject, List<TagSelection> tagSelections)
    {
        foreach (var tagSelection in tagSelections)
        {
            if (gameObject.IsTagValid(tagSelection))
            {
                return true;
            }
        }
        return false;
    }

    public static bool IsTagValid(this GameObject gameObject, TagSelection tagSelection)
    {
        switch (tagSelection)
        {
            case TagSelection.THIEF:
                return gameObject.CompareTag(THIEF_TAG);
            case TagSelection.VIGIL:
                return gameObject.CompareTag(VIGIL_TAG);
            case TagSelection.PNJ:
                return gameObject.CompareTag(PNJ_TAG);
            case TagSelection.ITEM:
                return gameObject.CompareTag(ITEM_TAG);
            default:
                throw new NotImplementedException($"No implementation for tag {tagSelection}");
        }
    }
}
