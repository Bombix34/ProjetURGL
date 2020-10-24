using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickTrigger : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer[] spriteRenderers;

    [SerializeField]
    private HighlightSettings highlightSettings;

    private GameObject feedbackRadiusContainer;
    private float lineWidth=0.03f;

    public bool IsInRange { get; set; } = false;

    public float AreaRange { get; set; } = 1.4f;
    public BaseSwitch CurrentInteractionAvailable { get; private set; }


    [SerializeField]
    private Material interactMaterial;

    private void Start()
    {
        feedbackRadiusContainer = GetComponentInChildren<SpriteRenderer>().gameObject;
        feedbackRadiusContainer.SetActive(false);
    }

    private void Update()
    {
        if (feedbackRadiusContainer == null || !feedbackRadiusContainer.activeInHierarchy)
            return;
        SpriteRenderer rend = feedbackRadiusContainer.GetComponent<SpriteRenderer>();
        float alpha = Mathf.PingPong(Time.time*0.8f, 0.6f);
        rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, alpha);
    }

    /// <summary>
    /// return true if player can interact with object
    /// return false if placer can't interact
    /// </summary>
    /// <param name="currentPlayer"></param>
    /// <returns></returns>
    public bool OnClickObject(PlayerManager currentPlayer)
    {
        if (PlayerCanInteract(currentPlayer))
        {
            if(spriteRenderers[0].material!=interactMaterial)
            {
                for (int i = 0; i < spriteRenderers.Length; ++i)
                {
                    spriteRenderers[i].material = interactMaterial;
                }
            }
            spriteRenderers[0].material.SetFloat("_Thickness", highlightSettings.size);
            spriteRenderers[0].material.SetColor("_SolidOutline", highlightSettings.onMouseClickColor);
            DrawFeedbackRadius(highlightSettings.onMouseClickColor);
            return true;
        }
        return false;
    }

    /// <summary>
    /// return true if player can interact
    /// return false if player can't
    /// </summary>
    /// <param name="currentPlayer"></param>
    /// <returns></returns>
    public bool OnMouseOverTrigger(PlayerManager currentPlayer)
    {
        if(PlayerCanInteract(currentPlayer))
        {
            for (int i = 0; i < spriteRenderers.Length; ++i)
            {
                spriteRenderers[i].material = interactMaterial;
            }
            spriteRenderers[0].material.SetFloat("_Thickness", highlightSettings.size);
            spriteRenderers[0].material.SetColor("_SolidOutline", highlightSettings.onMouseOverColor);
            return true;
        }
        return false;
    }

    public void OnMouseExitTrigger()
    {
        spriteRenderers[0].material.SetFloat("_Thickness", 0f);
        spriteRenderers[0].material.SetFloat("_OutlineMode", 0);
        spriteRenderers[0].material.SetFloat("_Angle", 0f);
        spriteRenderers[0].material.SetColor("_SolidOutline", highlightSettings.onMouseOverColor);
        feedbackRadiusContainer.SetActive(false);
    }

    public void PlayerInActionRange(bool isInRange)
    {
        if(isInRange && !this.IsInRange || !isInRange && this.IsInRange)
        {
            this.IsInRange = isInRange;
            Color finalColor = isInRange ? highlightSettings.onCanInteractColor : highlightSettings.onMouseClickColor;
            SpriteRenderer rendererFeedback = feedbackRadiusContainer.GetComponent<SpriteRenderer>();
            DrawFeedbackRadius(new Color(finalColor.r,finalColor.g,finalColor.b,rendererFeedback.color.a));
            spriteRenderers[0].material.SetColor("_SolidOutline", finalColor);
        }
    }


    private bool PlayerCanInteract(PlayerManager currentPlayer)
    {
        FieldOfView fieldViewManager = Camera.main.GetComponent<CameraManager>().FieldOfView;
        if (!fieldViewManager.IsObjectVisibleFromPlayer(currentPlayer.gameObject, this.transform.parent.gameObject))
            return false;
        bool result = false;
        BaseSwitch[] allSwitch = transform.parent.GetComponents<BaseSwitch>();
        foreach(var switchActiv in allSwitch)
        {
            if(currentPlayer.gameObject.IsTagValid(switchActiv.TagSelection))
            {
                CurrentInteractionAvailable = switchActiv;
                result = true;
            }
        }
        if (!result)
            CurrentInteractionAvailable = null;
        return result;
    }

    public void PlayerInteract()
    {
        CurrentInteractionAvailable?.OnActivate();
        CurrentInteractionAvailable = null;
    }


    public void DrawFeedbackRadius(Color color)
    {
        feedbackRadiusContainer.SetActive(true);
        SpriteRenderer rendererFeedback = feedbackRadiusContainer.GetComponent<SpriteRenderer>();
        feedbackRadiusContainer.transform.localScale = new Vector2(AreaRange, AreaRange);
        Color finalColor = new Color(color.r, color.g, color.b, 0f);
        rendererFeedback.color = finalColor;
    }

}

public enum TriggerClickType
{
    PNJ,
    THIEF,
    VIGIL
}
