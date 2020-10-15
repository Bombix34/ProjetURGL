using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using DG.Tweening;

public class CharacterRenderer : NetworkBehaviour
{
    [SyncVar]
    private bool isRendererFlip = false;

    [SerializeField]
    private SpriteRenderer bodyRenderer;

    private bool IsVisible = true;

    private void Update()
    {
        if (isRendererFlip)
            transform.localScale = new Vector3(-1f, 1f, 1f);
        else
            transform.localScale = new Vector3(1f, 1f, 1f);
    }

    public bool IsRendererFlip
    {
        get => isRendererFlip;
        set
        {
            isRendererFlip = value;
        }
    }

    public void SwitchMaterial(Material newMat, bool isVisible=true)
    {
        //PASSAGE A LETAT INVISIBLE
        if(!isVisible && this.IsVisible)
        {
            Color finalColor = new Color(bodyRenderer.color.r, bodyRenderer.color.g, bodyRenderer.color.b, 0f);
            bodyRenderer.DOColor(finalColor, 0.7f)
                .OnComplete(()=> bodyRenderer.material = newMat);
            this.IsVisible = isVisible;
        }
        //PASSAGE A LETAT VISIBLE
        else if(isVisible && !this.IsVisible)
        {
            Color finalColor = new Color(bodyRenderer.color.r, bodyRenderer.color.g, bodyRenderer.color.b, 1f);
            bodyRenderer.DOColor(finalColor, 0.7f);
            this.IsVisible = isVisible;
            bodyRenderer.material = newMat;
        }
    }
}
