using System.Collections;
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
    [NotNull]
    private SpriteRenderer bodyRenderer = null;


    private bool IsVisible = false;

    private void Awake()
    {
        animator = GetComponentInParent<Animator>();
    }

    public void InitPlayerCharacterRenderer()
    {
        FogOfWarSprite[] fogofwarSprites = GetComponentsInChildren<FogOfWarSprite>();
        foreach(var fog in fogofwarSprites)
        {
            fog.enabled = false;
            fog.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
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
            SetupFogShader(true);
        }
        //PASSAGE A LETAT VISIBLE
        else if(isVisible && !this.IsVisible)
        {
            SetupFogShader(false);
        }
        this.IsVisible = isVisible;
    }

    public void ActiveRenderer(bool isActive)
    {
        SetupFogShader(isActive);
        bodyRenderer.enabled = isActive;
        animator.enabled = isActive;
    }

    private void SetupFogShader(bool isFogShaderOn)
    {
        bodyRenderer.GetComponent<FogOfWarSprite>().enabled = isFogShaderOn;
        bodyRenderer.enabled = !isFogShaderOn;
    }
}
