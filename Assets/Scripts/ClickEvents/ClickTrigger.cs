using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TriggerTag))]
public class ClickTrigger : MonoBehaviour
{
    [SerializeField]
    private ActionsData actionDatas;
    private TriggerTag tagTrigger;
    [SerializeField]
    private SpriteRenderer[] spriteRenderers;

    [SerializeField]
    private HighlightSettings highlightSettings;

    private GameObject feedbackRadiusContainer;
    private float lineWidth=0.03f;

    public float AreaRange { get; set; } = 1.4f;


    [SerializeField]
    private Material interactMaterial;

    private void Start()
    {
        tagTrigger = GetComponent<TriggerTag>();
        feedbackRadiusContainer = GetComponentInChildren<SpriteRenderer>().gameObject;
        feedbackRadiusContainer.SetActive(false);
    }

    private void Update()
    {
        if (feedbackRadiusContainer == null)
            return;
        SpriteRenderer rend = feedbackRadiusContainer.GetComponent<SpriteRenderer>();
        float alpha = Mathf.PingPong(Time.time*0.8f, 0.6f);
        rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, alpha);
    }


    public void OnClickObject(PlayerManager currentPlayer)
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
        }
    }

    public void OnMouseOverTrigger(PlayerManager currentPlayer)
    {
        if(PlayerCanInteract(currentPlayer))
        {
            for (int i = 0; i < spriteRenderers.Length; ++i)
            {
                spriteRenderers[i].material = interactMaterial;
            }
            spriteRenderers[0].material.SetFloat("_Thickness", highlightSettings.size);
            spriteRenderers[0].material.SetColor("_SolidOutline", highlightSettings.onMouseOverColor);
        }
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
        Color finalColor = isInRange ? highlightSettings.onCanInteractColor : highlightSettings.onMouseClickColor;
        DrawFeedbackRadius(finalColor);
        spriteRenderers[0].material.SetColor("_SolidOutline", finalColor);
    }


    private bool PlayerCanInteract(PlayerManager currentPlayer)
    {
        FieldOfView fieldViewManager = Camera.main.GetComponent<CameraManager>().FieldOfView;
        if (!fieldViewManager.IsObjectVisibleFromPlayer(currentPlayer.gameObject, this.transform.parent.gameObject))
            return false;
        bool isPlayerVigil = currentPlayer.IsVigil;
        TriggerTagType playerTag = isPlayerVigil ? TriggerTagType.VIGIL : TriggerTagType.THIEF;
        bool result = actionDatas.IsActionValid(playerTag, tagTrigger.TagType);
        return result;
    }


    public void DrawFeedbackRadius(Color color)
    {
        feedbackRadiusContainer.SetActive(true);
        SpriteRenderer rendererFeedback = feedbackRadiusContainer.GetComponent<SpriteRenderer>();
        feedbackRadiusContainer.transform.localScale = new Vector2(AreaRange, AreaRange);
        Color finalColor = new Color(color.r, color.g, color.b, 0.6f);
        rendererFeedback.color = finalColor;
    }

}

public enum TriggerClickType
{
    PNJ,
    THIEF,
    VIGIL
}
