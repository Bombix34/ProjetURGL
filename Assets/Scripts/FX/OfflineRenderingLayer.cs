using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfflineRenderingLayer : MonoBehaviour
{
    SpriteRenderer spriteRender;
    public bool isParentTransform;
    public float modificator;

    public bool onlyOnStart = false;

    protected virtual void Start()
    {
        spriteRender = GetComponent<SpriteRenderer>();
        UpdateRendererLayer();
    }

    private void Update()
    {
        if (onlyOnStart)
            return;
        UpdateRendererLayer();
    }

    private void UpdateRendererLayer()
    {
        if (isParentTransform)
            spriteRender.sortingOrder = (int)(((1 / transform.parent.position.y) * 1000) + modificator);
        else
            spriteRender.sortingOrder = (int)(((1 / transform.position.y) * 1000) + modificator);
    }
}
