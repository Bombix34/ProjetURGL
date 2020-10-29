﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using DG.Tweening;

public class CharacterRenderer : NetworkBehaviour
{
    public static string MOVE_TRIGGER = "MOVE";
    public static string IDLE_TRIGGER = "IDLE";
    public static string FLIP_BOOL = "FLIP_X";
    [SyncVar]
    private bool isRendererFlip = false;

    private Animator animator;

    [SerializeField]
    private SpriteRenderer bodyRenderer;


    private bool IsVisible = true;

    private void Awake()
    {
        animator = GetComponentInParent<Animator>();
    }

    public bool IsRendererFlip
    {
        get => isRendererFlip;
        set
        {
            isRendererFlip = value;
            if (animator?.GetBool(FLIP_BOOL) == !isRendererFlip)
            {
                animator.SetBool(FLIP_BOOL, isRendererFlip);
            }
        }
    }

    public void SwitchVisibility(bool isVisible)
    {
        //PASSAGE A LETAT INVISIBLE
        if(!isVisible && this.IsVisible)
        {
            Color finalColor = new Color(bodyRenderer.color.r, bodyRenderer.color.g, bodyRenderer.color.b, 0f);
            bodyRenderer.DOColor(finalColor, 0.7f)
                .OnComplete(() =>
                SetupFogShader(true)
            );
            this.IsVisible = isVisible;
        }
        //PASSAGE A LETAT VISIBLE
        else if(isVisible && !this.IsVisible)
        {
            SetupFogShader(false);
            bodyRenderer.color = new Color(bodyRenderer.color.r, bodyRenderer.color.g, bodyRenderer.color.b, 0f);
            Color finalColor = new Color(bodyRenderer.color.r, bodyRenderer.color.g, bodyRenderer.color.b, 1f);
            bodyRenderer.DOColor(finalColor, 0.7f);
            this.IsVisible = isVisible;
        }
    }

    private void SetupFogShader(bool isFogShaderOn)
    {
        bodyRenderer.GetComponent<FogOfWarSprite>().enabled = isFogShaderOn;
        bodyRenderer.enabled = !isFogShaderOn;
    }
}
