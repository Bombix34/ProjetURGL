using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ItemUI : MonoBehaviour
{
    public void Init(ItemScriptableObject item)
    {
        var spriteRenderer = GetComponent<Image>();
        spriteRenderer.sprite = item.Sprite;
    }
}
