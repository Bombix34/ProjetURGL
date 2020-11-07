using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField]
    [NotNull]
    private Texture2D cursorBase = null;
    [SerializeField]
    [NotNull]
    private Texture2D cursorOnInteract = null;

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
