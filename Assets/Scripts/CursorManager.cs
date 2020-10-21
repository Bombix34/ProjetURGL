using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField]
    private Texture2D cursorBase, cursorOnInteract;

    private void Awake()
    {
        SwitchCursor(CursorType.BASIC);
    }

    public void SwitchCursor(CursorType type)
    {
        switch(type)
        {
            case CursorType.BASIC:
                Cursor.SetCursor(cursorBase, Vector2.zero, CursorMode.Auto);
                break;
            case CursorType.ON_INTERACT:
                Cursor.SetCursor(cursorOnInteract, Vector2.zero, CursorMode.Auto);
                break;
        }
    }


    public enum CursorType
    {
        BASIC,
        ON_INTERACT
    }
}
